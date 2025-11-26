namespace tMoney.WebApi.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IHostEnvironment _environment;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, IHostEnvironment environment, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _environment = environment;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Ocorreu um erro: {Message}", exception.Message);

            var (statusCode, errorTitle) = GetExceptionStatusCode(exception);

            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = statusCode;

            if (_environment.IsDevelopment())
            {
                var response = new
                {
                    StatusCode = statusCode,
                    Error = errorTitle,
                    Description = exception.Message,
                    StackTrace = exception.StackTrace
                };

                await httpContext.Response.WriteAsJsonAsync(response);
            }
            else
            {
                var description = statusCode == StatusCodes.Status500InternalServerError
                    ? "Ocorreu um erro interno no servidor. Tente novamente mais tarde."
                    : exception.Message;

                var response = new
                {
                    StatusCode = statusCode,
                    Error = errorTitle,
                    Description = description
                };

                await httpContext.Response.WriteAsJsonAsync(response);
            }
        }
    }

    private (int statusCode, string errorTitle) GetExceptionStatusCode(Exception exception)
    {
        var statusCode = exception switch
        {
            ArgumentException => (StatusCodes.Status400BadRequest, "Bad Request"),
            InvalidOperationException => (StatusCodes.Status400BadRequest, "Bad Request"),
            UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, "Unauthorized"),
            KeyNotFoundException => (StatusCodes.Status404NotFound, "Not Found"),
            _ => (StatusCodes.Status500InternalServerError, "Internal Server Error")
        };

        return statusCode;
    }
}
