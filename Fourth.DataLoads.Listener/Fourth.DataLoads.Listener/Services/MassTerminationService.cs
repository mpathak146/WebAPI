﻿using Fourth.DataLoads.Data.Entities;
using Fourth.DataLoads.Data.Interfaces;
using Fourth.DataLoads.Listener.Handlers;
using Fourth.Orchestration.Model.People;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fourth.DataLoads.Listener.Services
{
    class MassTerminationService : IMassTerminationService<Commands.CreateAccount>
    {
        public async Task<bool> ProcessPayload(Commands.CreateAccount payload, 
            IDataFactory dataFactory)
        {

            var result = dataFactory.GetMassTerminateRepository().GetValidBatch(Guid.Parse(payload.FirstName));
            foreach(var emp in result)
            {
                dataFactory.GetPortalRepository().ProcessMassTerminate(emp,payload);
            }
            if (result.Count != 0)
                return true;
            return false;

        }
    }
}
