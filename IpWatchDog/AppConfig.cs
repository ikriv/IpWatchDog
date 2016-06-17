using System.Configuration;

namespace IpWatchDog
{
    class AppConfig
    {
        public int PollingTimeoutSeconds => int.Parse(Config("PollingTimeoutSeconds"));

        public string MailFrom => Config("MailFrom");

        public string MailTo => Config("MailTo");

        public string SmtpHost => Config("SmtpHost");

        public string Subject => Config("Subject");

        public string Command => Config("Command");

        public string IPChecker => Config("IPChecker");

        public string IPCheckerRegExp => Config("IPCheckerRegExp");

        private static string Config(string arg)
        {
            return ConfigurationManager.AppSettings[arg];
        }
    }
}
