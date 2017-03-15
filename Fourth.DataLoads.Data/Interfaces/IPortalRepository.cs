using Fourth.DataLoads.Data.Entities;
using Fourth.DataLoads.Data.Models;
using Fourth.Orchestration.Model.People;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fourth.DataLoads.Data.Interfaces
{
    public interface IPortalRepository
    {
        bool ProcessMassTerminate(MassTerminationModelSerialized employee, Commands.DataloadRequest payload);
        void CopyTerminationStagingErrorsToPortal(Commands.DataloadRequest payload);
        void CopyRehireStagingErrorsToPortal(Commands.DataloadRequest payload);
        bool CopyDataloadBatchToPortal(Commands.DataloadRequest payload);
        Task<IEnumerable<DataLoadUploads>> GetDataLoadUploads(int groupID, string dateFrom, int dataloadType);
        Task<IEnumerable<ErrorModel>> GetDataLoadErrors(int groupID, string jobID);
        bool ProcessMassRehire(MassRehireModelSerialized employee, Commands.DataloadRequest payload);
    }
}