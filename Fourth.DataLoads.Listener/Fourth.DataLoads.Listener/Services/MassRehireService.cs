using Fourth.DataLoads.Data.Entities;
using Fourth.DataLoads.Data.Interfaces;
using Fourth.DataLoads.Listener.Handlers;
using Fourth.Orchestration.Model.People;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fourth.DataLoads.Listener.Services
{
    class MassRehireService : IMassRehireService<Commands.DataloadRequest>
    {
        private readonly ILog Logger =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public async Task<bool> ProcessPayload(Commands.DataloadRequest payload, 
            IDataFactory dataFactory)
        {
            if (payload.Dataload != Commands.DataLoadTypes.MASS_REHIRE) return false;
            Logger.Info("Entering ProcessPayload in MassRehireService");

            List<MassRehireModelSerialized> result=null;
            string batchID = payload.BatchID;
            try
            {
                if (dataFactory.GetPortalRepository().CopyDataloadBatchToPortal(payload))
                {
                    Logger.DebugFormat("payload batch recorded to portal, BatchID: {0}", batchID);
                    result = dataFactory.GetMassRehireRepository().GetValidBatch(Guid.Parse(batchID));
                    Logger.DebugFormat("Valid batch retrieved, BatchID: {0}", batchID);
                    foreach (var emp in result)
                    {
                        dataFactory.GetPortalRepository().ProcessMassRehire(emp, payload);
                        Logger.DebugFormat("Processed mass Rehire for Employee Number: {0}", batchID);
                    }
                    dataFactory.GetPortalRepository().CopyTerminationStagingErrorsToPortal(payload);
                    Logger.DebugFormat("All staging errors are copied to the portal db for Batch {0}", batchID);

                }
                return true;
            }
            catch (Exception e)
            {
                Logger.FatalFormat("Issue with the batch insert for BatchID: {0}, with Exception Message {1}", 
                    payload.BatchID,e.Message);
                return false;
            }

        }
    }
}
