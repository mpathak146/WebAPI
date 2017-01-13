using Fourth.DataLoads.Data.Interfaces;
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
        public void RecordStagingErrors(Commands.CreateAccount payload)
        {
            int groupID = int.Parse(payload.EmailAddress);
            Logger.InfoFormat("Validating a given employee number");

            using (var context = _contextFactory.GetPortalDBContextAsync(groupID))
            {
                using (var stagingContext = _contextFactory.GetStagingDBContext())
                {
                    var errors = 
                                 
                                 (from err in stagingContext.DataLoadErrors
                                 where (err.DataLoadBatchRefId.ToString() == payload.FirstName)
                                 select err)
                                 .AsEnumerable()
                                 .Select(x=>new 
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


                    {
                        using (var sqlConnection = new SqlConnection(context.Result.Database.Connection.ConnectionString))
                        {
                            using (SqlBulkCopy bulkCopy =
                               new SqlBulkCopy(sqlConnection))
                            {
                                bulkCopy.DestinationTableName = "dbo.t_MassTermination";
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

        public bool RecordDataloadBatch(MassTerminationModelSerialized employee, Commands.CreateAccount payload)
        {
            throw new NotImplementedException();
        }
    }
}