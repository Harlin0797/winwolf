using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using winwolf.util;
using System.IO;

namespace winwolf.businuess
{
    public class FileManager
    {
        public override string ToString()
        {
            return "文件分析类";
        }

        private List<Files> _fileList = new List<Files>();

        public FileManager()
        {
            Files file = new HtmlFile();
            file.FileExtension = Extension.HTML;
            _fileList.Add(file);

            file = new OtherFile();
            file.FileExtension = Extension.OTHER;
            _fileList.Add(file);
        }

        /// <summary>
        /// 发送文件到浏览器
        /// 根据后缀名查询分析文件类型，后缀名不在库里的按一般文件处理，直接发送到客户端
        /// </summary>
        /// <param name="client"></param>
        /// <param name="httpVersion"></param>
        /// <param name="mimeType"></param>
        /// <param name="realPath"></param>
        public void SendFileToBroswer(Client client, string httpVersion, string mimeType, string realPath, string rootPath)
        {
            try
            {
                FileInfo fileInfo = new FileInfo(realPath);
                if (!fileInfo.Exists) return;
                Files file = _fileList.Find((Files f) => { return f.FileExtension == fileInfo.Extension; });
                if (file == null)
                {
                    file = _fileList.Find((Files f) => { return f.FileExtension == Extension.OTHER; });
                }
                file.SendFileToBroswer(client, httpVersion, mimeType, realPath, rootPath);
            }
            catch (Exception ex)
            {
 
            }
        }
    }
}
