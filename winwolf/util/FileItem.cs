using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace winwolf.util
{
    public class FileItem
    {
        public override string ToString()
        {
            return "相关文件信息";
        }

        private string path;//文件路径
        private string mimeType;//文件对应的MIME类型

        /// <summary>
        /// 文件路径
        /// </summary>
        public string Path
        {
            get { return this.path; }
            set { this.path = value; }
        }

        /// <summary>
        /// 文件对应的MIME类型
        /// </summary>
        public string MimeType
        {
            get { return this.mimeType; }
            set { this.mimeType = value; }
        }
    }
}
