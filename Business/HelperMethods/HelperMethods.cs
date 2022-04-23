using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Business.HelperMethods
{
    public static class HelperMethods
    {
        public static string IdKod()
        {
            var randomName = string.Format($"{DateTime.Now.Ticks}");
            return randomName;

        }
        public static string GetLocalIPAddress()
        {
            {
                IPHostEntry host;
                string localIP = "?";
                host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (IPAddress ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        localIP = ip.ToString();
                    }
                }
                return localIP;
            }
        }
    }
}
