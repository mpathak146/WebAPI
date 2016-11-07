namespace Fourth.DataLoads.ApiEndPoint.Filters
{
    using Errors;
    using log4net;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Filters;

    /// <summary>
    /// This filter fires whenever an unhandled exception is thrown.
    /// </summary>
    public class ExceptionFilter : IExceptionFilter
    {
        /// <summary> The log4net Logger instance. </summary>
        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary> Represents an empty completed task. </summary>
        private static readonly Task CompletedTask = Task.Factory.StartNew(() => { });

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionFilter"/> class the error inserted as the response content.
        /// </summary>
        public ExceptionFilter()
        {
        }

        /// <summary>
        /// Executed whenever an unhandled exception is thrown. Formats the exception into a standard response message.
        /// </summary>
        public Task ExecuteExceptionFilterAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            if (actionExecutedContext != null)
            {
                Exception ex = null;
                var details = new List<string>();

                if (actionExecutedContext.Exception != null)
                {
                    // Handle aggregate exceptions by returning a message for each inner exception.
                    if (actionExecutedContext.Exception.GetType().Name == "AggregateException")
                    {
                        var ae = actionExecutedContext.Exception as AggregateException;
                        ae.Flatten();
                        foreach (var innerEx in ae.InnerExceptions)
                        {
                            details.Add(innerEx.Message);
                        }
                    }

                    // Decide which exception to report on
                    if (actionExecutedContext.Exception.InnerException != null)
                    {
                        ex = actionExecutedContext.Exception.InnerException;
                    }
                    else
                    {
                        ex = actionExecutedContext.Exception;
                    }
                }

                // If we have an HttpResponseException then return the response embedded in the exception (e.g. HTTP 415 "Unsupported media type")
                if (ex is HttpResponseException)
                {
                    var typedEx = ex as HttpResponseException;

                    // Log the error
                    Logger.Error(string.Format("HTTP error. Status code: {0}; Reason: {1}", typedEx.Response.StatusCode, typedEx.Response.ReasonPhrase), typedEx);

                    // Return the embedded response
                    actionExecutedContext.Response = typedEx.Response;
                }
                else
                {
                    // Log the error
                    Logger.Error(ex);

                    // Return the error information in our custom model
                    var model = new ErrorModel
                    {
                        Exception = ex,
                        Message = ex.Message,
                        StatusCode = HttpStatusCode.InternalServerError,
                        Details = details.AsEnumerable<string>()
                    };

                    // Send the response
                    actionExecutedContext.Response = new ErrorResult(
                        HttpStatusCode.InternalServerError,
                        actionExecutedContext.Request,
                        model)
                        .ExecuteAsync(cancellationToken).Result;
                }
            }

            return CompletedTask;
        }

        public bool AllowMultiple
        {
            get { return false; }
        }
    }
}