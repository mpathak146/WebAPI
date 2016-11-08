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
namespace Fourth.DataLoads.ApiEndPoint.Controllers
{
    public class MassTerminateController : ApiController
    {
        string controllerAction = string.Empty;
        /// <summary> Factory that creates data repository instances. </summary>
        private IDataFactory DataFactory { get; }

        /// <summary> The log4net Logger instance. </summary>
        private readonly ILog Logger =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        ///// <summary> The data repository that supplies the data. </summary>

        /// <summary> The authorization provider that checks the validity of requests. </summary>
        private IAuthorizationProvider Authorization { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MassTerminateController"/> class.
        /// </summary>
        public MassTerminateController(IDataFactory dataFactory, IAuthorizationProvider authorization)
        {
            this.DataFactory = dataFactory;
            this.Authorization = authorization;
        }


        [HttpPost]
        [Route("Dataload/Groups/{groupID}/MassTerminate")]
        public async Task<IHttpActionResult> SetDataAsync(string groupID, [FromBody] List<MassTerminate> input)
        {
            controllerAction = "MassTerminate: groupID:" + groupID;
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
                    var result = await repository.SetDataAsync(ID, input);
                    if (result)
                        return Ok();
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