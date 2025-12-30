namespace luckyoneApiv3.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(Exception ex)
            {
               _logger.LogError($"Something went wrong: {ex}");

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";

                var response = new
                {
                    success = false,
                    message = "An unexpected error occurred.",
                    statusCode = context.Response.StatusCode,
                    traceId = context.TraceIdentifier
                };

                await context.Response.WriteAsJsonAsync(response);

            }
        }

    }
}
