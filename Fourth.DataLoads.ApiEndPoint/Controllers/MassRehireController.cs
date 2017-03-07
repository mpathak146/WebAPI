using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Fourth.DataLoads.ApiEndPoint.Authorization;
using Fourth.DataLoads.Data.Interfaces;
using Fourth.DataLoads.ApiEndPoint.Mappers;
using Fourth.DataLoads.Data.Entities;

namespace Fourth.DataLoads.ApiEndPoint.Controllers
{
    public class MassRehireController : ApiController
    {
        private IDataFactory dataFactory;
        private IAuthorizationProvider authorizationProvider;
        private readonly IMappingFactory _mapFactory;
        private string controllerAction;

        public MassRehireController(IDataFactory factory, 
            IAuthorizationProvider authorization,
            IMappingFactory mapper)
        {
            this.dataFactory = factory;
            this.authorizationProvider = authorization;
            this._mapFactory = mapper;
        }

        public async Task<IHttpActionResult> SetDataAsync(string groupID, 
            [FromBody] List<MassRehireModel> input)
        {
            int ID;
            var mapper = this._mapFactory.Mapper;
            var serializedmodel = mapper.Map<List<MassRehireModelSerialized>>(input);
            controllerAction = "MassTerminate: By User: for groupID:" + groupID;

            
            if (string.IsNullOrEmpty(groupID) 
                || input == null 
                || ! int.TryParse(groupID, out ID))
                return BadRequest();
            else
                int.TryParse(groupID, out ID);

            return Ok();
        }
    }
}
