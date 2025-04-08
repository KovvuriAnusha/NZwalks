using System.Net;

namespace NZWalks.API.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly ILogger<ExceptionHandlerMiddleware> logger;
        private readonly RequestDelegate request;

        public ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> logger
            , RequestDelegate request)
        {
            this.logger = logger;
            this.request = request;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                // Log the errors at one place
                await this.request(httpContext);
            }
            catch (Exception ex)
            {
                var errorId = Guid.NewGuid().ToString();
                logger.LogError(ex, $"{errorId} : {ex.Message}");

                //Return custom error message
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                httpContext.Response.ContentType = "application/json";

                var error = new 
                {
                    error = errorId,
                    ErrorMessage = "Something went wrong, we are looking into it."
                };
                await httpContext.Response.WriteAsJsonAsync(error);
            }
        }
    }
}
