namespace Fourth.DataLoads.Listener.Handlers
{
    using System;
    using System.Threading.Tasks;
    using Orchestration.Messaging;
    using Orchestration.Model.People;
    using log4net;
    using System.Data.SqlClient;
    using System.Data;
    using Data.Models;
    using Data.Repositories;
    using Data.Interfaces;
    using Data.Entities;

    /// <summary>
    /// Handles commands of type 'DataloadRequest' that the service has received through 
    /// Orchestration API    
    /// </summary>
    public class MassTerminationHandler : IMessageHandler<Commands.CreateAccount>
    {
        /// <summary> The class Logger instance. </summary>
        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        IMassTerminationService<Commands.CreateAccount> MassTerminationService { get; }
        IDataFactory DataFactory { get; }

        IRepository<MassTerminationModelSerialized> _massTerminateRepository;
        /// <summary>
        /// Default constructor
        /// </summary>
        public MassTerminationHandler (IMassTerminationService<Commands.CreateAccount> massTerminationService
            , IDataFactory dataFactory)
        {
            MassTerminationService = massTerminationService;
            this.DataFactory = dataFactory;
        }

        /// <summary>
        /// Handles MassTerminationHandler asynchronously
        /// </summary>
        /// <param name="payload">The incoming command payload</param>
        /// <returns>Message result</returns>
        public async Task<MessageHandlerResult> HandleAsync(Commands.CreateAccount payload, string trackingId)
        {
            // Formatted payload arguments used in logging.
            string payloadArgs = string.Empty;

            try
            {

                // Log the incoming payload
                Logger.DebugFormat("{0} received: TrackingId: \"{1}\"; {2}", typeof(Commands.CreateAccount).Name, trackingId, payload.ToString().Replace("\n", "; "));

                var result = await this.MassTerminationService.ProcessPayload(payload, this.DataFactory);

                // Report success 
                Logger.InfoFormat("Successfully imported record to Id \"{0}\" for Organisation Id \"{1}\"", result, payload.CustomerId);
                return MessageHandlerResult.Success;

            }


            catch (DataException ex)
            {
                // Entity Framework database connectivity error .... retry the message
                Logger.Error(string.Format("Commands.MassTermination handler failed due to a Data exception. A status of Retry will be returned and the message will not be processed. Payload: {0}", payloadArgs), ex);
                return MessageHandlerResult.Retry;
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
