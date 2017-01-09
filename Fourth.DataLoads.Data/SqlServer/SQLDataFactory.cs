namespace Fourth.DataLoads.Data.SqlServer
{
    using Entities;
    using Fourth.DataLoads.Data;
    using Fourth.DataLoads.Data.Repositories;
    using Interfaces;
    using System.Collections;
    using System.Collections.Generic;
    using System;
    /// <summary>
    /// Creates MS Sql Server repository instances for consuming applications.
    /// </summary>
    public class SqlDataFactory : IDataFactory    {
        /// <summary> The factory that creates database contexts. </summary>
        private readonly IDBContextFactory _contextFactory;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlDataFactory"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string for the TRGManagement database. </param>
        public SqlDataFactory()
        {
            this._contextFactory = new DBContextFactory();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlDataFactory"/> class.
        /// </summary>
        /// <remarks> Allows a mocked context factory to be injected. </remarks>
        internal SqlDataFactory(IDBContextFactory contextFactory)
        {
            this._contextFactory = contextFactory;
        }

        /// <inheritdoc />

        public IStagingRepository<MassTerminationModelSerialized> GetMassTerminateRepository()
        {
            return new MassTerminateRepository(this._contextFactory);
        }

        public IPortalRepository GetPortalRepository()
        {
            return new PortalRepository(this._contextFactory);
        }
        public IQueueRepository GetQueueRepository()
        {
            return new QueueRepository();
        }
    }
}