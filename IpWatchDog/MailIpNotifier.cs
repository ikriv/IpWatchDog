using System;
using System.Net;
using System.Net.Mail;
using IpWatchDog.Log;

namespace IpWatchDog
{
    internal class MailIpNotifier : IIpNotifier
    {
        private readonly ILog _log;
        private readonly AppConfig _config;

        public MailIpNotifier(ILog log, AppConfig config)
        {
            _log = log;
            _config = config;
        }

        public void OnIpChanged(string oldIp, string newIp)
        {
            var msg = GetMessage(oldIp, newIp);
            _log.Write(LogLevel.Warning, msg);

            try
            {
                var smtpClient = new SmtpClient(_config.SmtpHost, _config.SmtpPort)
                {
                    EnableSsl = _config.SmtpUseSsl,
                    DeliveryMethod = SmtpDeliveryMethod.Network
                };

                if (_config.SmtpUserName != string.Empty)
                {
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential(_config.SmtpUserName, _config.SmtpPassword);
                }

                smtpClient.Send(
                    _config.MailFrom,
                    _config.MailTo,
                    _config.Subject,
                    msg);
            }
            catch (Exception ex)
            {
                _log.Write(LogLevel.Error, "Error sending e-mail. {0}", ex);
            }
        }

        private static string GetMessage(string oldIp, string newIp)
        {
            return $"IP changed from {oldIp} to {newIp}";
        }
    }
}
