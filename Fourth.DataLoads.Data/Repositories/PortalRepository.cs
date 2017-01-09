using Fourth.DataLoads.Data.Interfaces;
using log4net;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System;
using Fourth.DataLoads.Data.Entities;
using Fourth.Orchestration.Model.People;

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
                    var command = "sprc_Dataload_ProcessMassTermination";

                    using (var sqlConnection = new SqlConnection(context.Result.Database.Connection.ConnectionString))
                    {
                        sqlConnection.Open();
                        var cmd = sqlConnection.CreateCommand();
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.CommandText = command;
                        cmd.Parameters.Add(new SqlParameter("@BatchID", payload.FirstName));
                        cmd.Parameters.Add(new SqlParameter("@JobID", payload.LastName));
                        cmd.Parameters.Add(new SqlParameter("@EmployeeNumber", employee.EmployeeNumber));
                        cmd.Parameters.Add(new SqlParameter("@TerminationReason", employee.TerminationReason));
                        cmd.Parameters.Add(new SqlParameter("@TerminationDate", employee.TerminationDate));
                        cmd.Parameters.Add(new SqlParameter("@UploadedBy", employee.DataLoadBatchId));

                        var result = cmd.ExecuteScalar();
                        if (bool.Parse(result.ToString()) != true)
                        {
                            Logger.ErrorFormat("Error in processing Mass Terminate for EmployeeNumber: {0} for GroupID: {1}",
                                new object[] { employee, groupID });
                            return false;
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
    }
}