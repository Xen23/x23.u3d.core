namespace XenTek.Core
{
    public interface ILoggable
    {
        void LogMessage(string message);
        void LogError(string error);
    }
}