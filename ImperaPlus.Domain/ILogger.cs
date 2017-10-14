namespace ImperaPlus.Domain
{
    public enum LogLevel
    {
        Info,
        Error
    }

    public interface ILogger
    {
        void Log(LogLevel level, string format, params object[] args);
    }
}
