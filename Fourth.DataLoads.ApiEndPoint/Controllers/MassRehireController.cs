using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Fourth.DataLoads.ApiEndPoint.Authorization;
using Fourth.DataLoads.Data.Interfaces;

namespace Fourth.DataLoads.ApiEndPoint.Controllers
{
    public class MassRehireController : ApiController
    {
        private IDataFactory dataFactory;
        private IAuthorizationProvider authorizationProvider;

        public MassRehireController(IDataFactory fact, 
            IAuthorizationProvider auth)
        {
            this.dataFactory = fact;
            this.authorizationProvider = auth;
        }

        public Task<IHttpActionResult> SetDataAsync(string v, object models)
        {
            throw new NotImplementedException();
        }
    }
}
