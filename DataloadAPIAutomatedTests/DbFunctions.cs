using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;

namespace DataloadAPIAutomatedTests
{
    public static class DbFunctions
    {
        public static SqlConnection GetDbConn()
        {
            string ConString = ConfigurationManager.AppSettings["ConnString"];
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConString;
            con.Open();
            return con;
        }

        public static bool GetEmployeeStatus(string EmployeeNumber)
        {
            string query = "select Status from t_hrdetails where employeenumber = '"+EmployeeNumber+"'";
            var status = ExecuteScalar(query);
            return Convert.ToBoolean(status);
        }

        public static object ExecuteScalar(string query)
        {
            SqlCommand cmd = new SqlCommand();
            object record;
            try
            {
                cmd.Connection = GetDbConn();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = query;
                record = cmd.ExecuteScalar();
                return record;
            }
            catch (Exception ex)
            {

            }
            finally
            {

                cmd.Connection.Close();

            }

            return null;
        }
    }
}
