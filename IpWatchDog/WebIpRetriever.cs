using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using IpWatchDog.Log;

namespace IpWatchDog
{
    internal class WebIpRetriever : IIpRetriever
    {
        private readonly AppConfig _config;
        private readonly ILog _log;

        public WebIpRetriever(ILog log, AppConfig config)
        {
            _log = log;
            _config = config;
        }

        public string GetIp()
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(_config.IPChecker);
                request.Method = "GET";

                using (var response = request.GetResponseNoException())
                {
                    var webStatus = (int)response.StatusCode;
                    if (!(webStatus >= 200 && webStatus < 300))
                    {
                        _log.Write(LogLevel.Warning, "Could not retrieve current IP from web. Response code: {0}", webStatus);
                        response.Close();
                        return null;
                    }
                    var responseStream = response.GetResponseStream();
                    if (responseStream == null)
                    {
                        _log.Write(LogLevel.Warning, "Could not retrieve current IP from web. Response is empty.");
                        response.Close();
                        return null;
                    }
                    using (var reader = new StreamReader(responseStream))
                    {
                        var answer = string.Empty;
                        var i = 0;
                        while (reader.Peek() >= 0 && i++ < 4096)
                        {
                            var c = new char[4096];
                            reader.Read(c, 0, c.Length);
                            answer = answer + new string(c);
                        }
                        return ExtractIp(answer);
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Write(LogLevel.Warning, "Could not retrieve current IP from web. {0}", ex);
                return null;
            }
        }

        private string ExtractIp(string answer)
        {
            var regex = new Regex(_config.IPCheckerRegExp, RegexOptions.Compiled, new TimeSpan(0,0,5));
            var r = regex.Match(answer);
            return r.Success ? r.Value : null;
        }
    }
}
