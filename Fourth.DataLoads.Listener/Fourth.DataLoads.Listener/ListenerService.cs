using Fourth.DataLoads.Data;
using Fourth.Orchestration.Messaging;
using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Fourth.DataLoads.Listener
{
    public partial class ListenerService : ServiceBase, IListenerService
    {
        /// <summary> The log4net Logger instance. </summary>
        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary> The messaging factory to use when creating bus and listener instances. </summary>
        private readonly IMessagingFactory _messageFactory;

        /// <summary> The data factory to use when creating repositories. </summary>
        private readonly IDataFactory _dataFactory;

        /// <summary> The listener instance. </summary>
        private IMessageListener _messageListener;

        /// <summary> The message bus instance to use when sending messages. </summary>
        private IMessageBus _bus;

        public ListenerService(IMessagingFactory messageFactory, IDataFactory dataFactory)
        {
            InitializeComponent();
            _messageFactory = messageFactory;
            _dataFactory = dataFactory;
        }

        public void StartService(string[] args)
        {
            this.OnStart(args);
        }

        public void StopService()
        {
            this.OnStop();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                Logger.Info("Starting MenuCycles message listener ...");
                // Create the listener and bus instances used to send and receive messages.
                this._messageListener = this._messageFactory.CreateMessageListener(Constants.EndpointName);
                this._bus = this._messageFactory.CreateMessageBus();

                // Wire up the handlers here
                //TO DO
                //this._messageListener.RegisterHandler(new MyHandler(this._dataFactory));

                // Start the listener
                this._messageListener.StartListener();
            }
            catch (Exception ex)
            {
                Logger.Fatal("Fourth Dataloads Service Message Listener failed to start due to an exception in ListenerService.OnStart.", ex);
            }
        }

        protected override void OnStop()
        {
            Logger.Info("Stopping DataLoads message listener ...");

            if (this._messageListener != null)
            {
                this._messageListener.StopListener();
            }
        }
    }
}
