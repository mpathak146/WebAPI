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
        private readonly IEnumerable<ITableSchema> _tableSchemas;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlDataFactory"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string for the TRGManagement database. </param>
        public SqlDataFactory(string connectionString, IEnumerable<ITableSchema> tableSchemas)
        {
            this._contextFactory = new DBContextFactory(connectionString);
            this._tableSchemas = tableSchemas;
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

        public IRepository<MassTerminationModelSerialized> GetMassTerminateRepository()
        {
            return new MassTerminateRepository(this._contextFactory, _tableSchemas);
        }
        public IDefaultHolidayAllowanceRepository GetDefaultHolidayAllowanceRepository()
        {
            throw new NotImplementedException();
        }
    }
}