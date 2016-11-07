namespace Fourth.DataLoads.ApiEndPoint.Errors
{
    using System;
    using System.Collections.Generic;
    using System.Net;

    /// <summary> Model to that provides a standard structure for error responses. </summary>
    public class ErrorModel
    {
        /// <summary>
        /// Gets or sets the Http status code associated with this response.
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }

        /// <summary>
        /// Gets or sets any specific code associated with the error.
        /// </summary>
        public string ErrorCode { get; set; }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets any detailed error messages associated with the error (e.g. validation errors)
        /// </summary>
        public IEnumerable<string> Details { get; set; }

        /// <summary>
        /// Gets or sets a help URL associated with the error.
        /// </summary>
        public Uri HelpUrl { get; set; }

        /// <summary>
        /// Gets or sets the details of any underlying exception.
        /// </summary>
        /// <remarks>
        /// Note that this should not be populated in production.
        /// </remarks>
        public Exception Exception { get; set; }
    }
}