namespace IpWatchDog.Log
{
    internal interface ILog
    {
        void Write(LogLevel level, string format, params object[] args);
    }
}
