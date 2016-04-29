using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace winwolf.util
{
    public class Init
    {
        public override string ToString()
        {
            return "全局变量";
        }

        private static Init _instance = null;

        private string docBase = "/ROOT";//项目基础路径

        /// <summary>
        /// 项目基础路径
        /// </summary>
        public string DocBase
        {
            get { return this.docBase; }
            set { this.docBase = value; }
        }

        /// <summary>
        /// 全局变量单例
        /// </summary>
        public static Init Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Init();
                return _instance;
            }
        }
    }
}
