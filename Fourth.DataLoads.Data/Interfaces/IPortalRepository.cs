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
        bool ProcessMassTerminate(MassTerminationModelSerialized employee, Commands.CreateAccount payload);
        void DumpStagingErrorsToPortal(Commands.CreateAccount payload);
        bool DumpDataloadBatchToPortal(Commands.CreateAccount payload);
        Task<IEnumerable<DataLoadUploads>> GetDataLoadUploads(int groupID, string dateFrom, DataLoadTypes dataloadTypes);
        Task<IEnumerable<ErrorModel>> GetDataLoadErrors(int groupID, string jobID);
    }
}