using System;
using System.Threading.Tasks;
using log4net;
using System.Data.SqlClient;
using Fourth.Orchestration.Messaging;
using Fourth.Orchestration.Model.People;
using Fourth.DataLoads.Data.Interfaces;

namespace Fourth.DataLoads.Listener.Handlers
{


    /// <summary>
    /// Handles commands of type 'DataloadRequest' that the service has received through 
    /// Orchestration API    
    /// </summary>
    public class MessageRelayHandler : IMessageHandler<Commands.DataloadRequest>
    {
        /// <summary> The class Logger instance. </summary>
        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private IMassTerminationService<Commands.DataloadRequest> MassTerminationService;
        internal IDataFactory DataFactory;
        private IMassRehireService<Commands.DataloadRequest> MassRehireService;

        /// <summary>
        /// Default constructor
        /// </summary>
        public MessageRelayHandler (IMassTerminationService<Commands.DataloadRequest> massTerminationService,
            IMassRehireService<Commands.DataloadRequest> massRehireService
            ,IDataFactory dataFactory)
        {
            MassTerminationService = massTerminationService;
            MassRehireService = massRehireService;
            this.DataFactory = dataFactory;
        }

        /// <summary>
        /// Handles MassTerminationHandler asynchronously
        /// </summary>
        /// <param name="payload">The incoming command payload</param>
        /// <returns>Message result</returns>
        public async Task<MessageHandlerResult> HandleAsync(Commands.DataloadRequest payload, string trackingId)
        {
            // Formatted payload arguments used in logging.
            string payloadArgs = string.Empty;

            try
            {

                // Log the incoming payload
                Logger.DebugFormat("{0} received: TrackingId: \"{1}\"; {2}", typeof(Commands.DataloadRequest).Name, trackingId, payload.ToString().Replace("\n", "; "));
                bool result=false;
                switch (payload.Dataload)
                {
                    case Commands.DataLoadTypes.MASS_TERMINATION:
                        result = await this.MassTerminationService.ProcessPayload(payload, this.DataFactory);
                        break;
                    case Commands.DataLoadTypes.MASS_REHIRE:
                        result = await this.MassRehireService.ProcessPayload(payload, this.DataFactory);
                        break;
                }

                // Report success 
                Logger.InfoFormat("Successfully imported record to Id \"{0}\" for Organisation Id \"{1}\"", result, payload.OrganisationId);
                return MessageHandlerResult.Success;

            }
            catch (SqlException ex)
            {
                // Is this a transient SQL error?
                if (ex.Number == (int)SqlErrorCodes.Deadlock || ex.Number == (int)SqlErrorCodes.GeneralNetworkError || ex.Number == (int)SqlErrorCodes.Timeout)
                {
                    Logger.WarnFormat(
                        "Transient SQL exception - the message will be retried. Sql error # {ex.Number}. TrackingId: \"{payloadArgs}\"", ex);
                    return MessageHandlerResult.Retry;
                }
                else
                {
                    Logger.ErrorFormat(
                        "Fatal SQL exception - the message will not be processed. Sql error # {ex.Number}. TrackingId: \"{payloadArgs}\"", ex);
                    return MessageHandlerResult.Fatal;
                }
            }
            catch (Exception ex)
            {
                // Something went wrong so we need to fail the message
                Logger.Error(string.Format("Commands.MassTermination handler failed due to an exception. A status of Fatal will be returned and the message will not be processed. Payload: {0}", payloadArgs), ex);
                return MessageHandlerResult.Fatal;
            }
        }
    }
}
