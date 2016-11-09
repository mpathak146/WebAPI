namespace Fourth.DataLoads.ApiEndPoint.Authorization
{
    using System;
    using System.Web;

    /// <summary>
    /// Authorizes a request for an organisation's data based on the HTTP header.
    /// </summary>
    public class RequestHeaderAuthorizionProvider : IAuthorizationProvider
    {
        /// <summary> The HTTP header that contains the authorized organisation identifier. </summary>
        private const string HeaderName = "X-Fourth-Org";

        /// <inheritdoc />
        public bool IsAuthorized(string organisationId)
        {
            if (HttpContext.Current != null && HttpContext.Current.Request != null && HttpContext.Current.Request.Headers != null)
            {
                var header = HttpContext.Current.Request.Headers[Constants.HeaderName];
                if (!string.IsNullOrEmpty(header))
                {
                    return header.Equals(organisationId, StringComparison.InvariantCultureIgnoreCase);
                }
            }

            return false;
        }
    }
}