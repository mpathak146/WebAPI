namespace Fourth.DataLoads.ApiEndPoint.Authorization
{
    /// <summary>
    /// A dummy authorization provider that authorizes every request.
    /// </summary>
    public class DummyAuthorizionProvider : IAuthorizationProvider
    {
        /// <inheritdoc />
        public bool IsAuthorized(string organisationId)
        {
            return true;
        }
    }
}