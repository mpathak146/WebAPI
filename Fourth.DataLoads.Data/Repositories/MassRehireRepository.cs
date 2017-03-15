using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fourth.DataLoads.Data.Interfaces;
using Fourth.DataLoads.Data.Models;
using Fourth.DataLoads.Data.Repositories;
using Fourth.DataLoads.Data.Entities;
using System.Collections;
using System.Data.SqlClient;
using log4net;
using Dapper;
using System.Linq;
using EntityFramework.BulkInsert.Extensions;
using System.Data.Entity.Infrastructure;
using Fourth.Orchestration.Model.People;

namespace Fourth.DataLoads.Data.SqlServer
{
    internal class MassRehireRepository : IStagingRepository<MassRehireModelSerialized>
    {
        private IDBContextFactory _contextFactory;
        private static readonly string connectionString = AppSettings.DataloadContext;
        private static readonly ILog Logger =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IEnumerable<ITableSchema> _tableSchemas;
        public Dictionary<string, ITableSchema> TableSchemas
        {
            get
            {
                return _tableSchemas.ToDictionary(x => x.COLUMN_NAME, x => x);
            }
        }
        public MassRehireRepository(IDBContextFactory _contextFactory)
        {
            this._contextFactory = _contextFactory;
        }

        public async Task<IEnumerable<ITableSchema>> GetTableSchema()
        {
            IEnumerable<ITableSchema> ts = null;
            try
            {
                var sql =
                    string.Format(@"SELECT TABLE_NAME, 
                                COLUMN_NAME, 
                                CHARACTER_MAXIMUM_LENGTH, 
                                DATA_TYPE 
                                FROM INFORMATION_SCHEMA.COLUMNS 
                                WHERE TABLE_NAME = '{0}'", "t_MassRehire");

                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    await sqlConnection.OpenAsync();
                    ts = await sqlConnection.QueryAsync<TableSchema>(sql);
                }
            }
            catch (Exception e)
            {
                Logger.FatalFormat("Error in getting Table Schema for Mass Rehire, " +
                    "Exception Message: {0}, Inner Exception Message: {1}",
                    e.Message, e.InnerException == null ? "null" : e.InnerException.Message);
                throw e;
            }
            return ts;
        }

        public List<MassRehireModelSerialized> GetValidBatch(Guid batchID)
        {
            using (var contextStaging = this._contextFactory.GetStagingDBContext())
            {
                IEnumerable<MassRehireModelSerialized>
                    result = from mt in contextStaging.MassRehires
                             where mt.DataLoadBatchRefId == batchID
                             select new MassRehireModelSerialized
                             {
                                 DataLoadJobId = mt.DataLoadJobRefId,
                                 DataLoadBatchId = mt.DataLoadBatchRefId,
                                 EmployeeNumber = mt.EmployeeNumber,
                                 RehireDate = mt.RehireDate.ToString(),
                                 ErrValidation = "NA"
                             };
                return result.ToList<MassRehireModelSerialized>();
            }
        }

        public bool IsValid(MassRehireModelSerialized mr)
        {
            DateTime td;
            if (int.Parse(TableSchemas["EmployeeNumber"].CHARACTER_MAXIMUM_LENGTH)
                    < mr.EmployeeNumber.Length || string.IsNullOrWhiteSpace(mr.EmployeeNumber))
            {
                mr.ErrValidation = "Employee Number has invalid value";
                return false;
            }
            if (!DateTime.TryParse(mr.RehireDate.ToString(), out td))
            {
                mr.ErrValidation = "Rehire Date has invalid value";
                return false;
            }
            return true;
        }

        public async Task<IEnumerable<DataloadBatch>> SetDataAsync(UserContext userContext, List<MassRehireModelSerialized> input)
        {
            var jobGuid = Guid.NewGuid();
            var batches = new List<DataloadBatch>();
            if (input == null)
            {
                throw new ArgumentException("No legible data received through the body of upload, please check the data and upload right format.");
            }
            Logger.InfoFormat("Mass Rehire upload contains no null data, upload request process starting now...");

            if (_tableSchemas == null)
                _tableSchemas = await GetTableSchema();

            Logger.InfoFormat("Mass Rehire schema loaded into memory");
            try
            {
                using (var context = this._contextFactory.GetStagingDBContext())
                {
                    using (var dbContextTransaction = context.Database.BeginTransaction())
                    {
                        if (input.Count > 0)
                        {
                            try
                            {
                                var totbatches = input.Batch(AppSettings.BatchSize);
                                foreach (var batch in totbatches)
                                {
                                    var id = UpdateDataloadToContext(batch, userContext, context, jobGuid);
                                    batches.Add(new DataloadBatch
                                    {
                                        JobID = jobGuid,
                                        BatchID = id,
                                        OrganizationID = userContext.OrganisationId,
                                        User = userContext.UserId,
                                        Dataload = Commands.DataLoadTypes.MASS_REHIRE
                                    });
                                }
                                Logger.InfoFormat("MassRehire schema saved to entities, begining transaction commit");

                                dbContextTransaction.Commit();

                                Logger.InfoFormat("MassRehire schema saved to entities, transaction committed to the database");

                            }
                            catch (Exception e)
                            {
                                Logger.FatalFormat(string.Format("Internal database exception with error {0} and inner exception message {1}",
                                    e.Message, e.InnerException == null ? "null" : e.InnerException.Message));

                                dbContextTransaction.Rollback();

                                Logger.FatalFormat(string.Format("Transaction Rolledback on Internal database exception with error {0} and inner exception message {1}",
                                e.Message, e.InnerException == null ? "null" : e.InnerException.Message));
                                throw e;
                            }
                        }
                        else
                        {
                            Logger.InfoFormat("No data is received through the body of upload, please check the data and upload.");
                            throw new ArgumentException
                                ("No data is received through the body of upload, please check the data and upload.");
                        }
                        return batches;
                    }
                }
            }
            catch (Exception e)
            {
                Logger.ErrorFormat("Error inside MassTerminate Repository, function SetDataAsync(UserContext, input) with message: {0}", e.Message);
                throw e;
            }

        }
        private Guid UpdateDataloadToContext(IEnumerable<MassRehireModelSerialized> input,
            UserContext userContext,
            StagingDBContext context,
            Guid jobGuid)
        {
            try
            {
                var batchGuid = Guid.NewGuid();
                var dataload = new DataLoad
                {
                    DataLoadJobId = jobGuid,
                    DataLoadBatchId = batchGuid,
                    DataloadTypeRefID = (long)(DataLoadTypes.MASS_REHIRE),
                    DateCreated = DateTime.Now,
                    DateProcessed = null,
                    Status = DataloadStatus.STAGING_DB_UPDATED.ToString(),
                    GroupID = int.Parse(userContext.OrganisationId),
                    UserName = userContext.UserId
                };
                context.DataLoad.Add(dataload);

                context.BulkInsert<MassRehire>
                    (from m in input
                     where (IsValid(m))
                     select new MassRehire
                     {
                         DataLoadJobRefId = jobGuid,
                         DataLoadBatchRefId = batchGuid,
                         EmployeeNumber = m.EmployeeNumber,
                         RehireDate = DateTime.Parse(m.RehireDate),
                     });
                context.BulkInsert<DataLoadErrors>
                    (from m in input
                     where (!IsValid(m))
                     select new DataLoadErrors
                     {
                         DataLoadJobRefId = jobGuid,
                         DataLoadBatchRefId = batchGuid,
                         ErrRecord = m.ToXml<MassRehireModelSerialized>(),
                         ErrDescription = m.ErrValidation
                     });


                try
                {
                    Logger.InfoFormat("Dataload batch accepted and saving to the staging DB in progress");

                    context.SaveChanges();

                    Logger.InfoFormat("Dataload batch accepted and saved to the staging DB");

                    return batchGuid;
                }
                catch (DbUpdateException dbEx)
                {
                    Logger.FatalFormat(string.Format("Internal database exception with error {0}",
                        dbEx.InnerException == null ? "null" : dbEx.InnerException.Message));
                    throw new DbUpdateException(string.Format("Internal database exception with error {0}",
                        dbEx.InnerException == null ? "null" : dbEx.InnerException.Message), dbEx);
                }
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
                throw e;
            }
        }
    }
}