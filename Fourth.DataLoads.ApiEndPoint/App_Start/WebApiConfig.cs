namespace Fourth.DataLoads.ApiEndPoint
{
    using Filters;
    using System.Web.Http;

    /// <summary>
    /// Manages the configuration for the end-point.
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        /// Registers the Http configuration.
        /// </summary>
        /// <param name="config">The http configuration settings.,</param>
        public static void Register(HttpConfiguration config)
        {
            // Register any filters
            config.Filters.Add(new ExceptionFilter());

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });

            // Use XML serializer to remove namespaces from XML declarations - Informatica cannot cope with these
            config.Formatters.XmlFormatter.UseXmlSerializer = true;
        }
    }
}