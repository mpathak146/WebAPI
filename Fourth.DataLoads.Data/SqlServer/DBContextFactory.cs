namespace Fourth.DataLoads.Data.SqlServer

{
    using Entities.ManagementDB;
    using System;
    using System.Configuration;
    using System.Data;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Xml;
    using System.Xml.Linq;
    using Fourth.DataLoads.Data.Interfaces;

    /// <summary>
    /// A factory class that creates data contexts for SQL Server.
    /// </summary>
    public class DBContextFactory : IDBContextFactory
    {
        private const string CONNECTION_STRING_FORMAT = "Server={0};Database={1};User ID={2};Password={3};Connection Timeout=30;MultipleActiveResultSets=True;persist security info=True;";
        /// <summary>
        /// The connection string to use in database connections.
        /// </summary>
        public string _ConnectionString { get; set; }

        ///// <summary>
        ///// Initializes a new instance of the <see cref="PSLivePortalDBContextFactoryAsync"/> class.
        ///// </summary>

        /// <inheritdoc />
        public async Task<PortalDBContext> GetPortalDBContextAsync(int groupId)
        {
            _ConnectionString = await GetConnectionString(groupId);
            return new PortalDBContext(_ConnectionString);
        }

        public StagingDBContext GetStagingDBContext()
        {
            _ConnectionString = ConfigurationManager.ConnectionStrings["DataloadsContext"].ConnectionString;
            return new StagingDBContext(_ConnectionString);
        }
        /// <summary>
        /// Gets the connection details for a particular group id (aka organisation).
        /// </summary>
        /// <param name="groupId">The customer identifier.</param>
        /// <returns>A connection string to the customer database.</returns>
        private async Task<string> GetConnectionString(long groupId)
        {
            //string output = string.Empty;
            string file = AppSettings.PathConfig;
            if (_ConnectionString != string.Empty)
                if (file != "")
                {
                    XmlDocument xmlFile = new XmlDocument();
                    xmlFile.Load(file);
                    XDocument xdoc = XDocument.Load(file);
                    var dsns = from dsn in xdoc.Elements("Settings").Elements("DSNs").Elements("DSN")
                               where dsn.Attribute("GroupID").Value == groupId.ToString()
                               select new
                               {
                                   UserName = dsn.Attribute("Username").Value,
                                   Password = dsn.Attribute("Password").Value,
                                   DataSource = dsn.Attribute("DataSource").Value,
                                   SchemaName = dsn.Attribute("SchemaName").Value
                               };
                    var g_dsn = dsns.SingleOrDefault();
                    if (g_dsn != null)
                        _ConnectionString = string.Format(CONNECTION_STRING_FORMAT, g_dsn.DataSource,
                            g_dsn.SchemaName, g_dsn.UserName, g_dsn.Password);

                }
            return _ConnectionString;
        }
    }

}