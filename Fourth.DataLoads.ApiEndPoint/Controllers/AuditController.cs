using Fourth.DataLoads.ApiEndPoint.Authorization;
using Fourth.DataLoads.ApiEndPoint.Mappers;
using Fourth.DataLoads.Data.Interfaces;
using Fourth.DataLoads.Data.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Fourth.DataLoads.ApiEndPoint.Controllers
{
    public class AuditController : BaseApiController
    {
        private IDataFactory DataFactory { get; }
        string controllerAction = string.Empty;

        /// <summary> The log4net Logger instance. </summary>
        private readonly ILog Logger =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        ///// <summary> The data repository that supplies the data. </summary>

        /// <summary> The authorization provider that checks the validity of requests. </summary>
        private IAuthorizationProvider Authorization { get; }
        public AuditController(IDataFactory dataFactory,
            IAuthorizationProvider authorization)
        {
            this.DataFactory = dataFactory;
            this.Authorization = authorization;
        }
        [HttpGet]
        [Route("Dataload/Groups/{groupID}/Jobs")]
        public async Task<IHttpActionResult> GetDataAsync(string groupID, string dateFrom=null)
        {
            controllerAction = "MassTerminate: By User: for groupID:" + groupID;
            Logger.Info(controllerAction);
            int ID;
            IEnumerable<DataLoadUploads> dataloads=null;
            try
            {
                if (string.IsNullOrEmpty(groupID))
                    return BadRequest();
                else
                    int.TryParse(groupID, out ID);
                if (this.Authorization.IsAuthorized(groupID))
                {
                    dataloads = await DataFactory.GetPortalRepository().GetDataLoadUploads
                        (ID ,(dateFrom!=null) ? dateFrom : "");
                    return Ok(dataloads.ToList());
                }
                else
                {
                    Logger.WarnFormat("Unauthorized call made to supplier for organisation \"{0}\".", groupID);
                    return Unauthorized();
                }
            }
            catch (Exception e)
            {
                Logger.ErrorFormat("Failed to retrieve dataload information from the database, error: {0}", e.Message);
                return BadRequest(e.Message);
            }
        }


        [HttpGet]
        [Route("Dataload/Groups/{groupID}/Jobs/{jobID}")]
        public async Task<IHttpActionResult> GetErrorDataAsync(string groupID, string jobID)
        {
            controllerAction = "MassTerminate: By User: for groupID:" + groupID;
            Logger.Info(controllerAction);
            int ID;
            IEnumerable<ErrorModel> errors = null;
            try
            {
                if (string.IsNullOrEmpty(groupID))
                    return BadRequest();
                else
                    int.TryParse(groupID, out ID);
                if (this.Authorization.IsAuthorized(groupID))
                {
                    errors = await DataFactory.GetPortalRepository().GetDataLoadErrors(ID, jobID);
                    return Ok(errors.ToList());
                }
                else
                {
                    Logger.WarnFormat("Unauthorized call made to supplier for organisation \"{0}\".", groupID);
                    return Unauthorized();
                }
            }
            catch (Exception e)
            {
                Logger.ErrorFormat("Failed to retrieve dataload information from the database, error: {0}", e.Message);
                return BadRequest();
            }
        }
    }
}
