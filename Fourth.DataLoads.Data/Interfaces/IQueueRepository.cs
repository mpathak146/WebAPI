using Fourth.DataLoads.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fourth.DataLoads.Data.Interfaces
{
    public interface IQueueRepository
    {
        Task<bool> PushDataAsync(IEnumerable<DataloadBatch> batches);
    }
}