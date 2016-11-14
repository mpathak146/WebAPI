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
    

    /// <summary>
    /// A factory class that creates data contexts for SQL Server.
    /// </summary>
    public class PortalDBContextFactory : IPortalDBContextFactory
    {
        private const string CONNECTION_STRING_FORMAT = "Server={0};Database={1};User ID={2};Password={3};Connection Timeout=30;MultipleActiveResultSets=True;persist security info=True;";
        /// <summary>
        /// The connection string to use in database connections.
        /// </summary>
        private string _ConnectionString;

        ///// <summary>
        ///// Initializes a new instance of the <see cref="PSLivePortalDBContextFactoryAsync"/> class.
        ///// </summary>
        public PortalDBContextFactory(string connectionString)
        {
            this._ConnectionString = connectionString;
        }

        /// <inheritdoc />
        public async Task<PortalDBContext> GetContextAsync(int groupId)
        {
            return new PortalDBContext(await GetConnectionForGroupAsync(groupId));
        }

        /// <summary>
        /// Gets the connection details for a particular group id (aka organisation).
        /// </summary>
        /// <param name="groupId">The customer identifier.</param>
        /// <returns>A connection string to the customer database.</returns>
        private async Task<string> GetConnectionForGroupAsync(long groupId)
        {
            //string output = string.Empty;
            string file = AppSettings.PathConfig;
            if (_ConnectionString == string.Empty)
                if (file != "")
                {
                    XmlDocument xmlFile = new XmlDocument();
                    xmlFile.Load(file);

                    XmlNode node = xmlFile.SelectSingleNode("/Settings/DSNs/DSN[@GroupID='" + groupId + "']");
                    if (node != null)
                    {
                        _ConnectionString = string.Format(CONNECTION_STRING_FORMAT, node.Attributes["DataSource"].Value,
                            node.Attributes["SchemaName"].Value, node.Attributes["Username"].Value, node.Attributes["Password"].Value);
                    }
                    else
                    {
                        throw new Exception("Database not configured on XML");
                    }
                }
                else
                    throw new Exception("Database File not configured on app.Config");
            return _ConnectionString;

            //    using (var context = new PSLiveManagementDBContext(this._ConnectionString))
            //    {
            //        var group = await context.Groups.Where(g => g.GroupId == groupId)
            //                         .AsNoTracking()
            //                         .FirstOrDefaultAsync<DSNTable>();
            //        if (group == null)
            //        {
            //            throw new ArgumentException(string.Format("No connection details could be found for the group ID \"{0}\".", groupId));
            //        }

            //        output = string.Format(CONNECTION_STRING_FORMAT, group.DataSource, group.SchemaName, group.Username, group.Password);

            //    }
            //}
        }
    }
}