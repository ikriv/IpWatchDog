using System.Diagnostics;

namespace IpWatchDog.Log
{
    internal class SystemLog : ILog
    {
        private readonly EventLog _eventLog;

        public SystemLog(string eventSourceName)
        {
            if (!EventLog.SourceExists(eventSourceName))
            {
                Trace.WriteLine("ERROR: coudl not find event log source '{0}'. Please run the installer again.", eventSourceName);
                return;
            }

            _eventLog = new EventLog { Source = eventSourceName };
        }

        public void Write(LogLevel level, string format, params object[] args)
        {
            if (_eventLog == null) return;
            var message = string.Format(format, args);
            _eventLog.WriteEntry(message, ToEntryType(level));
        }

        private static EventLogEntryType ToEntryType(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Error: return EventLogEntryType.Error;
                case LogLevel.Warning: return EventLogEntryType.Warning;
                default:
                    return EventLogEntryType.Information;
            }
        }
    }
}
