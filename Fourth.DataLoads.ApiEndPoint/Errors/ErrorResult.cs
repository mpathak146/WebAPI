namespace Fourth.DataLoads.ApiEndPoint.Errors
{
    using log4net;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http;

    /// <summary>
    /// Generic IHttpExceptionResponse returning an ErrorModel in the content
    /// </summary>
    public class ErrorResult : IHttpActionResult
    {
        /// <summary> The Http status code associated with this response. </summary>
        private readonly HttpStatusCode statusCode;

        /// <summary> The request message. </summary>
        public readonly HttpRequestMessage request;

        /// <summary> The details of any error. </summary>
        private ErrorModel error;

        /// <summary> The logger to use for this class. </summary>
        private static ILog logger = LogManager.GetLogger(typeof(ErrorResult));

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorResult"/> class the error inserted as the response content.
        /// </summary>
        public ErrorResult(HttpStatusCode statusCode, HttpRequestMessage request, ErrorModel error)
        {
            this.statusCode = statusCode;
            this.request = request;
            this.error = error;
        }

        /// <summary>
        /// Gets or sets the underlying error information.
        /// </summary>
        public ErrorModel ErrorModel
        {
            get
            {
                return this.error;
            }

            set
            {
                this.error = value;
            }
        }

        /// <summary>
        /// Formats the underlying error model as a response.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>A task representing the response.</returns>
        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = this.request.CreateResponse(this.statusCode, this.error, "application/json");
            return Task.FromResult(response);
        }
    }
}