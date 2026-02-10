using Microsoft.Extensions.Logging;
using System.Text.Json;
using Capstone_dotnet.Logging.Models;

namespace Capstone_dotnet.Logging
{
    /// <summary>
    /// Production-ready logger service with structured JSON logging format.
    /// Includes timestamp, trace ID, HTTP context, and service metadata.
    /// </summary>
    public class LoggerService : ILoggerService
    {
        private readonly ILogger<LoggerService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public LoggerService(
            ILogger<LoggerService> logger, 
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <inheritdoc />
        public void LogInformation(string message, string traceId, string eventName)
        {
            var logEvent = CreateLogEvent(message, "Information", traceId, eventName, 200);
            LogStructured(LogLevel.Information, logEvent, null);
        }

        /// <inheritdoc />
        public void LogWarning(string message, string traceId, string eventName)
        {
            var logEvent = CreateLogEvent(message, "Warning", traceId, eventName, 0);
            LogStructured(LogLevel.Warning, logEvent, null);
        }

        /// <inheritdoc />
        public void LogError(Exception ex, string message, string traceId, string eventName)
        {
            var logEvent = CreateLogEvent(message, "Error", traceId, eventName, 400);
            LogStructured(LogLevel.Error, logEvent, ex);
        }

        /// <inheritdoc />
        public void LogCritical(Exception ex, string message, string traceId, string eventName)
        {
            var logEvent = CreateLogEvent(message, "Critical", traceId, eventName, 500);
            LogStructured(LogLevel.Critical, logEvent, ex);
        }

        /// <summary>
        /// Creates a structured log event with HTTP context and service metadata.
        /// </summary>
        private LogEvent CreateLogEvent(
            string message, 
            string level, 
            string traceId, 
            string eventName, 
            int statusCode)
        {
            var httpContext = _httpContextAccessor.HttpContext;

            return new LogEvent
            {
                Timestamp = DateTime.UtcNow.ToString("o"),
                TraceId = traceId,
                Message = message,
                Level = level,
                Method = httpContext?.Request.Method ?? "UNKNOWN",
                Endpoint = httpContext?.Request.Path ?? "UNKNOWN",
                StatusCode = statusCode,
                Service = new ServiceContext
                {
                    Name = _configuration["ServiceName"] ?? "PolicyManagement",
                    EventName = eventName
                }
            };
        }

        /// <summary>
        /// Logs the structured event as JSON.
        /// </summary>
        private void LogStructured(LogLevel level, LogEvent logEvent, Exception? exception)
        {
            var jsonOptions = new JsonSerializerOptions 
            { 
                WriteIndented = false,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            
            var logMessage = JsonSerializer.Serialize(logEvent, jsonOptions);
            
            if (exception != null)
            {
                _logger.Log(level, exception, "{LogMessage}", logMessage);
            }
            else
            {
                _logger.Log(level, "{LogMessage}", logMessage);
            }
        }
    }
}