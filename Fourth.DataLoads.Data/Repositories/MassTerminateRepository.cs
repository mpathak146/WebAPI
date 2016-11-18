using Fourth.DataLoads.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fourth.DataLoads.Data.Models;
using log4net;
using Fourth.DataLoads.Validation;

namespace Fourth.DataLoads.Data.Entities
{
    class MassTerminateRepository : IMassTerminateRepository
    {
        /// <summary> The log4net Logger instance. </summary>
        private static readonly ILog Logger = 
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary> The factory responsible for creating data contexts. </summary>
        private readonly IDBContextFactory _contextfactory;

        /// <summary>
        /// Creation of MassterminateRepo
        /// </summary>
        /// <param name="factory"></param>
        public MassTerminateRepository(IDBContextFactory factory)
        {
            this._contextfactory = factory;
        }



        public async Task<bool> SetDataAsync(int groupID, List<MassTermination> input)
        {
            if (input == null)
            {
                throw new ArgumentException("Parameter \"input\" is required.");
            }
            using (var context = this._contextfactory.GetContextAsync())
            {
                if (input.Count > 0)
                {
                    var dataloadType =
                        context.DataLoadType.Where
                        (d => d.DataloadType == DataLoadTypes.MassTermination.ToString())
                        .Take(1).ToList();

                    context.DataLoadBatch.Add(new DataLoadBatch { DataloadTypeID = dataloadType[0].DataloadTypeID, DateCreated = DateTime.Now, DateProcessed = DateTime.Now, Status = "Requested" });
                                                                              
                }
                foreach (var record in input)
                    context.MassTerminations.Add(record);
                await context.SaveChangesAsync();
            }
            return true;
        }

        
    }
}
