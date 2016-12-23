using Fourth.DataLoads.Data.Interfaces;

namespace Fourth.DataLoads.Data.SqlServer
{
    internal class PortalRepository : IPortalRepository
    {
        private IDBContextFactory _contextFactory;

        public PortalRepository(IDBContextFactory _contextFactory)
        {
            this._contextFactory = _contextFactory;
        }
    }
}