using Serilog.Context;


namespace Capstone_dotnet.Logging
{
    /// <summary>
    /// Middleware that ensures every request has a correlation ID for distributed tracing.
    /// Reads X-Correlation-Id from request headers or generates a new GUID.
    /// Pushes the correlation ID into Serilog's LogContext for automatic inclusion in logs.
    /// </summary>
    public class CorrelationIdMiddleware
    {
        private readonly RequestDelegate _next;
        private const string CorrelationIdHeader = "X-Correlation-Id";

        public CorrelationIdMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Read correlation ID from request header or generate new one
            var correlationId = GetOrCreateCorrelationId(context);

            // Store in HttpContext for access throughout the request pipeline
            context.Items["CorrelationId"] = correlationId;

            // Add to response headers for client tracking
            context.Response.OnStarting(() =>
            {
                if (!context.Response.Headers.ContainsKey(CorrelationIdHeader))
                {
                    context.Response.Headers[CorrelationIdHeader] = correlationId;
                }
                return Task.CompletedTask;
            });

            // Push correlation ID into Serilog's LogContext
            // This ensures all logs within this request automatically include the correlation ID
            using (LogContext.PushProperty("CorrelationId", correlationId))
            {
                await _next(context);
            }
        }

        /// <summary>
        /// Retrieves correlation ID from request header or generates a new GUID.
        /// </summary>
        private string GetOrCreateCorrelationId(HttpContext context)
        {
            if (context.Request.Headers.TryGetValue(CorrelationIdHeader, out var correlationId) 
                && !string.IsNullOrWhiteSpace(correlationId))
            {
                return correlationId.ToString();
            }

            return Guid.NewGuid().ToString();
        }
    }

    /// <summary>
    /// Extension method to register the CorrelationIdMiddleware.
    /// </summary>
    public static class CorrelationIdMiddlewareExtensions
    {
        public static IApplicationBuilder UseCorrelationId(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CorrelationIdMiddleware>();
        }
    }
}
