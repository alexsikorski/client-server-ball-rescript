using System;
using System.Net;
using System.Net.Sockets;

namespace BallGameClient.Utilities
{
    public class Constants
    {
        public static string GetLocalIpAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName()); // list of address
            foreach (var address in host.AddressList)
            {
                if (address.AddressFamily == AddressFamily.InterNetwork)
                {
                    return address.ToString(); // return ip as string
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        public const int PortNumber = 8302;
        // no delay as client does not utilize it
        public static readonly IPAddress IpAddress = IPAddress.Parse(GetLocalIpAddress());
    }
}