using System;
using System.Threading.Tasks;
using Fourth.Orchestration.Messaging;
using Fourth.Orchestration.Messaging.Azure;
using Fourth.Orchestration.Storage;
using Fourth.Orchestration.Storage.Azure;
using Google.ProtocolBuffers;
using log4net;

namespace Fourth.DataLoads.Data.Repositories
{


    /// <summary>
    /// Sends formatted payloads to Azure Service Bus.
    /// </summary>
    /// <remarks>
    /// We need to keep a single instance of IMessagingFactory and IBus open between requests. This is so we
    /// do not continuously reconnect to Azure every time we send a message.
    /// </remarks>
    public class AzureSender : IDisposable
    {
        /// <summary> The class logger.</summary>
        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary> The active instance of this class. </summary>
        private static volatile AzureSender instance;

        /// <summary> Used in synchronizing instance creation. </summary>
        private static object syncRoot = new object();

        /// <summary> The instance of the messaging factory to use for creating service bus objects. </summary>
        private AzureMessagingFactory messageFactory;

        /// <summary> The instance of the message bus to use for sending messages. </summary>
        private IMessageBus bus;

        /// <summary>
        /// Prevents a default instance of the <see cref="AzureSender"/> class from being created.
        /// </summary>
        private AzureSender()
        {
            Logger.Info("Constructor called: Initializing a new instance of the AzureSender.");

            IMessageStore store = null;
            this.messageFactory = new AzureMessagingFactory(store);
            this.bus = messageFactory.CreateMessageBus();
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="AzureSender"/> class.
        /// </summary>
        ~AzureSender()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// Gets the active instance of the <see cref="AzureSender"/>.
        /// </summary>
        public static AzureSender Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new AzureSender();
                        }
                    }
                }

                return instance;
            }
        }

        /// <summary>
        /// Publishes a Protocol Buffer event object on the service bus.
        /// </summary>
        /// <param name="message">The message to send.</param>
        /// <returns>An indication of whether the message was sent successfully.</returns>
        public bool Publish(IMessage message)
        {
            return this.bus.Publish(message);
        }

        /// <summary>
        /// Publishes a Protocol Buffer event object on the service bus asynchronously.
        /// </summary>
        /// <param name="message">The message to send.</param>
        /// <returns>A task representing the result.</returns>
        public async Task<bool> PublishAsync(IMessage message)
        {
            return await this.bus.SendAsync(message);
        }

        /// <summary>
        /// Sends a Protocol Buffer command object on the service bus.
        /// </summary>
        /// <param name="message">The message to send.</param>
        /// <returns>An indication of whether the message was sent successfully.</returns>
        public bool Send(IMessage message)
        {
            return this.bus.Send(message);
        }

        /// <summary>
        /// Sends a Protocol Buffer command object on the service bus asynchronously.
        /// </summary>
        /// <param name="message">The message to send.</param>
        /// <returns>A task representing the result.</returns>
        public async Task<bool> SendAsync(IMessage message)
        {
            return await this.bus.SendAsync(message);
        }

        /// <summary>
        /// Closes all the connections used by the class.
        /// </summary>
        public void Close()
        {
            if (this.bus != null)
            {
                this.bus.Dispose();
            }

            if (this.messageFactory != null)
            {
                this.messageFactory.Dispose();
            }
        }

        /// <summary>
        /// Releases all resources used by the <see cref="AzureSender"/>.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases all resources used by the <see cref="AzureSender"/>.
        /// </summary>
        /// <param name="disposing">Indicates whether the call is for disposal or finalization.</param>
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.Close();
            }
        }
    }
}
