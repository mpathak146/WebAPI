using Autofac;
using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Fourth.DataLoads.Listener
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application
        /// </summary>
        /// 
        private static ILifetimeScope _scope;

        private static ILog Logger;
        /// 
        static void Main(string[] args)
        {
            // Start log4net up
            XmlConfigurator.Configure();

            // Set the ApplicationId for logging
            log4net.GlobalContext.Properties["ApplicationId"] = "DataLoads.Listener";

            // Get the logger
            Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            try
            {
                // Set up the IOC container
                var container = ContainerConfig.Configure();
                _scope = container.BeginLifetimeScope();

                if (!Environment.UserInteractive)
                {
                    // This is running as a service
                    Logger.Debug("Starting up as a Windows Service.");

                    using (var service = _scope.Resolve<IListenerService>())
                    {
                        ServiceBase.Run(service as ServiceBase);
                    }
                }
                else
                {
                    // This is running as a console application
                    Logger.Debug("Starting up as a console application.");

                    using (var service = _scope.Resolve<IListenerService>())
                    {
                        service.StartService(args);

                        Console.WriteLine("The listener is running - press any key to stop...");
                        Console.ReadKey(true);

                        service.StopService();
                    }
                }
            }
            catch(Exception e)
            {
                Logger.Fatal("Fourth Dataload Message Listener encountered a fatal exception in Main().", e);
            }
        }
    }
}
