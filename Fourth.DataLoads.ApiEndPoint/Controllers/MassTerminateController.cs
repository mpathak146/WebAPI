using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using Fourth.DataLoads.Data.Models;
using Fourth.DataLoads.Data;
using log4net;
using Fourth.DataLoads.ApiEndPoint.Authorization;
using System.Web;
using Fourth.DataLoads.Data.Entities;
using AutoMapper;
using Fourth.DataLoads.ApiEndPoint.Mappers;
using Fourth.DataLoads.Data.Interfaces;
using Fourth.DataLoads.Data.Repositories;

namespace Fourth.DataLoads.ApiEndPoint.Controllers
{
    public class MassTerminateController : BaseApiController
    {
        string controllerAction = string.Empty;
        /// <summary> Factory that creates data repository instances. </summary>
        private IDataFactory<MassTerminationModelSerialized> DataFactory { get; }

        /// <summary> The log4net Logger instance. </summary>
        private readonly ILog Logger =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        ///// <summary> The data repository that supplies the data. </summary>

        private readonly IMapper _mapper;
        /// <summary> The authorization provider that checks the validity of requests. </summary>
        private IAuthorizationProvider Authorization { get; }

        private readonly IMappingFactory _mapFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="MassTerminateController"/> class.
        /// </summary>
        public MassTerminateController(IDataFactory<MassTerminationModelSerialized> dataFactory, 
            IAuthorizationProvider authorization, IMappingFactory mapFactory)
        {
            this.DataFactory = dataFactory;
            this.Authorization = authorization;
            this._mapFactory = mapFactory;            
        }


        [HttpPost]
        [Route("Dataload/Groups/{groupID}/MassTerminate")]
        public async Task<IHttpActionResult> SetDataAsync(string groupID, 
            [FromBody] List<MassTerminationModel> input)
        {
            var mapper = this._mapFactory.Mapper;
            var serializedmodel = mapper.Map<List<MassTerminationModelSerialized>>(input);
            controllerAction = "MassTerminate: By User: for groupID:" + groupID;
            int ID;
            if (string.IsNullOrEmpty(groupID) || input == null)
                return BadRequest();
            else
                int.TryParse(groupID, out ID);

            if (this.Authorization.IsAuthorized(groupID))
            {
                try
                {
                    var repository = this.DataFactory.GetMassTerminateRepository();
                    var insertedID = await repository.SetDataAsync(base.GetUserContext(), serializedmodel);
                    if (insertedID.ToString()!=string.Empty)
                    {
                        //await AzureSender.Instance.SendAsync(null);
                        //To Do
                        /* This is where orchestration model will be initialized
                         * insertedID and dataload type enum will be updated to model
                         * and a call to the Azure Bus queue will be made to prepare request                         *
                         */
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