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
        void DumpStagingErrorsToPortal(Commands.DataloadRequest payload);
        bool DumpDataloadBatchToPortal(Commands.DataloadRequest payload);
        Task<IEnumerable<DataLoadUploads>> GetDataLoadUploads(int groupID, string dateFrom, int dataloadType);
        Task<IEnumerable<ErrorModel>> GetDataLoadErrors(int groupID, string jobID);
    }
}