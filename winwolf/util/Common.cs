using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace winwolf.util
{
    public class Common
    {
        /// <summary>
        /// 获取MIME类型
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string GetMimeType(string path, string rootPath)
        {
            string type = string.Empty;
            if (path.Contains("."))
            {
                int pos = path.LastIndexOf('.');
                string suffex = path.Substring(pos);

                XmlHelper xmlHelper = new XmlHelper();
                string mimePath = rootPath + "\\conf\\Mimes.xml";
                List<string> mimeList = xmlHelper.GetMimes(mimePath);
                foreach (string str in mimeList)
                {
                    string[] strArr = str.Split(';');
                    if (strArr == null || strArr.Length == 0) continue;
                    if (suffex == strArr[0])
                    {
                        type = strArr[1];
                        break;
                    }
                }
            }
            return type;
        }
    }
}
