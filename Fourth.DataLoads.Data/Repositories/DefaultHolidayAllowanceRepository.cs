namespace Fourth.DataLoads.Data.SqlServer
{
    using Data;
    using Data.Entities;
    using Fourth.DataLoads.Data.Repositories;
    using log4net;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Fourth.DataLoads.Data.Interfaces;
    /// <summary>
    /// A repository that returns supplier data from SQL Server
    /// </summary>
    public class DefaultHolidayAllowanceRepository : IDefaultHolidayAllowanceRepository
    {
        /// <summary> The log4net Logger instance. </summary>
        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary> The factory responsible for creating data contexts. </summary>
        private readonly IDBContextFactory _contextfactory;

        /// <summary> The connection string to use for this instance. </summary>
        //private string _connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlSupplierExampleRepository"/> class.
        /// </summary>
        /// <param name="contextfactory">The factory responsible for creating data contexts.</param>
        public DefaultHolidayAllowanceRepository(IDBContextFactory factory)
        {
            this._contextfactory = factory;
        }

        public async Task<IEnumerable<DefaultHolidayAllowance>> GetDataAsync(int groupID)
        {
            using (var context = await this._contextfactory.GetContextAsync(groupID))
            {
                context.Configuration.AutoDetectChangesEnabled = false;

                var DefaultHolAllowances = await (from dhol in context.DefaultHolidayAllowances
                                                  join jt in context.JobTitles on dhol.JobTitleID equals jt.JobTitleID
                                                  select new DefaultHolidayAllowance
                                                  {
                                                      JobTitleID = dhol.JobTitleID,
                                                      YearsWorked = dhol.YearsWorked,
                                                      Allowance = dhol.Allowance,
                                                      LastModified = dhol.LastModified,
                                                      DateCreated = dhol.DateCreated,
                                                      JobTitleName = jt.Name
                                                  }).ToListAsync();

                return DefaultHolAllowances;
            }
        }

        public async Task<IEnumerable<DefaultHolidayAllowance>> GetDataAsync(int groupID, int jobtitleId)
        {
            using (var context = await this._contextfactory.GetContextAsync(groupID))
            {
                context.Configuration.AutoDetectChangesEnabled = false;

                var DefaultHolAllowances = await (from dhol in context.DefaultHolidayAllowances
                                                  join jt in context.JobTitles on dhol.JobTitleID equals jt.JobTitleID
                                                  where dhol.JobTitleID == jobtitleId
                                                  select new DefaultHolidayAllowance
                                                  {
                                                      JobTitleID = dhol.JobTitleID,
                                                      YearsWorked = dhol.YearsWorked,
                                                      Allowance = dhol.Allowance,
                                                      LastModified = dhol.LastModified,
                                                      DateCreated = dhol.DateCreated,
                                                      JobTitleName = jt.Name
                                                  }).ToListAsync();

                return DefaultHolAllowances;
            }
        }

        public async Task<IEnumerable<DefaultHolidayAllowance>> GetDataAsync(int groupID, int jobtitleId, int yearsworked)
        {
            
            using (var context = await this._contextfactory.GetContextAsync(groupID))
            {
                context.Configuration.AutoDetectChangesEnabled = false;

                   var DefaultHolAllowances = await (from dhol in context.DefaultHolidayAllowances
                                                      join jt in context.JobTitles on dhol.JobTitleID equals jt.JobTitleID
                                                      where dhol.JobTitleID == jobtitleId
                                                      && dhol.YearsWorked == yearsworked
                                                      select new DefaultHolidayAllowance
                                                      {
                                                          JobTitleID = dhol.JobTitleID,
                                                          YearsWorked = dhol.YearsWorked,
                                                          Allowance = dhol.Allowance,
                                                          LastModified = dhol.LastModified,
                                                          DateCreated = dhol.DateCreated,
                                                          JobTitleName = jt.Name
                                                      }).ToArrayAsync();
                    return DefaultHolAllowances;
            }
        }

        public async Task<IEnumerable<DefaultHolidayAllowance>> GetDataAsync(int groupID, int jobtitleId, int yearsworked, int allowance)
        {
            using (var context = await this._contextfactory.GetContextAsync(groupID))
            {
                context.Configuration.AutoDetectChangesEnabled = false;

                var DefaultHolAllowances = await (from dhol in context.DefaultHolidayAllowances
                                                  join jt in context.JobTitles on dhol.JobTitleID equals jt.JobTitleID
                                                  where dhol.JobTitleID == jobtitleId
                                                  && dhol.YearsWorked == yearsworked
                                                  && dhol.Allowance == allowance
                                                  select new DefaultHolidayAllowance
                                                  {
                                                      JobTitleID = dhol.JobTitleID,
                                                      YearsWorked = dhol.YearsWorked,
                                                      Allowance = dhol.Allowance,
                                                      LastModified = dhol.LastModified,
                                                      DateCreated = dhol.DateCreated,
                                                      JobTitleName = jt.Name
                                                  }).ToArrayAsync();

                return DefaultHolAllowances;
            }
        }

        public async Task<int> GetDefaultHolidayAllowanceJobTitleIDAsync(int groupID, string jobTitleName)
        {
            using (var context = await this._contextfactory.GetContextAsync(groupID))
            {
                var jt = await (from job in context.JobTitles
                                where job.Name == jobTitleName
                                select job.JobTitleID).FirstOrDefaultAsync();

                return jt;
            }
        }

        ///  <inheritdoc />
        public async Task<bool> SetDataAsync(int groupID, List<DefaultHolidayAllowance> input)
        {
            int JobtitleID = 0;

            if (input == null)
            {
                throw new ArgumentException("Parameter \"input\" is required.");
            }

            using (var context = await this._contextfactory.GetContextAsync(groupID))
            {
                foreach (var record in input)
                {
                    //Get job title Id if not supplied
                    if (record.JobTitleID == 0)
                    {
                        JobtitleID = await GetDefaultHolidayAllowanceJobTitleIDAsync(groupID, record.JobTitleName);
                    }
                    else
                    {
                        JobtitleID = record.JobTitleID;
                    }

                    //Do the search
                    var searchResult = await GetDataAsync(groupID, JobtitleID, record.YearsWorked);

                    //add if not found..
                    if (!searchResult.Any())
                    {
                        var newRecord = new DefaultHolidayAllowanceTable
                        {
                            JobTitleID = JobtitleID,
                            YearsWorked = record.YearsWorked,
                            Allowance = record.Allowance,
                            DateCreated = DateTime.UtcNow,
                            LastModified = DateTime.UtcNow
                        };
                        context.DefaultHolidayAllowances.Add(newRecord);
                    }
                    //update if found
                    else
                    {
                        var updatedRecord = new DefaultHolidayAllowanceTable
                        {
                            JobTitleID = searchResult.FirstOrDefault().JobTitleID,
                            YearsWorked = searchResult.FirstOrDefault().YearsWorked,
                            Allowance = searchResult.FirstOrDefault().Allowance,
                            LastModified = searchResult.FirstOrDefault().LastModified,
                            DateCreated = searchResult.FirstOrDefault().DateCreated
                        };

                        context.DefaultHolidayAllowances.Attach(updatedRecord);

                        updatedRecord.Allowance = record.Allowance;
                        updatedRecord.LastModified = DateTime.UtcNow;
                    }
                }

                // Save the changes asynchronously
                var result = await context.SaveChangesAsync();

                // Return an indication of whether any records have been updated
                //return (result > 0);
                return true;
            }
        }

        ///  <inheritdoc />
        public async Task<bool> DeleteDataAsync(int groupID, int jobtitleID)
        {
            using (var context = await this._contextfactory.GetContextAsync(groupID))
            {
                //Do the search
                var searchResult = await GetDataAsync(groupID, jobtitleID);

                foreach (DefaultHolidayAllowance dhol in searchResult)
                {
                    var delRecord = new DefaultHolidayAllowanceTable
                    {
                        JobTitleID = jobtitleID,
                        YearsWorked = dhol.YearsWorked,
                        Allowance = dhol.Allowance,
                        DateCreated = DateTime.UtcNow,
                        LastModified = DateTime.UtcNow
                    };

                    context.DefaultHolidayAllowances.Attach(delRecord);

                    context.DefaultHolidayAllowances.Remove(delRecord);
                }

                // Save the changes (deletions) asynchronously
                var result = await context.SaveChangesAsync();

                // Return an indication of whether any records have been updated
                return (result > 0);
            }
        }

        public Task<IEnumerable<ITableSchema>> GetTableSchema()
        {
            throw new NotImplementedException();
        }

        public Task<bool> SetDataAsync(UserContext userContext, List<DefaultHolidayAllowance> input)
        {
            throw new NotImplementedException();
        }

        Task<long> IRepository<DefaultHolidayAllowance>.SetDataAsync(UserContext userContext, List<DefaultHolidayAllowance> input)
        {
            throw new NotImplementedException();
        }
    }
}