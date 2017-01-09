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
                Logger.DebugFormat(
                   @"Call to CreateAccount: internalId=""{0}"", 
                                             emailAddress=""{1}"", 
                                             firstName=""{2}"", 
                                             lastName=""{3}"", 
                                             organisationId=""{4}""",
                   "",
                   "",
                   "",
                   "",
                   "");

                var builder = new Commands.CreateAccount.Builder();

                foreach (var batch in batches)
                {
                    builder.SetInternalId("")
                    .SetSource(Commands.SourceSystem.PS_LIVE)
                    .SetEmailAddress(batch.OrganizationID)
                    .SetFirstName(batch.BatchID.ToString())
                    .SetLastName(batch.JobID.ToString())
                    .SetCustomerId(batch.User);
                    var message = builder.Build();

                    await AzureSender.Instance.SendAsync(message);
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
