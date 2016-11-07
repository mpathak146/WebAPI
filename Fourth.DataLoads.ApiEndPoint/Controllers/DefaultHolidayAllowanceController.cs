namespace Fourth.DataLoads.ApiEndPoint.Controllers
{
    using Authorization;
    using Data;
    using Data.Models;
    using log4net;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http;

    /// <summary>
    /// Exposes an import\export endpoint for default holiday allowances.
    /// </summary>
    public class DefaultHolidayAllowanceController : ApiController
    {
        /// <summary> Factory that creates data repository instances. </summary>
        private IDataFactory DataFactory { get; }

        /// <summary> The log4net Logger instance. </summary>
        private readonly ILog Logger = 
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        ///// <summary> The data repository that supplies the data. </summary>
        //private readonly IDefaultHolidayAllowanceRepository repository;

        /// <summary> The authorization provider that checks the validity of requests. </summary>
        private IAuthorizationProvider Authorization { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultHolidayAllowanceController"/> class.
        /// </summary>
        public DefaultHolidayAllowanceController(IDataFactory dataFactory, IAuthorizationProvider authorization)
        {            
            this.DataFactory = dataFactory;
            this.Authorization = authorization;
        }

        /// <summary>
        /// Exports default holiday allowance figures data via a GET request.
        /// </summary>
        [HttpGet]
        [Route("organisations/{organisationId}/defaultholidayallowance")]
        public async Task<IHttpActionResult> GetDataAsync(string organisationId)
        {
            int groupID;

            if (string.IsNullOrEmpty(organisationId))
                return BadRequest();
            else
                int.TryParse(organisationId, out groupID);

            if (this.Authorization.IsAuthorized(organisationId))
            {
                // Fetch the data
                var repository = this.DataFactory.GetDefaultHolidayAllowanceRepository();
                var data = await repository.GetDataAsync(groupID);

                if (data.Any())
                    return Ok(data);
                else
                    return NotFound();
            }
            else
            {
                Logger.WarnFormat("Unauthorized call made to get default holiday allowance for organisation \"{0}\".", organisationId);                
                return Unauthorized();
            }
        }

        /// <summary>
        /// Exports default holiday allowance figures data pertaining to a single JobTitleID via a GET request .
        /// </summary>
        /// 
        [HttpGet]
        [Route("organisations/{organisationId}/defaultholidayallowance/{jobtitleId:int}")]
        public async Task<IHttpActionResult> GetDataAsync(string organisationId, int jobtitleId)
        {
            int groupID;
            
            if (string.IsNullOrEmpty(organisationId))
                return BadRequest();
            else
                int.TryParse(organisationId, out groupID);

            if (this.Authorization.IsAuthorized(organisationId))
            {
                // Fetch the data
                var repository = this.DataFactory.GetDefaultHolidayAllowanceRepository();
                var data = await repository.GetDataAsync(groupID, jobtitleId);

                if (data.Any())
                    return Ok(data);
                else
                    return NotFound();
            }
            else
            {
                Logger.WarnFormat("Unauthorized call made to get default holiday allowance for organisation \"{0}\".", organisationId);
                return Unauthorized();
            }
        }

        /// <summary>
        /// Imports default holiday allowance figures data pertaining to a single OrganisationID via a POST request .
        /// </summary>
        [HttpPost]
        [Route("Dataload/Groups/{groupID}/defaultholidayallowance")]
        public async Task<IHttpActionResult> SetDataAsync(string groupID, [FromBody] List<DefaultHolidayAllowance> input)
        {
            int ID;

            if (string.IsNullOrEmpty(groupID) || input == null)
                return BadRequest();
            else
                int.TryParse(groupID, out ID);

            if (this.Authorization.IsAuthorized(groupID))
            {
                var repository = this.DataFactory.GetDefaultHolidayAllowanceRepository();
                var result = await repository.SetDataAsync(ID, input);

                if (result)
                    return Ok();
                else
                {
                    Logger.WarnFormat("Import failed for organisation \"{0}\".", groupID);
                    return NotFound();
                }
            }
            else
            {
                Logger.WarnFormat("Unauthorized call made to supplier for organisation \"{0}\".", groupID);
                return Unauthorized();
            }
        }

        /// <summary>
        /// Imports default holiday allowance figures data pertaining to a single OrganisationID via a PUT request .
        /// </summary>
        [HttpPut]
        [Route("organisations/{organisationId}/defaultholidayallowance")]
        public async Task<IHttpActionResult> SetDataAsyncPUT(string organisationId, [FromBody] List<DefaultHolidayAllowance> input)
        {
            int groupID;

            if (string.IsNullOrEmpty(organisationId) || input == null)
                return BadRequest();
            else
                int.TryParse(organisationId, out groupID);

            if (this.Authorization.IsAuthorized(organisationId))
            {
                var repository = this.DataFactory.GetDefaultHolidayAllowanceRepository();
                var result = await repository.SetDataAsync(groupID, input);

                if (result)
                    return Ok();
                else
                {
                    Logger.WarnFormat("Import failed for organisation \"{0}\".", organisationId);
                    return NotFound();
                }
            }
            else
            {
                Logger.WarnFormat("Unauthorized call made to supplier for organisation \"{0}\".", organisationId);
                return Unauthorized();
            }
        }

        /// <summary>
        /// Deletes default holiday allowance figures data pertaining to a single OrgainsationID and JobTitleID via a DELETE request.
        /// </summary>
        [HttpDelete]
        [Route("organisations/{organisationId}/defaultholidayallowance/{jobtitleId:int}")]
        public async Task<IHttpActionResult> DeleteDataAsync(string organisationId, int jobtitleId)
        {
            int groupID;

            if (string.IsNullOrEmpty(organisationId))
                return BadRequest();
            else
                int.TryParse(organisationId, out groupID);

            if (this.Authorization.IsAuthorized(organisationId))
            {
                var repository = this.DataFactory.GetDefaultHolidayAllowanceRepository();
                var result = await repository.DeleteDataAsync(groupID, jobtitleId);

                if (result)
                    return Ok();
                else
                    return NotFound();
            }
            else
            {
                Logger.WarnFormat("Unauthorized call made to delete default holiday allowance data for organisation \"{0}\", job title ID {1}.", organisationId, jobtitleId);
                return Unauthorized();
            }
        }
    }
}