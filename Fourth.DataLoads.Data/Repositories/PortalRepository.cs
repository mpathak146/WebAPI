﻿using Fourth.DataLoads.Data.Interfaces;
using log4net;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System;
using Fourth.DataLoads.Data.Entities;
using Fourth.Orchestration.Model.People;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.ComponentModel;
using System.Xml.Linq;
using System.Xml;
using Fourth.DataLoads.Data.Models;

namespace Fourth.DataLoads.Data.SqlServer
{
    internal class PortalRepository : IPortalRepository
    {
        private IDBContextFactory _contextFactory;

        /// <summary> The log4net Logger instance. </summary>
        private static readonly ILog Logger =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public PortalRepository(IDBContextFactory _contextFactory)
        {
            this._contextFactory = _contextFactory;
        }
        public bool ProcessMassTerminate(MassTerminationModelSerialized employee, Commands.CreateAccount payload)
        {
            int groupID = int.Parse(payload.EmailAddress);
            Logger.InfoFormat("Validating a given employee number");
            using (var context = _contextFactory.GetPortalDBContextAsync(groupID))
            {
                try
                {
                    var command = "sprc_DL_ProcessMassTermination";

                    using (var sqlConnection = new SqlConnection(context.Result.Database.Connection.ConnectionString))
                    {
                        sqlConnection.Open();
                        var cmd = sqlConnection.CreateCommand();
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.CommandText = command;
                        cmd.Parameters.Add(new SqlParameter("@BatchID", employee.DataLoadBatchId));
                        cmd.Parameters.Add(new SqlParameter("@JobID", employee.DataLoadJobId));
                        cmd.Parameters.Add(new SqlParameter("@EmployeeNumber", employee.EmployeeNumber));
                        cmd.Parameters.Add(new SqlParameter("@TerminationReason", employee.TerminationReason));
                        cmd.Parameters.Add(new SqlParameter("@TerminationDate", employee.TerminationDate));
                        cmd.Parameters.Add(new SqlParameter("@UploadedBy", employee.DataLoadBatchId));

                        var result = cmd.ExecuteScalar();

                        if (result.GetType() != typeof(DBNull))
                        {
                            if ((int?)result == 1)
                            {
                                Logger.InfoFormat("No Error reported for EmployeeNumber: {0} for GroupID: {1}",
                                    new object[] { employee, groupID });
                                return true;
                            }
                            else
                            {
                                Logger.ErrorFormat("Error in processing Mass Terminate for EmployeeNumber: {0} for GroupID: {1}",
                                    new object[] { employee, groupID });
                                return false;
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Logger.FatalFormat("Error in processing Mass Terminate, " +
                        "Exception Message: {0}, Inner Exception Message: {1}",
                        e.Message, e.InnerException == null ? "null" : e.InnerException.Message);
                    return false;
                }

            }
            return true;
        }
        public void DumpStagingErrorsToPortal(Commands.CreateAccount payload)
        {
            int groupID = int.Parse(payload.EmailAddress);
            Logger.InfoFormat("Validating a given employee number");
            using (var context = _contextFactory.GetPortalDBContextAsync(groupID))
            {
                using (var stagingContext = _contextFactory.GetStagingDBContext())
                {
                    var errors = (from err in stagingContext.DataLoadErrors
                                  where (err.DataLoadBatchRefId.ToString() == payload.FirstName)
                                  select err)
                                 .AsEnumerable()
                                 .Select(x => new
                                 {
                                     MassTerminationId = "",
                                     DataLoadJobRefId = x.DataLoadJobRefId,
                                     DataLoadBatchRefId = x.DataLoadBatchRefId,
                                     ClientID = 0,
                                     EmployeeNumber = GetParam(x.ErrRecord, "EmployeeNumber"),
                                     TerminationDate = GetParam(x.ErrRecord, "TerminationDate"),
                                     TerminationReason = GetParam(x.ErrRecord, "TerminationReason"),
                                     ErrorStatus = 2,
                                     ErrorDescription = x.ErrDescription
                                 });
                    using (var sqlConnection = new SqlConnection(context.Result.Database.Connection.ConnectionString))
                    {
                        using (SqlBulkCopy bulkCopy = new SqlBulkCopy(sqlConnection))
                        {
                            bulkCopy.DestinationTableName = "dbo.t_DL_MassTermination";
                            try
                            {
                                sqlConnection.Open();
                                var bulk = ConvertToDataTable(errors.ToList());
                                bulkCopy.WriteToServer(bulk);
                            }
                            catch (Exception e)
                            {
                                Logger.FatalFormat("Error doing bulk upload into t_MassTermination with message {0}", e.Message);
                            }
                            finally
                            {
                                sqlConnection.Close();
                            }
                        }
                    }
                }
            }
        }

        private string GetParam(string errRecord, string v)
        {
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(errRecord);
            XmlNodeList xnList = xml.SelectNodes("/MassTerminationModelSerialized");
            var emp = xnList[0][v].InnerText;
            return emp;
        }

        private DataTable ConvertToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties =
               TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }

        public bool DumpDataloadBatchToPortal(Commands.CreateAccount payload)
        {
            int groupID = int.Parse(payload.EmailAddress);
            using (var context = _contextFactory.GetPortalDBContextAsync(groupID))
            {
                var command = "dbo.sprc_DL_InsertPayload";

                using (var sqlConnection = new SqlConnection(context.Result.Database.Connection.ConnectionString))
                {
                    try
                    {
                        sqlConnection.Open();
                        var cmd = sqlConnection.CreateCommand();
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.CommandText = command;
                        cmd.Parameters.Add(new SqlParameter("@BatchID", payload.FirstName));
                        cmd.Parameters.Add(new SqlParameter("@JobID", payload.LastName));
                        cmd.Parameters.Add(new SqlParameter("@DataloadTypeID", DataLoadTypes.MASS_TERMINATION));
                        cmd.Parameters.Add(new SqlParameter("@DateUploaded", DateTime.Now));
                        cmd.Parameters.Add(new SqlParameter("@UploadedBy", payload.CustomerId));
                        var result = cmd.ExecuteScalar();

                        if (result.GetType() != typeof(DBNull))
                        {
                            if ((int?)result == 1)
                            {
                                Logger.InfoFormat("No Error reported for BatchNumber: {0} ",
                                    new object[] { payload.FirstName });
                                return true;
                            }
                            else
                            {
                                Logger.ErrorFormat("Error reported for inside the proc {0} BatchNumber: {1} ",
                                    new object[] { command, payload.FirstName });
                                return false;
                            }
                        }
                        return false;
                    }
                    catch (Exception e)
                    {
                        Logger.FatalFormat("Error in processing payload insert, " +
                        "Exception Message: {0}, Inner Exception Message: {1}",
                        e.Message, e.InnerException == null ? "null" : e.InnerException.Message);
                        return false;
                    }
                }
            }
        }

        public async Task<IEnumerable<DataLoadUploads>> GetDataLoadUploads
            (int groupID, string dateFrom)
        {
            List<DataLoadUploads> uploads = new List<DataLoadUploads>();

            using (var context = _contextFactory.GetPortalDBContextAsync(groupID))
            {
                string command;
                if (dateFrom != string.Empty)
                {
                    command = string.Format(@"SELECT DL.DataLoadJobRefId, DL.DataLoadBatchRefId, dlt.DataloadName,DL.UploadedBy, DL.DateUploaded 
                        FROM t_DL_DataLoad DL left join t_DL_DataloadType dlt ON dl.DataloadTypeID = dlt.DataloadTypeID
                        WHERE DL.DateUploaded > '{0}' 
                        GROUP BY DL.DataLoadJobRefId, DL.DateUploaded,DL.DataLoadBatchRefId, dlt.DataloadName,DL.UploadedBy
                        HAVING DL.DateUploaded = (SELECT MAX(D.DateUploaded) FROM t_DL_DataLoad D 
                        WHERE D.DataLoadJobRefId = DL.DataLoadJobRefId GROUP BY D.DataLoadJobRefId)", dateFrom);
                }
                else
                {
                    command = string.Format(@"SELECT DL.DataLoadJobRefId, DL.DataLoadBatchRefId, dlt.DataloadName,DL.UploadedBy, DL.DateUploaded 
                        FROM t_DL_DataLoad DL left join t_DL_DataloadType dlt ON dl.DataloadTypeID = dlt.DataloadTypeID
                        GROUP BY DL.DataLoadJobRefId, DL.DateUploaded,DL.DataLoadBatchRefId, dlt.DataloadName,DL.UploadedBy
                        HAVING DL.DateUploaded = (SELECT MAX(D.DateUploaded) FROM t_DL_DataLoad D 
                        WHERE D.DataLoadJobRefId = DL.DataLoadJobRefId GROUP BY D.DataLoadJobRefId)", dateFrom);
                }

                using (var sqlConnection = new SqlConnection(context.Result.Database.Connection.ConnectionString))
                {
                    try
                    {
                        sqlConnection.Open();
                        var cmd = sqlConnection.CreateCommand();
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.CommandText = command;
                        var result = await cmd.ExecuteReaderAsync();
                        if (result.GetType() != typeof(DBNull))
                        {
                            while(result.Read()!=false)
                            {
                                uploads.Add(new DataLoadUploads()
                                {
                                    jobID=result.GetString(0),
                                    DataloadType = result.GetString(2),
                                    UploadedBy = result.GetString(3),
                                    DateUploaded = DateTime.Parse(result.GetValue(4).ToString())
                                });
                            }

                        }
                        return uploads.AsEnumerable();
                    }
                    catch (Exception e)
                    {
                        Logger.FatalFormat("Error in retrieval for Dataload records, " +
                        "Exception Message: {0}, Inner Exception Message: {1}",
                        e.Message, e.InnerException == null ? "null" : e.InnerException.Message);
                        return null;
                    }
                }
            }
        }

        public async Task<IEnumerable<ErrorModel>> GetDataLoadErrors
            (int groupID, string jobID)
        {
            List<ErrorModel> uploads = new List<ErrorModel>();

            using (var context = _contextFactory.GetPortalDBContextAsync(groupID))
            {
                string command=string.Empty;                
                    command = string.Format(@"
                            select ClientID,EmployeeNumber, ErrorStatus,ErrorDescription 
                            from t_DL_Dataload d left join t_DL_MassTermination m on d.DataloadJobRefId=m.DataloadJobRefId
                            where m.DataloadJobRefId='{0}' 
                            and (m.ErrorStatus=1 or m.ErrorStatus=2)", jobID);
                

                using (var sqlConnection = new SqlConnection(context.Result.Database.Connection.ConnectionString))
                {
                    try
                    {
                        sqlConnection.Open();
                        var cmd = sqlConnection.CreateCommand();
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.CommandText = command;
                        var result = await cmd.ExecuteReaderAsync();
                        if (result.GetType() != typeof(DBNull))
                        {
                            while (result.Read() != false)
                            {
                                uploads.Add(new ErrorModel()
                                {
                                    ClientID = result.GetInt32(0),
                                    EmployeeNumber = result.GetString(1),
                                    ErrorStatus = result.GetInt32(2),
                                    ErrorDescription = result.GetString(3)
                                });
                            }

                        }
                        return uploads.AsEnumerable();
                    }
                    catch (Exception e)
                    {
                        Logger.FatalFormat("Error in retrieval for Dataload records, " +
                        "Exception Message: {0}, Inner Exception Message: {1}",
                        e.Message, e.InnerException == null ? "null" : e.InnerException.Message);
                        return null;
                    }
                }
            }
        }
    }
}