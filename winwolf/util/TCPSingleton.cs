using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using winwolf.events;

namespace winwolf.util
{
    public class TCPSingleton
    {
        public override string ToString()
        {
            return "TCP单例";
        }

        private static TCPSingleton _instance = null;

        public IPEndPoint ipEndPoint { get; set; }
        public Socket _socket = null;
        public event EventHandler<MsgChangedEventArgs> MsgChanged;

        private TCPSingleton()
        {
            
        }

        /// <summary>
        /// TCP单例
        /// </summary>
        public static TCPSingleton Instance
        {
            get 
            {
                if (_instance == null)
                {
                    _instance = new TCPSingleton();
                }
                return _instance;
            }
        }

        public void Start(IPEndPoint endPoint)
        {
            if (_socket == null)
            {
                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _socket.Bind(endPoint);
                _socket.Listen(200);
            }
        }

        public void Accept()
        {
            _socket.BeginAccept(new AsyncCallback(Accept_Callback), _socket);
        }

        private void Accept_Callback(IAsyncResult iar)
        {
            Socket socket = (Socket)iar.AsyncState;
            Client client = new Client();
            client.socket = socket.EndAccept(iar);
            client.socket.BeginReceive(client.tempbuffer, 0, 1024, SocketFlags.None, new AsyncCallback(Receive_Callback), client);
            _socket.BeginAccept(new AsyncCallback(Accept_Callback), _socket);
        }

        /// <summary>
        /// 接收数据回调函数
        /// </summary>
        /// <param name="iar"></param>
        private void Receive_Callback(IAsyncResult iar)
        {
            Client client = (Client)iar.AsyncState;
            try
            {
                int len = client.socket.EndReceive(iar);
                ReceiveData(client,len);
                if (!client.Closed)
                {
                    client.socket.BeginReceive(client.tempbuffer, 0, 1024, SocketFlags.None, new AsyncCallback(Receive_Callback), client);
                }
                else
                {
                    client.socket.Close();
                }
            }
            catch (Exception ex)
            {
                client.socket.Close();
            }
        }

        /// <summary>
        /// 接收数据
        /// </summary>
        /// <param name="client"></param>
        /// <param name="len"></param>
        private void ReceiveData(Client client, int len)
        {
            client.Write(client.tempbuffer, 0, len);
            EventHandler<MsgChangedEventArgs> handler = this.MsgChanged;
            if (handler != null)
            {
                handler(this, new MsgChangedEventArgs(client.buffer, len, client));
            }
        }
    }
}
