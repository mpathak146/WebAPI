using Fourth.DataLoads.Data.Interfaces;
using Fourth.DataLoads.Data.Models;
using Fourth.Orchestration.Model.People;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fourth.DataLoads.Data.Repositories
{
    class QueueRepository : IQueueRepository
    {
        /// <summary> The log4net Logger instance. </summary>
        private static readonly ILog Logger =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public async Task<bool> PushDataAsync(IEnumerable<DataloadBatch> batches)
        {

            try
            {
                var builder = new Commands.DataloadRequest.Builder();

                foreach (var batch in batches)
                {
                    builder
                    .SetSource(Commands.SourceSystem.PS_LIVE)
                    .SetOrganisationId(batch.OrganizationID)
                    .SetBatchID(batch.BatchID.ToString())
                    .SetJobID(batch.JobID.ToString())
                    .SetRequestedBy(batch.User)
                    .SetDataload(Commands.DataLoadTypes.MASS_TERMINATION);
                    var message = builder.Build();

                    await AzureSender.Instance.SendAsync(message);
                    Logger.DebugFormat(
                       @"Call to DataloadRequest: OrganizationID=""{0}"", 
                                                                 JobID=""{1}"", 
                                                                 BatchID=""{2}"", 
                                                                 RequestedBy=""{3}""",

                       batch.OrganizationID,
                       batch.JobID,
                       batch.BatchID,
                       batch.User);
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error("Error in PushDataAsync method.", ex);
                return false;
            }
        }
    }
}
