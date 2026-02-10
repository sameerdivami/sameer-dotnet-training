namespace Capstone_dotnet.Logging.Models
{
    /// <summary>
    /// Structured log event model that defines the exact JSON structure for all logs.
    /// </summary>
    public class LogEvent
    {
        public string Timestamp { get; set; } = string.Empty;
        public string TraceId { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Level { get; set; } = string.Empty;
        public string Method { get; set; } = string.Empty;
        public string Endpoint { get; set; } = string.Empty;
        public int StatusCode { get; set; }
        public ServiceContext Service { get; set; } = new ServiceContext();
        public ExceptionSource? ExceptionSource { get; set; }
    }

    /// <summary>
    /// Service context information included in every log event.
    /// </summary>
    public class ServiceContext
    {
        public string Name { get; set; } = string.Empty;
        public string EventName { get; set; } = string.Empty;
    }

    /// <summary>
    /// Exception source information including file, class, method, and line number.
    /// </summary>
    public class ExceptionSource
    {
        public string Layer { get; set; } = string.Empty;
        public string ClassName { get; set; } = string.Empty;
        public string MethodName { get; set; } = string.Empty;
        public string SourceFile { get; set; } = string.Empty;
        public int LineNumber { get; set; }
        public string ExceptionType { get; set; } = string.Empty;
    }
}