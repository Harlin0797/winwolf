using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Net.Sockets;
using winwolf.events;

namespace winwolf.util
{
    public class TCPManager
    {
        public override string ToString()
        {
            return "TCP服务端通讯管理类";
        }

        public event EventHandler<MsgChangedEventArgs> MsgChanged;

        public TCPManager()
        {
            TCPSingleton.Instance.MsgChanged += new EventHandler<MsgChangedEventArgs>(Instance_MsgChanged);
        }

        /// <summary>
        /// 注册TCP单例消息改变事件回调函数
        /// </summary>
        /// <param name="sender">TCP单例实例</param>
        /// <param name="e">消息改变回传参数</param>
        void Instance_MsgChanged(object sender, MsgChangedEventArgs e)
        {
            EventHandler<MsgChangedEventArgs> handler = this.MsgChanged;
            if (handler != null)
            {
                handler(sender, e);
            }
        }

        /// <summary>
        /// 开启TCP连接，调用TCP单例中的Accept方法
        /// </summary>
        public void Start(string ip, int port)
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            TCPSingleton.Instance.Start(endPoint);
            TCPSingleton.Instance.Accept();
        }

        /// <summary>
        /// 给客户端发送消息
        /// </summary>
        /// <param name="client">对应的客户端</param>
        /// <param name="head">消息头</param>
        /// <param name="content">消息内容</param>
        public void SendMessage(Client client, byte[] content)
        {
            try
            {
                client.socket.Send(content);
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 析构函数
        /// </summary>
        ~TCPManager()
        {
            TCPSingleton.Instance.MsgChanged -= new EventHandler<MsgChangedEventArgs>(Instance_MsgChanged);
        }
    }
}
