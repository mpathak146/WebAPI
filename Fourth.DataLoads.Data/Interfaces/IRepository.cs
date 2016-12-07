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
    public interface IRepository<T>
    {
        Task<IEnumerable<ITableSchema>> GetTableSchema();

        Task<Guid> SetDataAsync(UserContext userContext, List<T> input);
    }
}
