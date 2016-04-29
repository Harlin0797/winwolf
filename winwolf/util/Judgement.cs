using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace winwolf.util
{
    public class Judgement
    {
        public override string ToString()
        {
            return "判断字符串是否为数字等";
        }

        /// <summary>
        /// 判断字符串是否为整型
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public bool IsNumberic(string str)
        {
            Regex regex = new Regex(@"^\d+$");
            if (regex.IsMatch(str))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 判断字符串是否为数字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public bool IsNumberic2(string str)
        {
            Regex regex = new Regex(@"^-?\d+$");
            Regex regex2 = new Regex(@"^(-?\d+)(\.\d+)?$");
            if (regex.IsMatch(str) || regex2.IsMatch(str))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
