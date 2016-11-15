namespace Fourth.DataLoads.ApiEndPoint
{
    using Authorization;
    using Autofac;
    using Autofac.Integration.WebApi;
    using Data;
    using Fourth.DataLoads.Data.SqlServer;
    using log4net;
    using log4net.Config;
    using System;
    using System.Configuration;
    using System.Reflection;
    using System.Web.Http;
    using System.Web.Mvc;

    public class Global : System.Web.HttpApplication
    {
        /// <summary>The class logger.</summary>
        private static ILog Logger;

        protected void Application_Start(object sender, EventArgs e)
        {
            // Initialise log4net
            XmlConfigurator.Configure();
            log4net.GlobalContext.Properties["ApplicationId"] = typeof(Global).Namespace;
            Logger = LogManager.GetLogger(typeof(Global));
            Logger.Info("Starting application.");
            
            try
            {
                AreaRegistration.RegisterAllAreas();
                GlobalConfiguration.Configure(WebApiConfig.Register);
                var config = GlobalConfiguration.Configuration;

                // Kick off Autofac
                var builder = new ContainerBuilder();

                // Register the Web API controllers - only one statement is needed to register them all
                builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

                var appConfig = ApiConfigurationSection.GetConfig();

                // Determine which authorization to use, i.e. header or dummy
                if (appConfig.Authorization == ApiConfigurationSection.AuthorizationMode.Header)
                {
                    builder.RegisterType<RequestHeaderAuthorizionProvider>().As<IAuthorizationProvider>().InstancePerRequest();
                }
                else if (appConfig.Authorization == ApiConfigurationSection.AuthorizationMode.Disabled)
                {
                    builder.RegisterType<DummyAuthorizionProvider>().As<IAuthorizationProvider>().InstancePerRequest();
                }
                else
                {
                    throw new NotImplementedException(string.Format("The authorization mode \"{0}\" has not been implemented.", appConfig.Authorization));
                }

                // Add any other dependencies here, i.e. the repositories and context factory
                builder.RegisterType<SqlDataFactory>().As<IDataFactory>().InstancePerRequest()
                    .WithParameter("connectionString", ConfigurationManager.ConnectionStrings["DataloadsContext"].ConnectionString);

                
                // Set Autofac as the dependency resolver
                var container = builder.Build();
                config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
                
            }
            catch (Exception ex)
            {
                Logger.Fatal("The application failed to initialize", ex);
            }
        }
    }
}