namespace Fourth.DataLoads.Data.SqlServer
{
    using Entities;
    using Fourth.DataLoads.Data;
    using Fourth.DataLoads.Data.Repositories;
    using Interfaces;

    /// <summary>
    /// Creates MS Sql Server repository instances for consuming applications.
    /// </summary>
    public class SqlDataFactory : IDataFactory
    {
        /// <summary> The factory that creates database contexts. </summary>
        private readonly IDBContextFactory _ContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlDataFactory"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string for the TRGManagement database. </param>
        public SqlDataFactory(string connectionString)
        {
            this._ContextFactory = new DBContextFactory(connectionString);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlDataFactory"/> class.
        /// </summary>
        /// <remarks> Allows a mocked context factory to be injected. </remarks>
        internal SqlDataFactory(IDBContextFactory contextFactory)
        {
            this._ContextFactory = contextFactory;
        }

        /// <inheritdoc />
        public IDefaultHolidayAllowanceRepository GetDefaultHolidayAllowanceRepository()
        {
            return new DefaultHolidayAllowanceRepository(this._ContextFactory);
        }
        public IMassTerminateRepository GetMassTerminateRepository()
        {
            return new MassTerminateRepository(this._ContextFactory);
        }

    }
}