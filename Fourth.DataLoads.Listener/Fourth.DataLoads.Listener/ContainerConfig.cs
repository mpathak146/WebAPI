using Autofac;
using Fourth.DataLoads.Data;
using Fourth.Orchestration.Messaging;
using Fourth.Orchestration.Messaging.Azure;
using Fourth.Orchestration.Storage;
using Fourth.Orchestration.Storage.Azure;
using log4net;
using Fourth.DataLoads.Data.SqlServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Fourth.DataLoads.Data.Interfaces;
using Fourth.DataLoads.Data.Entities;
using Fourth.Orchestration.Model.People;
using Fourth.DataLoads.Listener.Handlers;
using Fourth.DataLoads.Listener.Services;
using System.Web.Http;
namespace Fourth.DataLoads.Listener
{
    class ContainerConfig
    {
        /// <summary> The log4net Logger instance. </summary>
        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Sets up the IoC container with the correct dependencies.
        /// </summary>
        /// <param name="config">The configuration section that describes the dependencies to set up. </param>
        /// <returns>A container with the correct dependencies configured.</returns>
        public static IContainer Configure()
        {
            Logger.DebugFormat("Resolving dependencies.");

            var builder = new ContainerBuilder();

            // Set the messaging implementation
            builder.RegisterType<AzureMessageStore>().As<IMessageStore>().InstancePerLifetimeScope();
            builder.RegisterType<AzureMessagingFactory>().As<IMessagingFactory>().InstancePerLifetimeScope();

            // Set the data factory - feed in the connection string for the login database
            // Add any other dependencies here, i.e. the repositories and context factory
            builder
                .RegisterType<SqlDataFactory>()
                .As<IDataFactory>()
                .InstancePerLifetimeScope();
                //.WithParameter("connectionString",
                //ConfigurationManager.ConnectionStrings["DataloadsContext"].ConnectionString);

            //builder.RegisterType<MappingFactory>().As<IMappingFactory>().InstancePerLifetimeScope();

            builder.RegisterType<MassTerminationService>().As<IMassTerminationService<Commands.CreateAccount>>().InstancePerLifetimeScope();

            // Register the service class that sits at the top of the dependency chain
            builder.RegisterType<ListenerService>().As<IListenerService>().InstancePerLifetimeScope();

            return builder.Build();
        }

    }
}
