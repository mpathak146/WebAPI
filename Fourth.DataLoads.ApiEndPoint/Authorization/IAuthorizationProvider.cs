namespace Fourth.DataLoads.ApiEndPoint.Authorization
{
    /// <summary>
    /// Authorizes incoming requests based on the user's organisation.
    /// </summary>
    public interface IAuthorizationProvider
    {
        /// <summary>
        /// Indicates whether the current request is valid for the supplied organisation.
        /// </summary>
        /// <param name="organisationId">The unique identifier for the organisation.</param>
        /// <returns>An indication of whether the request is authorized.</returns>
        bool IsAuthorized(string organisationId);
    }
}