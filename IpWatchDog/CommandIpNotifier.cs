using System;
using System.Diagnostics;
using IpWatchDog.Log;
using System.IO;

namespace IpWatchDog
{
    internal class CommandIpNotifier : IIpNotifier
    {
        private readonly string _command;
        private readonly ILog _log;

        public CommandIpNotifier(ILog log, AppConfig config)
        {
            _log = log;
            _command = config.Command.Trim();
            if (string.IsNullOrEmpty(_command))
            {
                throw new ArgumentException("Command cannot be null or empty");
            }
        }

        public void OnIpChanged(string oldIp, string newIp)
        {
            string command = _command.Replace("${old}", oldIp).Replace("${new}", newIp);
            var sysDir = Environment.GetFolderPath(Environment.SpecialFolder.System);
            var cmdLocation = Path.Combine(sysDir, "cmd.exe");

            var startInfo = new ProcessStartInfo { FileName = cmdLocation, Arguments = "/c " + command, CreateNoWindow = true, UseShellExecute=false };

            try
            {
                _log.Write(LogLevel.Warning, "IP changed from {0} to {1}. Executing command: '{2}'", oldIp, newIp, command);
                Process.Start(startInfo);
            }
            catch (Exception ex)
            {
                _log.Write(LogLevel.Warning, "An error occured while executing command\r\n'{0}'\r\n {1}", command, ex);
            }
        }
    }
}
