using System.Net;
using System.Text.Json;
using Capstone_dotnet.Logging;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace PolicyManagement.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ILoggerService loggerService)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await LogAndHandleExceptionAsync(context, ex, loggerService);
            }
        }

        private async Task LogAndHandleExceptionAsync(HttpContext context, Exception exception, ILoggerService loggerService)
        {
            // Extract stack trace details to find where the exception originated
            var stackTrace = new System.Diagnostics.StackTrace(exception, true);

            Console.WriteLine($"Stack Trace: {stackTrace}");
            var frame = stackTrace.GetFrame(0);
            
            var sourceFile = frame?.GetFileName() ?? "Unknown";
            var methodName = frame?.GetMethod()?.Name ?? "Unknown";
            var className = frame?.GetMethod()?.DeclaringType?.Name ?? "Unknown";
            var lineNumber = frame?.GetFileLineNumber() ?? 0;
            
            // Determine layer (Controller, Service, Repository)
            var layer = DetermineLayer(className, sourceFile);

            // Prepare log details
            var logDetails = new
            {
                TraceId = context.TraceIdentifier,
                Path = context.Request.Path.ToString(),
                Method = context.Request.Method,
                ExceptionType = exception.GetType().FullName,
                Message = exception.Message,
                Layer = layer,
                ClassName = className,
                MethodName = methodName,
                SourceFile = Path.GetFileName(sourceFile),
                LineNumber = lineNumber
            };

            // Determine status code and message based on exception type
            var (statusCode, message) = GetExceptionDetails(exception);

            // Log the exception with all details
            loggerService.LogError(
                exception,
                $"Technical exception occurred. Path: {logDetails.Path}, HttpMethod: {logDetails.Method}, Layer: {logDetails.Layer}, Class: {logDetails.ClassName}, Method: {logDetails.MethodName}, File: {logDetails.SourceFile}, Line: {logDetails.LineNumber}, ExceptionType: {logDetails.ExceptionType}, Message: {logDetails.Message}, StatusCode: {(int)statusCode}",
                logDetails.TraceId,
                "TECHNICAL_EXCEPTION"
            );

            // Return error response to client
            await WriteErrorResponseAsync(context, statusCode, message);
        }

        private static string DetermineLayer(string className, string sourceFile)
        {
            if (className.Contains("Controller") || sourceFile.Contains("Controllers"))
                return "Controller";
            if (className.Contains("Service") || sourceFile.Contains("Services"))
                return "Service";
            if (className.Contains("Repository") || className.Contains("Repo") || sourceFile.Contains("Repositories"))
                return "Repository";
            if (className.Contains("Middleware") || sourceFile.Contains("Middleware"))
                return "Middleware";
            return "Unknown";
        }

        private static (HttpStatusCode statusCode, string message) GetExceptionDetails(Exception exception)
        {
            return exception switch
            {
                DbUpdateException or NpgsqlException => 
                    (HttpStatusCode.InternalServerError, "A database error occurred. Please contact support."),
                
                TimeoutException => 
                    (HttpStatusCode.RequestTimeout, "The request timed out. Please contact support."),
                
                UnauthorizedAccessException => 
                    (HttpStatusCode.Unauthorized, "Unauthorized access."),
                
                InvalidOperationException ex when ex.Message.Contains("JWT") || ex.Message.Contains("authentication") => 
                    (HttpStatusCode.Unauthorized, "Authentication error occurred."),
                
                HttpRequestException => 
                    (HttpStatusCode.BadGateway, "External service communication failed."),
                
                IOException => 
                    (HttpStatusCode.InternalServerError, "File operation failed."),
                
                NullReferenceException or ArgumentNullException => 
                    (HttpStatusCode.InternalServerError, "A system error occurred. Please contact support."),
                
                KeyNotFoundException => 
                    (HttpStatusCode.NotFound, "The requested resource was not found."),
                
                _ => 
                    (HttpStatusCode.InternalServerError, "An unexpected error occurred. Please contact support.")
            };
        }

        private static async Task WriteErrorResponseAsync(HttpContext context, HttpStatusCode statusCode, string message)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var response = new
            {
                success = false,
                statusCode = (int)statusCode,
                message = message,
                traceId = context.TraceIdentifier,
                timestamp = DateTime.UtcNow
            };

            var result = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(result);
        }
    }
}