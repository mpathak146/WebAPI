using Fourth.DataLoads.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fourth.DataLoads.Data.Models;
using log4net;
using Fourth.DataLoads.Validation;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Configuration;
using Dapper;

using Fourth.DataLoads.Data.Repositories;
using Fourth.DataLoads.Data.Interface;

namespace Fourth.DataLoads.Data.Entities
{
    class MassTerminateRepository : IMassTerminateRepository
    {
        /// <summary> The log4net Logger instance. </summary>
        private static readonly ILog Logger =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary> The factory responsible for creating data contexts. </summary>
        private readonly IDBContextFactory _contextfactory;
        private static readonly string connectionString = 
            ConfigurationManager.ConnectionStrings["DataloadsContext"].ConnectionString;
        private static IEnumerable<TableSchema> tableSchemas;
        public static Dictionary<string, TableSchema> TableSchemas
        {
            get {
                return tableSchemas.ToDictionary(x => x.COLUMN_NAME, x => x);
           }
        }

        /// <summary>
        /// Creation of MassterminateRepo
        /// </summary>
        /// <param name="factory"></param>
        public MassTerminateRepository(IDBContextFactory factory)
        {
            this._contextfactory = factory;
        }



        public async Task<bool> SetDataAsync(UserContext userContext, List<MassTerminationModel> input)
        {
            if (input == null)
            {
                throw new ArgumentException("No legible data received through the body of upload, please check the data and upload right format.");
            }
            Logger.InfoFormat("MassTermination upload contains no null data, upload request process starting now...");
            await GetTableSchema();
            Logger.InfoFormat("MassTermination schema loaded into memory");
            using (var context = this._contextfactory.GetContextAsync())
            {
                if (input.Count > 0)
                {
                    try
                    {
                        UpdateDataloadContext(input, userContext, context);
                    }
                    catch (Exception e)
                    {
                        Logger.FatalFormat(string.Format("Internal database exception with error {0} and inner exception message {1}",
                            e.Message,e.InnerException.Message));
                        throw e;
                    }

                }
                else
                {
                    Logger.InfoFormat("No data is received through the body of upload, please check the data and upload.");
                    throw new ArgumentException
                        ("No data is received through the body of upload, please check the data and upload.");
                }
                try
                {
                    Logger.InfoFormat("Dataload accepted and saving to the staging DB in progress");
                    await context.SaveChangesAsync();
                    Logger.InfoFormat("Dataload accepted and saved to the staging DB");
                }
                catch (DbUpdateException dbEx)
                {
                    Logger.FatalFormat(string.Format("Internal database exception with error {0}",
                        dbEx.InnerException.Message));
                    throw new DbUpdateException(string.Format("Internal database exception with error {0}",
                        dbEx.InnerException.Message),dbEx);
                }
                return true;
            }
        }

        private void UpdateDataloadContext(List<MassTerminationModel> input, UserContext userContext , DataloadsContext context)
        {
            context.DataLoadBatch.Add(new DataLoadBatch
            {
                DataloadTypeRefID = (long)(DataLoadTypes.MassTermination),
                DateCreated = DateTime.Now,
                DateProcessed = null,
                Status = DataloadStatus.Requested.ToString(),
                GroupID = int.Parse(userContext.OrganisationId),
                UserName = userContext.UserId
            });
            context.MassTerminations.AddRange
                (from m in input
                 where (IsValid(m))
                 select new MassTermination
                 {
                     DataLoadBatchRefId = m.DataLoadBatchId,
                     EmployeeNumber = m.EmployeeNumber,
                     TerminationDate = DateTime.Parse(m.TerminationDate),
                     TerminationReason = m.TerminationReason
                 });
            context.DataLoadErrors.AddRange
                (from m in input
                 where (!IsValid(m))
                 select new DataLoadErrors
                 {
                     DataLoadBatchRefId = m.DataLoadBatchId,
                     ErrRecord = ((IModelMarker)m).ToXml(),
                     ErrDescription = m.ErrValidation
                 });
        }

         private bool IsValid(MassTerminationModel mr)
        {
            DateTime td;
            if (int.Parse(TableSchemas["EmployeeNumber"].CHARACTER_MAXIMUM_LENGTH)
                    < mr.EmployeeNumber.Length || string.IsNullOrWhiteSpace(mr.EmployeeNumber))
            {
                mr.ErrValidation = "Employee Number has invalid value";          
                return false;
            }
            if (!DateTime.TryParse(mr.TerminationDate.ToString(), out td)) 
            {
                mr.ErrValidation = "Termination Date has invalid value";
                return false;
            }
            if (int.Parse(TableSchemas["TerminationReason"].CHARACTER_MAXIMUM_LENGTH)
                    < mr.EmployeeNumber.Length)
            {
                mr.ErrValidation = "Termination Reason has invalid value";
                return false;
            }
            return true;
        }

        private static async Task<bool> GetTableSchema()
        {
            var sql = string.Format(@"select TABLE_NAME, COLUMN_NAME, CHARACTER_MAXIMUM_LENGTH, DATA_TYPE 
                                      from INFORMATION_SCHEMA.COLUMNS IC where TABLE_NAME = '{0}'", 
                                      "t_MassTermination");

            if (tableSchemas==null)
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    await sqlConnection.OpenAsync();
                    tableSchemas = await sqlConnection.QueryAsync<TableSchema>(sql);
                }
            }
            return true;
        }
    }
    
}
