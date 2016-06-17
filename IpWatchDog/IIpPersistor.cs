namespace IpWatchDog
{
    internal interface IIpPersistor : IIpRetriever
    {
        void SaveIp(string ip);
    }
}
