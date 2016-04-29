using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace winwolf.util
{
    public class XmlHelper
    {
        public override string ToString()
        {
            return "XML工具类";
        }

        /// <summary>
        /// 获取Mime类型
        /// </summary>
        /// <returns></returns>
        public List<string> GetMimes(string path)
        {
            List<string> mimeList = new List<string>();
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            XmlElement root = doc.DocumentElement;//根节点
            XmlNodeList nodes = root.GetElementsByTagName("value");
            if (nodes != null)
            {
                foreach (XmlNode node in nodes)
                {
                    mimeList.Add(node.InnerText.Trim());
                }
            }
            return mimeList;
        }

        /// <summary>
        /// 获取服务器Connector信息
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public XmlNode GetServerConnector(string path)
        {
            XmlNode node = null;
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            XmlElement root = doc.DocumentElement;
            XmlNodeList nodes = root.GetElementsByTagName("Connector");
            if (nodes != null && nodes.Count > 0)
            {
                node = nodes[0];
            }
            return node;
        }

        /// <summary>
        /// 获取服务器Context信息
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public XmlNode GetServerContext(string path)
        {
            XmlNode node = null;
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            XmlElement root = doc.DocumentElement;
            XmlNodeList nodes = root.GetElementsByTagName("Context");
            if (nodes != null && nodes.Count > 0)
            {
                node = nodes[0];
            }
            return node;
        }
    }
}
