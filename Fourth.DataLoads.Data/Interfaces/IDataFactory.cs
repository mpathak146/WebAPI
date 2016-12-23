namespace Fourth.DataLoads.Data.Interfaces
{
    using Entities;
    using Fourth.DataLoads.Data.Repositories;
    using Interfaces;

    public interface IDataFactory
    {
        /// <summary>
        /// Creates an instance of the Mass Terminate repository.
        /// </summary>
        /// <returns>A repository instance that applications can use to consume data.</returns>
        IAPIRepository<MassTerminationModelSerialized> GetMassTerminateRepository();
        IPortalRepository GetPortalRepository();
        IQueueRepository GetQueueRepository();
    }
}