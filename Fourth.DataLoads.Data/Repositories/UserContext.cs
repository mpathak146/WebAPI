

namespace Fourth.DataLoads.Data.Repositories
{
    public class UserContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserContext"/> class.
        /// </summary>
        public UserContext(string userId, string organisationId)
        {
            this.UserId = userId;
            this.OrganisationId = organisationId;
        }

        /// <summary>
        /// The public user identifier - this normally corresponds to the X-Fourth-UserId header.
        /// </summary>
        public string UserId
        { get; protected set; }

        /// <summary>
        /// The customer public identifier - this normally corresponds to the X-Fourth-Org header.
        /// </summary>
        public string OrganisationId
        { get; protected set; }
    }
}
