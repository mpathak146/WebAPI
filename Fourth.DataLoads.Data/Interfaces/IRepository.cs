using Fourth.DataLoads.Data.Entities;
using Fourth.DataLoads.Data.Models;
using Fourth.DataLoads.Data.Repositories;
using Google.ProtocolBuffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fourth.DataLoads.Data.Interfaces
{
    public interface IAPIRepository<T>
    {
        Task<IEnumerable<ITableSchema>> GetTableSchema();
        Task<IEnumerable<DataloadBatch>> SetDataAsync(UserContext userContext, List<T> input);
        bool IsValid(T genericType);       
        List<T> GetData(Guid batchID);
    }
}
