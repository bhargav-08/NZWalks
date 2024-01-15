using System.Net;

namespace NZWalks.API.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly ILogger<ExceptionHandlerMiddleware> logger;
        private readonly RequestDelegate next;

        public ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> logger, RequestDelegate next)
        {
            this.logger = logger;
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {

            try
            {
                await next(httpContext);
            }
            catch (Exception ex)
            {
                // log this exception
                logger.LogError(ex, ex.Message);

                // return custom error message response
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                httpContext.Response.ContentType = "application/json";

                var errorId = Guid.NewGuid();

                await httpContext.Response.WriteAsJsonAsync(new { Id = errorId, ErrorMessage = "Something went Wrong ! We are looking to resolve it." });
            }
        }

    }
}
