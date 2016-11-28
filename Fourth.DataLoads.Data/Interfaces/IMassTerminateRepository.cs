using Fourth.DataLoads.Data.Entities;
using Fourth.DataLoads.Data.Models;
using Fourth.DataLoads.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fourth.DataLoads.Data.Interfaces
{
    public interface IMassTerminateRepository
    {
        /// <summary>
        /// Updates a submitted list of default holiday allowances.
        /// </summary>
        /// <param name="groupID">The identifier of the groupID.</param>
        /// <param name="input">The list of default holiday allawances to update.</param>
        /// <returns>An indication of whether any records were updated.</returns>
        Task<bool> SetDataAsync(UserContext groupID, List<MassTerminationModelSerialized> input);

    }
}
