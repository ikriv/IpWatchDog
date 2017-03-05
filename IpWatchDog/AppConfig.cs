using System;
using System.Configuration;

namespace IpWatchDog
{
    class AppConfig
    {
        public int PollingTimeoutSeconds => int.Parse(Config("PollingTimeoutSeconds"));

        public string MailFrom => Config("MailFrom");

        public string MailTo => Config("MailTo");

        public string SmtpHost => Config("SmtpHost");

        public int SmtpPort => int.Parse(Config("SmtpPort"));

        public string SmtpUserName => Config("SmtpUserName");

        public string SmtpPassword => Config("SmtpPassword");

        public bool SmtpUseSsl => bool.Parse(Config("SmtpUseSsl"));

        public string Subject => Config("Subject");

        public string Command => Config("Command");

        public string IpCheckerUrl => Config("IpCheckerUrl");

        public string IpCheckerRegEx => Config("IpCheckerRegEx");

        public int MaxHttpResponseLength
            => Math.Min(100*1024*1024, Math.Max(1024, int.Parse(Config("MaxHttpResponseLength"))));

        private static string Config(string arg)
        {
            return ConfigurationManager.AppSettings[arg];
        }
    }
}
