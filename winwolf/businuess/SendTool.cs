using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using winwolf.util;
using System.IO;
using System.Xml;

namespace winwolf.businuess
{
    public class SendTool
    {
        public override string ToString()
        {
            return "消息发送类";
        }

        /// <summary>
        /// 向浏览器发送头部信息
        /// </summary>
        /// <param name="client"></param>
        /// <param name="httpVersion"></param>
        /// <param name="mimeHeader"></param>
        /// <param name="len"></param>
        /// <param name="statusCode"></param>
        public void SendHeader(Client client, string httpVersion, string mimeHeader, int len, string statusCode)
        {
            TCPManager tcpManager = new TCPManager();
            string value = string.Empty;
            if (mimeHeader.Length == 0)
            {
                mimeHeader = "text/html";
            }
            value += httpVersion + " " + statusCode + "\r\n";
            value += "Server:winwolf\r\n";
            value += "Content-Type:" + mimeHeader + "\r\n";
            value += "Accept-Ranges:bytes\r\n";
            value += "Content-Length:" + len + "\r\n\r\n";
            byte[] buf = Encoding.UTF8.GetBytes(value);
            tcpManager.SendMessage(client, buf);
        }

        /// <summary>
        /// 将信息发送给浏览器
        /// </summary>
        /// <param name="client"></param>
        /// <param name="msg"></param>
        public void SendToBroswer(Client client, string msg)
        {
            byte[] buf = Encoding.UTF8.GetBytes(msg);
            SendToBroswer(client, buf);
        }

        /// <summary>
        /// 将信息发送给浏览器
        /// </summary>
        /// <param name="client"></param>
        /// <param name="buf"></param>
        public void SendToBroswer(Client client, byte[] buf)
        {
            TCPManager tcpManager = new TCPManager();
            tcpManager.SendMessage(client, buf);
        }

        /// <summary>
        /// 发送文件到浏览器
        /// </summary>
        /// <param name="client"></param>
        /// <param name="realPath"></param>
        public void SendFileToBroswer(Client client, string httpVersion, string mimeType, string realPath)
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(realPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                byte[] buf = new byte[fs.Length];
                fs.Read(buf, 0, (int)fs.Length);

                SendHeader(client, httpVersion, mimeType, (int)fs.Length, "200");
                SendToBroswer(client, buf);
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
        }

        /// <summary>
        /// 发送项目起始界面
        /// </summary>
        public void SendProjectStartFile(Client client, string httpVersion, string realPath, string rootPath)
        {
            XmlHelper xmlHelper = new XmlHelper();
            XmlNode node = xmlHelper.GetServerContext(rootPath + "\\conf\\server.xml");
            string docBase = node.Attributes["docBase"].Value;
            string file = node.Attributes["default"].Value;

            string path = rootPath + "\\webapps";

            string temp = rootPath + "\\webapps" + Init.Instance.DocBase;
            DirectoryInfo dirInfo1 = new DirectoryInfo(temp);
            DirectoryInfo dirInfo2 = new DirectoryInfo(realPath);
            if (dirInfo1.Exists && dirInfo2.Exists)
            {
                if (dirInfo1.FullName == dirInfo2.FullName || dirInfo1.Parent.FullName == dirInfo2.FullName)
                {
                    path += docBase;
                }
                else
                {
                    path += "\\ROOT";
                }
            }
            else
            {
                path += "\\ROOT";
            }
            if (!string.IsNullOrEmpty(file))
            {
                path += "\\" + file;
            }
            else
            {
                path += "\\index.html";
            }
            FileManager fileManager = new FileManager();
            fileManager.SendFileToBroswer(client, httpVersion, "text/html", path, rootPath);
        }
    }
}
