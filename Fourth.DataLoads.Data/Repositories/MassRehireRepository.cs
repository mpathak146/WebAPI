using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fourth.DataLoads.Data.Interfaces;
using Fourth.DataLoads.Data.Models;
using Fourth.DataLoads.Data.Repositories;
using Fourth.DataLoads.Data.Entities;
using System.Collections;

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

        public async Task<IEnumerable<DataloadBatch>> SetDataAsync(UserContext userContext, List<MassRehireModelSerialized> input)
        {
            var batchesToSent = new List<DataloadBatch>();
            batchesToSent.Add(new DataloadBatch
            {
                BatchID = Guid.Parse("bf0a634d-c76a-4f53-838d-ec9742e5d348"),
                JobID = Guid.Parse("bf0a634d-c76a-4f53-838d-ec9742e5d348"),
                OrganizationID = "85",
                User = "StubUser"
            });
            return batchesToSent;
        }
    }
}