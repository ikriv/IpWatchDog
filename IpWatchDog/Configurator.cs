using IpWatchDog.Log;

namespace IpWatchDog
{
    class Configurator
    {
        private ILog _log;

        public Configurator(ILog log)
        {
            _log = log;
        }

        public IService CreateWatchDogService()
        {
            var config = new AppConfig();

            return new IpWatchDogService(
                _log, 
                config,
                new IpPersistor(_log), 
                new WebIpRetriever(_log), 
                new MailIpNotifier(_log, config));
        }
    }
}
