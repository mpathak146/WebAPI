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

namespace Fourth.DataLoads.Data.Entities
{
    class MassTerminateRepository : IRepository<MassTerminationModelSerialized>
    {
        /// <summary> The log4net Logger instance. </summary>
        private static readonly ILog Logger =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary> The factory responsible for creating data contexts. </summary>
        private readonly IDBContextFactory _contextfactory;
        private static readonly string connectionString = 
            ConfigurationManager.ConnectionStrings["DataloadsContext"].ConnectionString;
        private IEnumerable<ITableSchema> _tableSchemas;
        public Dictionary<string, ITableSchema> TableSchemas
        {
            get {
                return _tableSchemas.ToDictionary(x => x.COLUMN_NAME, x => x);
           }
        }

        /// <summary>
        /// Creation of MassterminateRepo
        /// </summary>
        /// <param name="factory"></param>
        public MassTerminateRepository(IDBContextFactory factory,
            IEnumerable<ITableSchema> tableSchema)
        {
            this._contextfactory = factory;
            if(tableSchema!=null)
                this._tableSchemas = tableSchema;
        }



        public async Task<long> SetDataAsync(UserContext userContext, 
            List<MassTerminationModelSerialized> input)
        {
            DataLoadBatch dataloadBatchID = null;

            if (input == null)
            {
                throw new ArgumentException("No legible data received through the body of upload, please check the data and upload right format.");
            }
            Logger.InfoFormat("MassTermination upload contains no null data, upload request process starting now...");

            if (_tableSchemas == null)
                _tableSchemas = await GetTableSchema();

            Logger.InfoFormat("MassTermination schema loaded into memory");
            using (var context = this._contextfactory.GetContextAsync())
            {
                if (input.Count > 0)
                {
                    try
                    {
                        dataloadBatchID = 
                            UpdateDataloadToContext(input, userContext, context);
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
                return dataloadBatchID.DataLoadBatchId;
            }
        }

        private DataLoadBatch UpdateDataloadToContext(List<MassTerminationModelSerialized> input, UserContext userContext , DataloadsContext context)
        {
            try
            {
                var batch = new DataLoadBatch
                {
                    DataloadTypeRefID = (long)(DataLoadTypes.MassTermination),
                    DateCreated = DateTime.Now,
                    DateProcessed = null,
                    Status = DataloadStatus.Requested.ToString(),
                    GroupID = int.Parse(userContext.OrganisationId),
                    UserName = userContext.UserId
                };
                context.DataLoadBatch.Add(batch);                
                
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
                         ErrRecord = m.ToXml<MassTerminationModelSerialized>(),
                         ErrDescription = m.ErrValidation
                     });
                return batch;

            }
            catch(Exception e)
            {
                Logger.Error(e.Message);
                throw e;
            }
        }

         private bool IsValid(MassTerminationModelSerialized mr)
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

        public async Task<IEnumerable<ITableSchema>> GetTableSchema()
        {
            IEnumerable<ITableSchema> ts =null;            
            try
            {
                var sql =
                    string.Format(@"SELECT TABLE_NAME, 
                                COLUMN_NAME, 
                                CHARACTER_MAXIMUM_LENGTH, 
                                DATA_TYPE 
                                FROM INFORMATION_SCHEMA.COLUMNS 
                                WHERE TABLE_NAME = '{0}'", "t_MassTermination");

                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    await sqlConnection.OpenAsync();
                    ts = await sqlConnection.QueryAsync<TableSchema>(sql);
                }
            }
            catch(Exception e)
            {
                Logger.FatalFormat("Error in getting Table Schema for Mass Terminate, " +
                    "Exception Message: {0}, Inner Exception Message: {1}", 
                    e.Message, e.InnerException.Message);
                throw e;                
            }
            return ts;
        }
    }
    
}
