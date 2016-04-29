using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using winwolf.util;

namespace winwolf.businuess
{
    public class Files
    {
        private string fileExtension = Extension.HTML;//文件后缀名

        /// <summary>
        /// 文件后缀名
        /// </summary>
        public string FileExtension
        {
            get { return this.fileExtension; }
            set { this.fileExtension = value; }
        }

        /// <summary>
        /// 发送文件到浏览器
        /// </summary>
        /// <param name="client"></param>
        /// <param name="httpVersion"></param>
        /// <param name="mimeType"></param>
        /// <param name="realPath">文件实际路径</param>
        /// <param name="rootPath">项目根目录路径</param>
        public virtual void SendFileToBroswer(Client client, string httpVersion, string mimeType, string realPath, string rootPath)
        {
            SendTool st = new SendTool();
            st.SendFileToBroswer(client, httpVersion, mimeType, realPath);
        }
    }
}
