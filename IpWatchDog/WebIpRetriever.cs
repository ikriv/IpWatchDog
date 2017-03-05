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
                var request = (HttpWebRequest)WebRequest.Create(_config.IpCheckerUrl);
                request.Method = "GET";

                using (var response = request.GetResponseNoException())
                {
                    var webStatus = (int)response.StatusCode;
                    if (!IsSuccess(webStatus))
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
                        var buffer = new char[_config.MaxHttpResponseLength];
                        reader.Read(buffer, 0, buffer.Length);
                        var answer = new string(buffer);
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

        private static bool IsSuccess(int status)
        {
            return status >= 200 && status <= 299;
        }

        private string ExtractIp(string answer)
        {
            var regex = new Regex(_config.IpCheckerRegEx, RegexOptions.Compiled);
            
            var match = regex.Match(answer);
            if (!match.Success) return null;

            return match.Groups["ip"].Value;
        }
    }
}
