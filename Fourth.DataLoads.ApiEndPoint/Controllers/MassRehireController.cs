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
using log4net;
using Fourth.DataLoads.Data.Models;

namespace Fourth.DataLoads.ApiEndPoint.Controllers
{
    public class MassRehireController : BaseApiController
    {
        private IDataFactory dataFactory;
        private IAuthorizationProvider authorizationProvider;
        private readonly IMappingFactory _mapFactory;
        private string controllerAction;
        /// <summary> The log4net Logger instance. </summary>
        private readonly ILog Logger =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Initializes a new instance of the <see cref="MassRehireController"/> class.
        /// </summary>
        public MassRehireController(IDataFactory factory, 
            IAuthorizationProvider authorization,
            IMappingFactory mapper)
        {
            this.dataFactory = factory;
            this.authorizationProvider = authorization;
            this._mapFactory = mapper;
        }
        [HttpPost]
        [Route("Dataload/Groups/{groupID}/MassRehire")]
        public async Task<IHttpActionResult> SetDataAsync(string groupID, 
            [FromBody] List<MassRehireModel> input)
        {
            int ID;
            var mapper = this._mapFactory.Mapper;
            var serializedmodel = mapper.Map<List<MassRehireModelSerialized>>(input);
            controllerAction = "MassRehire: By User: for groupID:" + groupID;

            
            if (string.IsNullOrEmpty(groupID) 
                || input == null 
                || ! int.TryParse(groupID, out ID))
                return BadRequest();
            else
                int.TryParse(groupID, out ID);
            if (this.authorizationProvider.IsAuthorized(groupID))
            {
                try
                {
                    var repository = this.dataFactory.GetMassRehireRepository();
                    var repoQueue = this.dataFactory.GetQueueRepository();
                    var batchesToSend = await repository
                        .SetDataAsync(base.GetUserContext(), serializedmodel);
                    if (batchesToSend.Count<DataloadBatch>() != 0)
                    {
                        await repoQueue.PushDataAsync(batchesToSend);
                        return Ok();
                    }
                    else
                    {
                        Logger.WarnFormat("Import failed for organisation \"{0}\".", groupID);
                        return NotFound();
                    }
                }
                catch (Exception e)
                {
                    Logger.ErrorFormat("Mass Terminate failed on getting the repository at {0}, with error message {1}",
                        controllerAction, e.Message);
                    throw new HttpResponseException(HttpStatusCode.InternalServerError);
                }
            }
            else
            {
                Logger.WarnFormat("Unauthorized call made to supplier for organisation \"{0}\".", groupID);
                return Unauthorized();
            }
        }
    }
}
