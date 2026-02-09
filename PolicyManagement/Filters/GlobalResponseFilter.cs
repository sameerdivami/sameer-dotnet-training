namespace policyManagementApp.Filters
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using System.Net;

    public class GlobalResponseFilter : IAsyncResultFilter
    {
       public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
             var result = context.Result;
             if(result != null)
            {
                int statusCode = (int)HttpStatusCode.OK;
                
                // Get the actual status code from the result
                if (result is ObjectResult objectResult && objectResult.StatusCode.HasValue)
                {
                    statusCode = objectResult.StatusCode.Value;
                }
                else if (result is StatusCodeResult statusCodeResult)
                {
                    statusCode = statusCodeResult.StatusCode;
                }

                var response = new
                {
                    success = statusCode >= 200 && statusCode < 300,
                    StatusCode = statusCode,
                    Message = statusCode >= 200 && statusCode < 300 ? "Request processed successfully" : "Request failed",
                    Data = result is ObjectResult objResult ? objResult.Value : null,
                    traceId = context.HttpContext.TraceIdentifier
                };
                context.Result = new JsonResult(response)
                {
                    StatusCode = statusCode
                };
            }
            await next();
        }
    }
}