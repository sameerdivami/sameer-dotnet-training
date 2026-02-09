using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace policyManagementApp.Filters
{
    public class ResponseTimeFilter : IAsyncActionFilter
    {
        private readonly ILogger<ResponseTimeFilter> _logger;

        public ResponseTimeFilter(ILogger<ResponseTimeFilter> logger)
        {
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var start = DateTime.UtcNow;
            var resultContext = await next();
            var end = DateTime.UtcNow;
            var elapsedMs = (end - start).TotalMilliseconds;

            _logger.LogInformation($"API {context.HttpContext.Request.Path} executed in {elapsedMs} ms");
            context.HttpContext.Response.Headers["X-Response-Time-ms"] = elapsedMs.ToString();
        }
    }
}
