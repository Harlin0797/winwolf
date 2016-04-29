using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using winwolf.util;
using System.IO;
using System.Xml;
using winwolf.events;
using winwolf.businuess;

namespace winwolf
{
    public class WebServer
    {
        TCPManager _tcpManager = new TCPManager();
        XmlHelper _xmlHelper = new XmlHelper();
        public WebServer()
        {
            _tcpManager.MsgChanged += new EventHandler<MsgChangedEventArgs>(_tcpManager_MsgChanged);
        }

        void _tcpManager_MsgChanged(object sender, MsgChangedEventArgs e)
        {
            this.DealData(e);
        }

        public void Start()
        {
            this.InitData();//初始化项目基础路径等配置信息

            IpHelper iptool = new IpHelper();
            String ip = iptool.GetIp();
            int port = GetPort();
            _tcpManager.Start(ip, port);
        }

        /// <summary>
        /// 处理数据
        /// </summary>
        /// <param name="content"></param>
        private void DealData(MsgChangedEventArgs e)
        {
            int len = e.Length;
            if (len <= 0)
            {
                e.Client.Closed = true;
                return;
            }
            else
            {
                string httpreq = Encoding.UTF8.GetString(e.Content);
                HandlerBusinuess hb=new HandlerBusinuess();
                List<string[]> reqArrs = hb.AnalyseHttpRequest(httpreq);
                if (reqArrs != null && reqArrs.Count > 0)
                {
                    hb.HandlerContent(e.Client,reqArrs,GetRootPath());
                    e.Client.buffer = new byte[0];
                    e.Client.tempbuffer = new byte[1024];
                    e.Client.length = 0;
                    e.Client.capacity = 0;
                }
            }
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        private void InitData()
        {
            HandlerBusinuess hb = new HandlerBusinuess();

            string rootPath = GetRootPath();
            XmlHelper xmlHelper = new XmlHelper();
            XmlNode node = xmlHelper.GetServerContext(rootPath + "\\conf\\server.xml");
            string docBase = node.Attributes["docBase"].Value;
            string file = node.Attributes["default"].Value;
            Init.Instance.DocBase = docBase;
        }

        /// <summary>
        /// 获取端口号
        /// </summary>
        /// <returns></returns>
        private int GetPort()
        {
            int port = 0;
            Judgement judgement=new Judgement();
            string rootPath = GetRootPath();
            string path = rootPath + "\\conf\\server.xml";
            XmlNode node = _xmlHelper.GetServerConnector(path);
            string portStr = node.Attributes["port"].Value;
            if (judgement.IsNumberic(portStr))
            {
                port = int.Parse(portStr);
            }
            return port;
        }

        /// <summary>
        /// 获取项目根目录
        /// </summary>
        /// <returns></returns>
        private string GetRootPath()
        {
            string rootPath = AppDomain.CurrentDomain.BaseDirectory;
            DirectoryInfo dir = new DirectoryInfo(rootPath);
            return dir.Parent.Parent.FullName;
        }
    }
}
