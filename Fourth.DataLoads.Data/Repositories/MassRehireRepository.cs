using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fourth.DataLoads.Data.Interfaces;
using Fourth.DataLoads.Data.Models;
using Fourth.DataLoads.Data.Repositories;
using Fourth.DataLoads.Data.Entities;

namespace Fourth.DataLoads.Data.SqlServer
{
    internal class MassRehireRepository : IStagingRepository<MassRehireModelSerialized>
    {
        private IDBContextFactory _contextFactory;

        public MassRehireRepository(IDBContextFactory _contextFactory)
        {
            this._contextFactory = _contextFactory;
        }

        public Task<IEnumerable<ITableSchema>> GetTableSchema()
        {
            throw new NotImplementedException();
        }

        public List<MassRehireModelSerialized> GetValidBatch(Guid batchID)
        {
            throw new NotImplementedException();
        }

        public bool IsValid(MassRehireModelSerialized genericType)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DataloadBatch>> SetDataAsync(UserContext userContext, List<MassRehireModelSerialized> input)
        {
            throw new NotImplementedException();
        }
    }
}