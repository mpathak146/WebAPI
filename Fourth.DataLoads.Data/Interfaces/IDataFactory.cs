using Fourth.DataLoads.Data.Entities;
using Fourth.PSLiveDataLoads.ApiEndPoint;
namespace Fourth.DataLoads.Data.Interfaces
{
    public interface IDataFactory
    {
        /// <summary>
        /// Creates an instance of the Mass Terminate repository.
        /// </summary>
        /// <returns>A repository instance that applications can use to consume data.</returns>
        IStagingRepository<MassTerminationModelSerialized> GetMassTerminateRepository();
        IPortalRepository GetPortalRepository();
        IQueueRepository GetQueueRepository();

        IPortalVerificationRepository GetPortalVerificationRepository();
        IStagingRepository<MassRehireModelSerialized> GetMassRehireRepository();
    }
}