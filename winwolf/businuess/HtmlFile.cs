using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using winwolf.util;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;

namespace winwolf.businuess
{
    public class HtmlFile:Files
    {
        /// <summary>
        /// 将Html文件发送到浏览器
        /// </summary>
        /// <param name="client"></param>
        /// <param name="httpVersion"></param>
        /// <param name="mimeType"></param>
        /// <param name="realPath">文件实际路径</param>
        /// /// <param name="rootPath">项目根目录路径</param>
        public override void SendFileToBroswer(Client client, string httpVersion, string mimeType, string realPath, string rootPath)
        {
            AnalyseHtmlFile(client, httpVersion, mimeType, realPath, rootPath);
        }

        /// <summary>
        /// 分析html文件
        /// 把html中相关联的文件都发给浏览器
        /// </summary>
        /// <param name="client"></param>
        /// <param name="httpVersion"></param>
        /// <param name="mimeType"></param>
        /// <param name="realPath">文件实际路径</param>
        /// <param name="rootPath">项目根目录路径</param>
        public void AnalyseHtmlFile(Client client, string httpVersion, string mimeType, string realPath, string rootPath)
        {
            List<FileItem> fileItemList = GetHtmlExtralFiles(realPath, rootPath);

            SendTool st = new SendTool();
            //先把html文件发送到客户端
            st.SendFileToBroswer(client, httpVersion, mimeType, realPath);
            //Thread.Sleep(50);

            ////把html相关文档发送到客户端
            //if (fileItemList != null && fileItemList.Count > 0)
            //{
            //    foreach (FileItem fileItem in fileItemList)
            //    {
            //        st.SendFileToBroswer(client, httpVersion, fileItem.MimeType, fileItem.Path);
            //        Thread.Sleep(50);
            //    }
            //}
        }

        /// <summary>
        /// 获取html关联文件
        /// </summary>
        /// <param name="realPath"></param>
        /// <returns></returns>
        public List<FileItem> GetHtmlExtralFiles(string realPath,string rootPath)
        {
            List<FileItem> fileItemList = new List<FileItem>();
            FileStream fs = null;
            StreamReader sr = null;
            try
            {
                fs = new FileStream(realPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                sr = new StreamReader(fs);
                string html = sr.ReadToEnd();
                string imgPattern = "[<][i][m][g].+[/][>]";
                string jsPattern = "[<][s][c][r][i][p][t].+[/>]";
                string cssPattern = "[<][l][i][n][k].+[/>]";
                List<FileItem> imgList = GetExtraFileItems(html, imgPattern, "src", realPath, rootPath);
                List<FileItem> jsList = GetExtraFileItems(html, jsPattern, "src", realPath, rootPath);
                List<FileItem> cssList = GetExtraFileItems(html, cssPattern, "href", realPath, rootPath);
                if (imgList != null && imgList.Count > 0)
                {
                    fileItemList.AddRange(imgList);
                }
                if (jsList != null && jsList.Count > 0)
                {
                    fileItemList.AddRange(jsList);
                }
                if (cssList != null && cssList.Count > 0)
                {
                    fileItemList.AddRange(cssList);
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (sr != null)
                    sr.Close();
                if (fs != null)
                    fs.Close();
            }
            return fileItemList;
        }

        /// <summary>
        /// 获取html中的关联图片
        /// </summary>
        /// <param name="realPath"></param>
        /// <param name="rootPath"></param>
        /// <returns></returns>
        public List<FileItem> GetExtraFileItems(string html,string pattern,string srcPattern, string realPath, string rootPath)
        {
            List<FileItem> imgList = new List<FileItem>();
            Common cn = new Common();

            Regex regex = new Regex(pattern);
            MatchCollection matchs = regex.Matches(html);
            foreach (Match match in matchs)
            {
                string img = match.Value;
                string[] imgArr = img.Split(' ');
                if (imgArr != null && imgArr.Length > 0)
                {
                    string src = string.Empty;
                    foreach (string str in imgArr)
                    {
                        if (str.ToUpper().Contains(srcPattern.ToUpper()))
                        {
                            src = str.Replace("/>", " ").Trim();
                            break;
                        }
                    }
                    string[] srcArr = src.Split('=');
                    if (srcArr != null && srcArr.Length > 0)
                    {
                        string value = srcArr[1].Replace('\"', ' ').Trim();
                        string str1 = realPath.Replace(rootPath + "\\webapps", "");
                        string app = str1.Substring(0, str1.IndexOf('\\'));

                        string imgPath = rootPath + "\\webapps" + app + "\\" + value;
                        FileItem fileItem = new FileItem();
                        fileItem.Path = imgPath;
                        fileItem.MimeType = cn.GetMimeType(imgPath, rootPath);
                        imgList.Add(fileItem);
                    }
                }
            }
            return imgList;
        }

        /// <summary>
        /// 获取html中的关联图片
        /// </summary>
        /// <param name="realPath"></param>
        /// <param name="rootPath"></param>
        /// <returns></returns>
        public List<FileItem> GetExtraImages(string html,string realPath,string rootPath)
        {
            List<FileItem> imgList = new List<FileItem>();
            Common cn = new Common();

            Regex regex = new Regex("[<][i][m][g].+[/][>]");
            MatchCollection matchs = regex.Matches(html);
            foreach (Match match in matchs)
            {
                string img = match.Value;
                string[] imgArr = img.Split(' ');
                if (imgArr != null && imgArr.Length > 0)
                {
                    string src = string.Empty;
                    foreach (string str in imgArr)
                    {
                        if (str.ToUpper().Contains("SRC"))
                        {
                            src = str;
                            break;
                        }
                    }
                    string[] srcArr = src.Split('=');
                    if (srcArr != null && srcArr.Length > 0)
                    {
                        string value = srcArr[1].Replace('\"', ' ').Trim();
                        string str1 = realPath.Replace(rootPath + "\\webapps", "");
                        string app = str1.Substring(0, str1.IndexOf('\\'));

                        string imgPath = rootPath + "\\webapps" + app + "\\" + value;
                        FileItem fileItem = new FileItem();
                        fileItem.Path = imgPath;
                        fileItem.MimeType = cn.GetMimeType(imgPath, rootPath);
                        imgList.Add(fileItem);
                    }
                }
            }
            return imgList;
        }
    }
}
