using Fourth.DataLoads.Data.Entities;
using Fourth.DataLoads.Data.Interfaces;
using Fourth.DataLoads.Listener.Handlers;
using Fourth.Orchestration.Model.People;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fourth.DataLoads.Listener.Services
{
    class MassTerminationService : IMassTerminationService<Commands.CreateAccount>
    {
        public async Task<bool> ProcessPayload(Commands.CreateAccount payload, 
            IDataFactory<MassTerminationModelSerialized> dataFactory)
        {

            var result = dataFactory.GetMassTerminateRepository().GetData(Guid.NewGuid());
            if (result.Count != 0)
                return true;
            return false;

        }
    }
}
