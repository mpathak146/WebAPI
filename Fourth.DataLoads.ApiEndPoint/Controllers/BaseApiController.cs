using Fourth.DataLoads.Data.Repositories;
using System.Collections.Generic;
using System.Web.Http;
using System.Linq;

namespace Fourth.DataLoads.ApiEndPoint.Controllers
{
    public abstract class BaseApiController: ApiController
    {
        public ApiConfigurationSection Config { get; private set; }

        public BaseApiController(ApiConfigurationSection config = null)
        {
            if (config == null)
            {
                config = ApiConfigurationSection.GetConfig();
            }
            Config = config;
        }
        protected UserContext GetUserContext()
        {
            string userId = string.Empty;
            string organisationId = string.Empty;

            if (this.Request != null && this.Request.Headers != null)
            {
                IEnumerable<string> values;
                if (this.Request.Headers.TryGetValues(Constants.HeaderOrg, out values))
                {
                    organisationId = values.FirstOrDefault();
                }
                if (this.Request.Headers.TryGetValues(Constants.HeaderUser, out values))
                {
                    userId = values.FirstOrDefault();
                }
            }

            return new UserContext(userId, organisationId);
        }
    }
}