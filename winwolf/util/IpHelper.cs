using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace winwolf.util
{
    public class IpHelper
    {
        public override string ToString()
        {
            return "IP工具类";
        }

        /// <summary>
        /// 获取IPV4地址
        /// </summary>
        /// <returns></returns>
        public string GetIp()
        {
            string result = "127.0.0.1";
            IPHostEntry localhost = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress[] ips = localhost.AddressList;
            foreach (IPAddress ip in ips)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    result = ip.ToString();
                    break;
                }
            }
            return result;
        }
    }
}
