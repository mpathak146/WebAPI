using Fourth.DataLoads.Data.Entities;
using Fourth.Orchestration.Model.People;

namespace Fourth.DataLoads.Data.Interfaces
{
    public interface IPortalRepository
    {
        bool ProcessMassTerminate(MassTerminationModelSerialized employee, Commands.CreateAccount payload);
        void RecordStagingErrors(Commands.CreateAccount payload);

        bool RecordDataloadBatch(Commands.CreateAccount payload);
    }
}