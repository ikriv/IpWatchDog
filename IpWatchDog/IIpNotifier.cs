namespace IpWatchDog
{
    internal interface IIpNotifier
    {
        void OnIpChanged(string oldIp, string newIp);
    }
}
