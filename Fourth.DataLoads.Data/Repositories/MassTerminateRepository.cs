using Fourth.DataLoads.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fourth.DataLoads.Data.Models;
using log4net;

namespace Fourth.DataLoads.Data.Entities
{
    class MassTerminateRepository : IMassTerminateRepository
    {
        /// <summary> The log4net Logger instance. </summary>
        private static readonly ILog Logger = 
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary> The factory responsible for creating data contexts. </summary>
        private readonly IPortalDBContextFactory _contextfactory;

        /// <summary>
        /// Creation of MassterminateRepo
        /// </summary>
        /// <param name="factory"></param>
        public MassTerminateRepository(IPortalDBContextFactory factory)
        {
            this._contextfactory = factory;
        }

        public async Task<bool> SetDataAsync(int groupID, List<MassTerminate> input)
        {
            if (input == null)
            {
                throw new ArgumentException("Parameter \"input\" is required.");
            }
            using (var context = await this._contextfactory.GetContextAsync(groupID))
            {
                foreach (var record in input)
                {
                   
                }
            }
            return true;
        }
    }
}
