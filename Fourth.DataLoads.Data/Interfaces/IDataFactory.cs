namespace Fourth.DataLoads.Data.Interfaces
{
    using Fourth.DataLoads.Data.Repositories;
    using Interfaces;

    public interface IDataFactory<T>
    {
        /// <summary>
        /// Creates an instance of the default holiday allowance repository.
        /// </summary>
        /// <returns>A repository instance that applications can use to consume data.</returns>
        IDefaultHolidayAllowanceRepository GetDefaultHolidayAllowanceRepository();

        /// <summary>
        /// Creates an instance of the Mass Terminate repository.
        /// </summary>
        /// <returns>A repository instance that applications can use to consume data.</returns>
        IRepository<T> GetMassTerminateRepository();

    }
}