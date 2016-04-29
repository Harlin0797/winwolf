using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace winwolf
{
    public class Program
    {
        static void Main(string[] args)
        {
            WebServer webserver = new WebServer();
            webserver.Start();
            Console.WriteLine("信息：WinWolf started...");
            Console.ReadLine();
        }
    }
}
