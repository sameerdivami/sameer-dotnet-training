namespace Capstone_dotnet.Logging
{
    /// <summary>
    /// Logger service interface that enforces structured logging standards
    /// with trace IDs and event names for comprehensive observability.
    /// </summary>
    public interface ILoggerService
    {
        /// <summary>
        /// Logs informational message with trace ID and event name.
        /// </summary>
        /// <param name="message">The log message</param>
        /// <param name="traceId">Unique trace ID for the request</param>
        /// <param name="eventName">Event name identifier (e.g., USER_LOGIN_SUCCESS)</param>
        void LogInformation(string message, string traceId, string eventName);

        /// <summary>
        /// Logs warning message with trace ID and event name.
        /// </summary>
        /// <param name="message">The log message</param>
        /// <param name="traceId">Unique trace ID for the request</param>
        /// <param name="eventName">Event name identifier (e.g., USER_NOT_FOUND)</param>
        void LogWarning(string message, string traceId, string eventName);

        /// <summary>
        /// Logs error with exception, trace ID and event name.
        /// </summary>
        /// <param name="ex">The exception to log</param>
        /// <param name="message">The log message</param>
        /// <param name="traceId">Unique trace ID for the request</param>
        /// <param name="eventName">Event name identifier (e.g., USER_REGISTRATION_FAILED)</param>
        void LogError(Exception ex, string message, string traceId, string eventName);

        /// <summary>
        /// Logs critical error with exception, trace ID and event name.
        /// </summary>
        /// <param name="ex">The exception to log</param>
        /// <param name="message">The log message</param>
        /// <param name="traceId">Unique trace ID for the request</param>
        /// <param name="eventName">Event name identifier (e.g., DATABASE_CONNECTION_FAILED)</param>
        void LogCritical(Exception ex, string message, string traceId, string eventName);
    }
}
