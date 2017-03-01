using Fourth.DataLoads.Data.Interfaces;
using Fourth.DataLoads.Data.SqlServer;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fourth.DataLoads.Data.Repositories
{
    class PortalVerificationRepository : IPortalVerificationRepository
    {
        public bool IsValidEmployee(string groupID,string employee)
        {
            var _contextFactory = new DBContextFactory();
            try
            {
                object result = null;
                using (var context =
                    _contextFactory.GetPortalDBContextAsync(int.Parse(groupID)))
                {
                    using (var sqlConnection =
                        new SqlConnection(context.Result.Database.Connection.ConnectionString))
                    {
                        sqlConnection.Open();
                        var cmd = sqlConnection.CreateCommand();
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.CommandText = string.Format(
                            "select [Status] from t_HRDetails where EmployeeNumber='{0}'",
                            employee);
                        result = cmd.ExecuteScalar();
                    }
                    return (result.ToString() == "1")
                        ? true : false;
                }
            }
            catch(Exception e)
            {
                log4net.LogManager.GetLogger("WebAPITestAutomation").Error(e.Message);
                return false;
            }            
        }

        public bool RollbackEmployee(string groupID, string commaSeparatedEmployees)
        {
            var _contextFactory = new DBContextFactory();
            try
            {
                object result = null;
                using (var context =
                    _contextFactory.GetPortalDBContextAsync(int.Parse(groupID)))
                {
                    var command = "sprc_DL_ReinstateTerminatedEmployees";
                    using (var sqlConnection =
                        new SqlConnection(context.Result.Database.Connection.ConnectionString))
                    {
                        sqlConnection.Open();
                        var cmd = sqlConnection.CreateCommand();
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.CommandText = command;
                        cmd.Parameters.Add(new SqlParameter("@EmployeesToReinstate", commaSeparatedEmployees));
                        result = cmd.ExecuteNonQuery();
                    }

                }
                return true;
            }
            catch (Exception e)
            {
                log4net.LogManager.GetLogger("WebAPITestAutomation").Error(e.Message);
                return false;
            }
        }
    }
}
