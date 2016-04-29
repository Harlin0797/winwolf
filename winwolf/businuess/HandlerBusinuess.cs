using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using winwolf.util;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml;

namespace winwolf.businuess
{
    public class HandlerBusinuess
    {
        public override string ToString()
        {
            return "业务逻辑类";
        }

        /// <summary>
        /// 解析Http请求
        /// </summary>
        /// <param name="req"></param>
        public List<string[]> AnalyseHttpRequest(string req)
        {
            Console.WriteLine("==============HttpRequest===============\r\n"+req);
            List<string[]> methodList = new List<string[]>();
            Regex regex = new Regex("[G][E][T].+[H][T][T][P][/][0-9][.][0-9]");
            MatchCollection matchs = regex.Matches(req);
            if (matchs != null && matchs.Count > 0)
            {
                foreach (Match match in matchs)
                {
                    string[] strArr = Regex.Split(match.Value, @"\r\n");
                    if (strArr != null && strArr.Length > 0)
                    {
                        string head = strArr[0];
                        string[] valueArr = head.Split(' ');
                        methodList.Add(valueArr);
                    }
                }
            }
            return methodList;
        }

        /// <summary>
        /// 服务端处理内容
        /// </summary>
        /// <param name="reqArr"></param>
        public void HandlerContent(Client client,List<string[]> reqArrs,string rootPath)
        {
            if (reqArrs != null && reqArrs.Count > 0)
            {
                foreach (string[] reqArr in reqArrs)
                {
                    HandlerContent(client, reqArr, rootPath);
                }
            }
        }

        public void HandlerContent(Client client, string[] reqArr, string rootPath)
        {
            Common cn = new Common();
            string method = reqArr[0];
            string path = reqArr[1];
            string httpVersion = reqArr[2];
            string mimetype = cn.GetMimeType(path, rootPath);
            string realPath = GetRealPath(path, rootPath);

            string errorMsg = "<H2>请求的路径不存在</H2><Br>";
            SendTool st = new SendTool();
            if (!IsProjectExits(path, rootPath))//项目不存在
            {
                st.SendHeader(client, httpVersion, "", errorMsg.Length, "404");
                st.SendToBroswer(client, errorMsg);
            }
            else
            {
                if (IsFile(realPath))//如果是文件
                {
                    if (!IsFileExits(realPath))//文件不存在
                    {
                        errorMsg = "<H2>请求的文件不存在</H2><Br>";
                        st.SendHeader(client, httpVersion, "", errorMsg.Length, "404");
                        st.SendToBroswer(client, errorMsg);
                    }
                    else//文件存在则发送文件
                    {
                        FileManager fileManager = new FileManager();
                        fileManager.SendFileToBroswer(client, httpVersion, mimetype, realPath, rootPath);
                    }
                }
                else//如果不是文件，则显示部署项目下的起始界面
                {
                    st.SendProjectStartFile(client, httpVersion, realPath, rootPath);
                }
            }
        }

        /// <summary>
        /// 项目是否存在
        /// </summary>
        /// <param name="path"></param>
        /// <param name="rootPath"></param>
        /// <returns></returns>
        private bool IsProjectExits(string path,string rootPath)
        {
            bool ret = false;
            string ppath=path;
            if (path.Contains('.'))
            {
                string temp = path.Substring(1, path.Length - 1);
                ppath = temp.Substring(0, temp.IndexOf('/'));
            }
            string dirPath = rootPath + "\\webapps\\" + ppath;
            if (path.Trim().Length == 1)
            {
                dirPath = rootPath + "\\webapps\\ROOT";
            }

            string dirPath2 = rootPath + "\\webapps" + Init.Instance.DocBase + "\\" + ppath;
            //仅输入IP与端口号或者URL后面带有项目项目名称的情况
            DirectoryInfo dirInfo = new DirectoryInfo(dirPath);
            //使用DocBase配置项，URL上不包含项目名的情况
            DirectoryInfo dirInfo2 = new DirectoryInfo(dirPath2);
            if (dirInfo.Exists || dirInfo2.Exists)
            {
                ret = true;
            }
            return ret;
        }

        /// <summary>
        /// 文件是否存在
        /// </summary>
        /// <param name="realPath"></param>
        /// <returns></returns>
        private bool IsFileExits(string realPath)
        {
            bool ret = false;
            FileInfo fileInfo = new FileInfo(realPath);
            if (fileInfo.Exists)
            {
                ret = true;
            }
            return ret;
        }

        /// <summary>
        /// 请求的是否是文件
        /// 包含后缀的即为文件
        /// </summary>
        /// <param name="realPath"></param>
        /// <returns></returns>
        private bool IsFile(string realPath)
        {
            bool ret = false;
            if (realPath.Contains('.'))
            {
                ret = true;
            }
            return ret;
        }

        /// <summary>
        /// 获取文件实际路劲
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private string GetRealPath(string path,string rootPath)
        {
            string ret = string.Empty;
            ret = rootPath + "\\webapps";
            string ppath = string.Empty;

            if (path.Length > 1)
            {
                string temp = path.Substring(1, path.Length - 1);
                ppath = temp.Substring(0, temp.IndexOf('/'));
                string temp1 = ret + "\\" + ppath;
                try
                {
                    DirectoryInfo dir = new DirectoryInfo(temp1);
                    if (!dir.Exists)
                    {
                        ret += Init.Instance.DocBase;
                    }
                }
                catch (Exception ex)
                {

                }
            }
            else
            {
                ret += Init.Instance.DocBase;
            }
            if (path.Trim().Length > 1)
            {
                ret += path.Replace('/', '\\');
            }
            return ret;
        }
        
    }
}
