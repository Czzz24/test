using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EF.Component.Tools
{
    /// <summary>
    /// StringHelper
    /// </summary>
    public static class StringHelper
    {
        /// <summary>
        /// 截取字符串，保留其HTML格式
        /// </summary>
        /// <param name="strText"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string CutWithSubstring(string strText, int len)
        {
            if (strText.Length > len)
            {
                return strText.Substring(0, len) + "…";
            }
            else
            {
                return strText;
            }
        }

        /// <summary>
        /// 截取字符串去掉其HTML字符
        /// </summary>
        /// <param name="inputString"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string CutWithOutHtml(string inputString, int len)
        {
            int tempLen = 0;
            string tempString = "";
            inputString = LoseHtml(inputString);
            ASCIIEncoding ascii = new ASCIIEncoding();

            byte[] s = ascii.GetBytes(inputString);
            for (int i = 0; i < s.Length; i++)
            {
                if ((int)s[i] == 63)
                {
                    tempLen += 2;
                }
                else
                {
                    tempLen += 1;
                }

                try
                {
                    tempString += inputString.Substring(i, 1);
                }
                catch
                {
                    break;
                }

                if (tempLen > len)
                    break;
            }
            //如果截过则加上半个省略号
            byte[] mybyte = System.Text.Encoding.Default.GetBytes(inputString);
            if (mybyte.Length > len)
            {
                tempString = tempString.Substring(0, tempString.Length - 3 == 0 ? 1 : tempString.Length - 3) + "…";
            }
            return tempString;
        }

        /// <summary>
        /// 去掉html标签
        /// </summary>
        /// <param name="ContentStr"></param>
        /// <returns></returns>
        private static string LoseHtml(string ContentStr)
        {
            string ClsTempLoseStr = "";
            System.Text.RegularExpressions.Regex regEx = new System.Text.RegularExpressions.Regex("<\\/*[^<>]*>");
            ClsTempLoseStr = regEx.Replace(ContentStr, "");
            return ClsTempLoseStr;
        }

        /// <summary>
        /// 裁切字符串（中文按照两个字符计算）
        /// </summary>
        /// <param name="str">旧字符串</param>
        /// <param name="len">新字符串长度</param>
        /// <param name="HtmlEnable">为 false 时过滤 Html 标签后再进行裁切，反之则保留 Html 标签。</param>
        /// <remarks>
        /// <para>注意：<ol>
        /// <li>若字符串被截断则会在末尾追加“...”，反之则直接返回原始字符串。</li>
        /// <li>参数 <paramref name="HtmlEnable"/> 为 false 时会先调用<see cref="uoLib.Common.Functions.HtmlFilter"/>过滤掉 Html 标签再进行裁切。</li>
        /// <li>中文按照两个字符计算。若指定长度位置恰好只获取半个中文字符，则会将其补全，如下面的例子：<br/>
        /// <code><![CDATA[
        /// string str = "感谢使用uoLib。";
        /// string A = CutStr(str,4);   // A = "感谢..."
        /// string B = CutStr(str,5);   // B = "感谢使..."
        /// ]]></code></li>
        /// </ol>
        /// </para>
        /// </remarks>
        public static string CutStr(string str, int len, bool HtmlEnable)
        {
            if (str == null || str.Length == 0 || len <= 0) { return string.Empty; }

            if (HtmlEnable == false) str = HtmlFilter(str);
            int l = str.Length;

            #region 计算长度
            int clen = 0;//当前长度
            while (clen < len && clen < l)
            {
                //每遇到一个中文，则将目标长度减一。
                if ((int)str[clen] > 128) { len--; }
                clen++;
            }
            #endregion

            if (clen < l)
            {
                return str.Substring(0, clen) + "...";
            }
            else
            {
                return str;
            }
        }

        /// <summary>
        /// 裁切字符串（中文按照两个字符计算，裁切前会先过滤 Html 标签）
        /// </summary>
        /// <param name="str">旧字符串</param>
        /// <param name="len">新字符串长度</param>
        /// <remarks>
        /// <para>注意：<ol>
        /// <li>若字符串被截断则会在末尾追加“...”，反之则直接返回原始字符串。</li>
        /// <li>中文按照两个字符计算。若指定长度位置恰好只获取半个中文字符，则会将其补全，如下面的例子：<br/>
        /// <code><![CDATA[
        /// string str = "感谢使用uoLib模块。";
        /// string A = CutStr(str,4);   // A = "感谢..."
        /// string B = CutStr(str,5);   // B = "感谢使..."
        /// ]]></code></li>
        /// </ol>
        /// </para>
        /// </remarks>
        public static string CutStr(string str, int len)
        {
            if (string.IsNullOrEmpty(str)) { return string.Empty; }
            else
            {
                return CutStr(str, len, false);
            }
        }

        /// <summary>
        /// 获取字符串长度。与string.Length不同的是，该方法将中文作 2 个字符计算。
        /// </summary>
        /// <param name="str">目标字符串</param>
        /// <returns></returns>
        public static int GetLength(string str)
        {
            if (str == null || str.Length == 0) { return 0; }

            int l = str.Length;
            int realLen = l;

            #region 计算长度
            int clen = 0;//当前长度
            while (clen < l)
            {
                //每遇到一个中文，则将实际长度加一。
                if ((int)str[clen] > 128) { realLen++; }
                clen++;
            }
            #endregion

            return realLen;
        }

        /// <summary>
        /// 过滤HTML标签
        /// </summary>
        public static string HtmlFilter(string str)
        {
            if (string.IsNullOrEmpty(str)) { return string.Empty; }
            else
            {
                Regex re = new Regex("<\\/*[^<>]*>", RegexOptions.IgnoreCase);
                return re.Replace(str, "");
            }
        }

        /// <summary>
        /// 过滤字符串中的空格
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string EmptyStringFilter(string str)
        {
            if (string.IsNullOrEmpty(str)) { return string.Empty; }
            else
            {
                Regex re = new Regex(@"\s+", RegexOptions.IgnoreCase);
                return re.Replace(str, "");
            }
        }

        /// <summary>
        /// 正则表达式过滤字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="regex">正则表达式</param>
        /// <returns></returns>
        public static string RegexFilter(string str, string regex)
        {
            if (string.IsNullOrEmpty(str)) { return string.Empty; }
            else
            {
                Regex re = new Regex(regex, RegexOptions.IgnoreCase);
                return re.Replace(str, "");
            }
        }

        /// <summary>
        ///判断输入字符串是否为数字
        /// </summary>
        /// <param name="nValue">字符串</param>
        /// <returns></returns>
        public static bool IsNumeric(string nValue)
        {
            int i, iAsc, idecimal = 0;
            if (nValue.Trim() == "") return false;
            for (i = 0; i <= nValue.Length - 1; i++)
            {
                iAsc = (int)Convert.ToChar(nValue.Substring(i, 1));
                //'-'45 '.'46 '''0-9' 48-57
                if (iAsc == 45)
                {
                    if (nValue.Length == 1)//不能只有一个负号
                    {
                        return false;
                    }
                    if (i != 0)   //'-'不能在字符串中间
                    {
                        return false;
                    }
                }
                else if (iAsc == 46)
                {
                    idecimal++;
                    if (idecimal > 1)  //如果有两个以上的小数点
                        return false;
                }
                else if (iAsc < 48 || iAsc > 57)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 判断是否为整数
        /// </summary>
        /// <param name="nValue"></param>
        /// <returns></returns>
        public static bool IsInteger(string nValue)
        {
            int i, iAsc;
            if (nValue.Trim() == "") return false;
            for (i = 0; i <= nValue.Length - 1; i++)
            {
                iAsc = (int)Convert.ToChar(nValue.Substring(i, 1));
                //'-' 45 '0-9' 48-57
                if (iAsc == 45)
                {
                    if (nValue.Length == 1)//不能只有一个负号
                    {
                        return false;
                    }
                    if (i != 0)   //'-'不能在字符串中间
                    {
                        return false;
                    }
                }
                else if (iAsc < 48 || iAsc > 57)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 按字节长度截取字符串(支持截取带HTML代码样式的字符串)
        /// </summary>
        /// <param name="param">将要截取的字符串参数</param>
        /// <param name="length">截取的字节长度</param>
        /// <param name="end">字符串末尾补上的字符串</param>
        /// <returns>返回截取后的字符串</returns>
        public static string SubstringToHTML(string param, int length, string end)
        {
            string Pattern = null;
            MatchCollection m = null;
            StringBuilder result = new StringBuilder();
            int n = 0;
            char temp;
            bool isCode = false; //是不是HTML代码
            bool isHTML = false; //是不是HTML特殊字符,如&nbsp;
            char[] pchar = param.ToCharArray();
            for (int i = 0; i < pchar.Length; i++)
            {
                temp = pchar[i];
                if (temp == '<')
                {
                    isCode = true;
                }
                else if (temp == '&')
                {
                    isHTML = true;
                }
                else if (temp == '>' && isCode)
                {
                    n = n - 1;
                    isCode = false;
                }
                else if (temp == ';' && isHTML)
                {
                    isHTML = false;
                }

                if (!isCode && !isHTML)
                {
                    n = n + 1;
                    //UNICODE码字符占两个字节
                    if (System.Text.Encoding.Default.GetBytes(temp + "").Length > 1)
                    {
                        n = n + 1;
                    }
                }

                result.Append(temp);
                if (n >= length)
                {
                    break;
                }
            }
            result.Append(end);
            //取出截取字符串中的HTML标记
            string temp_result = result.ToString().Replace("(>)[^<>]*(<?)", "$1$2");
            //去掉不需要结素标记的HTML标记
            temp_result = temp_result.Replace(@"</?(AREA|BASE|BASEFONT|BODY|BR|COL|COLGROUP|DD|DT|FRAME|HEAD|HR|HTML|IMG|INPUT|ISINDEX|LI|LINK|META|OPTION|P|PARAM|TBODY|TD|TFOOT|TH|THEAD|TR|area|base|basefont|body|br|col|colgroup|dd|dt|frame|head|hr|html|img|input|isindex|li|link|meta|option|p|param|tbody|td|tfoot|th|thead|tr)[^<>]*/?>",
             "");
            //去掉成对的HTML标记
            temp_result = temp_result.Replace(@"<([a-zA-Z]+)[^<>]*>(.*?)</\1>", "$2");
            //用正则表达式取出标记
            Pattern = ("<([a-zA-Z]+)[^<>]*>");
            m = Regex.Matches(temp_result, Pattern);

            ArrayList endHTML = new ArrayList();

            foreach (Match mt in m)
            {
                endHTML.Add(mt.Result("$1"));
            }
            //补全不成对的HTML标记
            for (int i = endHTML.Count - 1; i >= 0; i--)
            {
                result.Append("</");
                result.Append(endHTML[i]);
                result.Append(">");
            }
            return result.ToString();
        }

        /// <summary>
        /// 计算有多少个中文字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int CountChineseWord(String str)
        {
            int n = 0, i = 0;
            if (string.IsNullOrEmpty(str))
            {
                n = 0;
            }
            else
            {
                for (i = 0; i < str.Length; i++)
                {
                    byte[] b = Encoding.Default.GetBytes(str.Substring(i, 1));
                    if (b.Length > 1)
                    {
                        n = n + 1;
                    }
                }
            }
            return n;
        }

        /// <summary>
        /// 过滤字符串 html标签和编码的标签如:&nbsp;
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string FilterString(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            //删 除脚本
            str = Regex.Replace(str, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            //删 除HTML
            str = Regex.Replace(str, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, @"-->", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, @"<!--.*", "", RegexOptions.IgnoreCase);

            str = Regex.Replace(str, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, @"&#(\d+);", "", RegexOptions.IgnoreCase);

            return str;
        }

        /// <summary>
        /// 转换人民币大小金额 
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string CmycurD(decimal num)
        {
            string str = "零壹贰叁肆伍陆柒捌玖";
            string str2 = "万仟佰拾亿仟佰拾万仟佰拾元角分";
            string str3 = "";
            string str4 = "";
            string str5 = "";
            string str6 = "";
            string str7 = "";
            int num4 = 0;
            num = Math.Round(Math.Abs(num), 2);
            str4 = ((long)(num * 100M)).ToString();
            int length = str4.Length;
            if (length > 15) return "溢出";
            str2 = str2.Substring(15 - length);
            for (int i = 0; i < length; i++)
            {
                str3 = str4.Substring(i, 1);
                int startIndex = Convert.ToInt32(str3);
                if (i != length - 3 && i != length - 7 && i != length - 11 && i != length - 15)
                {
                    if (str3 == "0")
                    {
                        str6 = "";
                        str7 = "";
                        num4++;
                    }
                    else if (str3 != "0" && num4 != 0)
                    {
                        str6 = "零" + str.Substring(startIndex, 1);
                        str7 = str2.Substring(i, 1);
                        num4 = 0;
                    }
                    else
                    {
                        str6 = str.Substring(startIndex, 1);
                        str7 = str2.Substring(i, 1);
                        num4 = 0;
                    }
                }
                else if (str3 != "0" && num4 != 0)
                {
                    str6 = "零" + str.Substring(startIndex, 1);
                    str7 = str2.Substring(i, 1);
                    num4 = 0;
                }
                else if (str3 != "0" && num4 == 0)
                {
                    str6 = str.Substring(startIndex, 1);
                    str7 = str2.Substring(i, 1);
                    num4 = 0;
                }
                else if (str3 == "0" && num4 >= 3)
                {
                    str6 = "";
                    str7 = "";
                    num4++;
                }
                else if (length >= 11)
                {
                    str6 = "";
                    num4++;
                }
                else
                {
                    str6 = "";
                    str7 = str2.Substring(i, 1);
                    num4++;
                }
                if (i == length - 11 || i == length - 3) str7 = str2.Substring(i, 1);
                str5 = str5 + str6 + str7;
                if (i == length - 1 && str3 == "0") str5 = str5 + '整';
            }
            if (num == 0M) str5 = "零元整";
            return str5;
        }

        /// <summary>
        /// 转换人民币大小金额 (一个重载，将字符串先转换成数字在调用CmycurD) 
        /// </summary>
        /// <param name="numstr"></param>
        /// <returns></returns>
        public static string CmycurD(string numstr)
        {
            try
            {
                return CmycurD(Convert.ToDecimal(numstr));
            }
            catch
            {
                return "非数字形式！";
            }
        }

        /// <summary>
        /// 删除结尾最后一个逗号
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string DelLastComma(string str)
        {
            return str.Substring(0, str.LastIndexOf(","));
        }

        /// <summary>
        /// 删除最后结尾的指定字符后的字符 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="strchar"></param>
        /// <returns></returns>
        public static string DelLastChar(string str, string strchar)
        {
            return str.Substring(0, str.LastIndexOf(strchar));
        }
        /// <summary>
        /// 得到数组列表以逗号分隔的字符串
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string GetArrayStr(List<int> list)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < list.Count; i++)
            {
                if (i == list.Count - 1)
                    builder.Append(list[i].ToString());
                else
                {
                    builder.Append(list[i]);
                    builder.Append(",");
                }
            }
            return builder.ToString();
        }

        /// <summary>
        /// 转半角的函数(SBC case) 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToDBC(string input)
        {
            char[] chArray = input.ToCharArray();
            for (int i = 0; i < chArray.Length; i++)
            {
                if (chArray[i] == '　')
                    chArray[i] = ' ';
                else if (chArray[i] > 0xff00 && chArray[i] < 0xff5f) chArray[i] = (char)(chArray[i] - 0xfee0);
            }
            return new string(chArray);
        }

        /// <summary>
        /// 转全角的函数(SBC case) 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToSBC(string input)
        {
            char[] chArray = input.ToCharArray();
            for (int i = 0; i < chArray.Length; i++)
            {
                if (chArray[i] == ' ')
                    chArray[i] = '　';
                else if (chArray[i] < '\x007f') chArray[i] = (char)(chArray[i] + 0xfee0);
            }
            return new string(chArray);
        }

        /// <summary>
        /// 过滤sql注入
        /// </summary>
        /// <param name="String"></param>
        /// <param name="IsDel"></param>
        /// <returns></returns>
        public static string SqlSafeString(string String, bool IsDel)
        {
            if (IsDel)
            {
                String = String.Replace("'", "");
                String = String.Replace("\"", "");
                return String;
            }
            String = String.Replace("'", "&#39;");
            String = String.Replace("\"", "&#34;");
            return String;
        }

        /// <summary>
        /// 过滤搜索--去掉html标签,只保留数字、字母、汉字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string SafeSearchStr(string str)
        {
            string tmpStr = EmptyStringFilter(HtmlFilter(str));
            Regex re = new Regex(@"[^(0-9A-Za-z|\u3E00-\u9FA5)]", RegexOptions.IgnoreCase);
            return re.Replace(tmpStr, "");
        }
        /// <summary>
        /// 过滤搜索--去掉html标签,只保留数字、字母、汉字
        /// </summary>
        /// <param name="str"></param>
        /// <param name="len">保留的字符数</param>
        /// <returns></returns>
        public static string SafeSearchStr(string str, int len)
        {
            string tmpStr = EmptyStringFilter(HtmlFilter(str));
            Regex re = new Regex(@"[^(0-9A-Za-z|\u3E00-\u9FA5)]", RegexOptions.IgnoreCase);

            if (len > 0 && tmpStr.Length > len)
                return re.Replace(tmpStr, "").Substring(0, len);
            return re.Replace(tmpStr, "");
        }

        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="splitstr"></param>
        /// <returns></returns>
        public static string[] SplitMulti(string str, string splitstr)
        {
            string[] strArray = null;
            if (str != null && str != "") strArray = new Regex(splitstr).Split(str);
            return strArray;
        }

        /// <summary>
        /// 过滤掉 脚本语言标记及其语言内容。
        /// </summary>
        /// <param name="_ScriptStr"></param>
        /// <returns></returns>
        public static string NoScript(string _ScriptStr)
        {
            _ScriptStr = _ScriptStr.Trim();
            return _ScriptStr = Regex.Replace(_ScriptStr, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 过滤掉 CSS标记及CSS内容。
        /// </summary>
        /// <param name="_CSSStr"></param>
        /// <returns></returns>
        public static string NoCSS(string _CSSStr)
        {
            _CSSStr = _CSSStr.Trim();
            return _CSSStr = Regex.Replace(_CSSStr, @"<STYLE[^>]*?>.*?</STYLE>", "", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
        }

        /// <summary>
        ///  过滤掉除P标签以外的HTML标签,保留内容。
        /// </summary>
        /// <param name="_str">要过滤的原字符串</param>
        /// <returns>过滤掉HTML代码后的字符串</returns>
        public static string NoHTMLNoP(string _HTMLStr)
        {
            _HTMLStr = _HTMLStr.Trim();
            _HTMLStr = Regex.Replace(_HTMLStr, @"(?!\<p\>)\<.[^\<]*\>", "", RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase);
            _HTMLStr = Regex.Replace(_HTMLStr, @"(?!\</p\>)\<\/[^\<]*\>", "", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
            return _HTMLStr;
        }
        /// <summary>
        /// 位数不足,补0
        /// </summary>
        /// <param name="num"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string FillInt(int num, int length)
        {
            string tmpStr = num + "";
            if (tmpStr.Length < length)
            {
                int tmpLen = length - tmpStr.Length;
                for (int i = 0; i < tmpLen; i++)
                {
                    tmpStr = ("0" + tmpStr);
                }
            }
            return tmpStr;
        }
        /// <summary>
        /// 位数不足,补0
        /// </summary>
        /// <param name="num"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string FillInt(string num, int length)
        {
            string tmpStr = num;
            if (tmpStr.Length < length)
            {
                int tmpLen = length - tmpStr.Length;
                for (int i = 0; i < tmpLen; i++)
                {
                    tmpStr = ("0" + tmpStr);
                }
            }
            return tmpStr;
        }

        /// <summary>
        /// string转换int,失败返回0
        /// </summary>
        /// <param name="intStr"></param>
        /// <returns></returns>
        public static int IntParse(string intStr)
        {
            int tmpInt = 0;
            int.TryParse(intStr, out tmpInt);
            return tmpInt;
        }

        /// <summary>
        /// 将字符串转换为Base64编码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string StringToBase64(string str)
        {
            if (string.IsNullOrEmpty(str)) return null;

            byte[] b = Encoding.Default.GetBytes(str);
            return Convert.ToBase64String(b);
        }

        /// <summary>
        /// 从汉字转换到16进制
        /// </summary>
        /// <param name="s"></param>
        /// <param name="charset">编码,如"utf-8","gb2312"</param>
        /// <param name="fenge">是否每字符用逗号分隔</param>
        /// <returns></returns>
        public static string ToHex(string s, string charset, bool fenge)
        {
            if ((s.Length % 2) != 0)
            {
                s += " ";//空格
                //throw new ArgumentException("s is not valid chinese string!");
            }
            System.Text.Encoding chs = System.Text.Encoding.GetEncoding(charset);
            byte[] bytes = chs.GetBytes(s);
            string str = "";
            for (int i = 0; i < bytes.Length; i++)
            {
                str += string.Format("{0:X}", bytes[i]);
                if (fenge && (i != bytes.Length - 1))
                {
                    str += string.Format("{0}", ",");
                }
            }
            return str.ToLower();
        }
        ///<summary>
        /// 从16进制转换成汉字
        /// </summary>
        /// <param name="hex"></param>
        /// <param name="charset">编码,如"utf-8","gb2312"</param>
        /// <returns></returns>
        public static string UnHex(string hex, string charset)
        {
            if (hex == null)
                throw new ArgumentNullException("hex");
            hex = hex.Replace(",", "");
            hex = hex.Replace("\n", "");
            hex = hex.Replace("\\", "");
            hex = hex.Replace(" ", "");
            if (hex.Length % 2 != 0)
            {
                hex += "20";//空格
            }
            // 需要将 hex 转换成 byte 数组。 
            byte[] bytes = new byte[hex.Length / 2];

            for (int i = 0; i < bytes.Length; i++)
            {
                try
                {
                    // 每两个字符是一个 byte。 
                    bytes[i] = byte.Parse(hex.Substring(i * 2, 2),
                    System.Globalization.NumberStyles.HexNumber);
                }
                catch
                {
                    // Rethrow an exception with custom message. 
                    throw new ArgumentException("hex is not a valid hex number!", "hex");
                }
            }
            System.Text.Encoding chs = System.Text.Encoding.GetEncoding(charset);
            return chs.GetString(bytes);
        }

        /// <summary>
        /// 作用：将字符串内容转化为16进制数据编码，其逆过程是Decode
        /// 参数说明：
        /// strEncode 需要转化的原始字符串
        /// 转换的过程是直接把字符转换成Unicode字符,比如数字"3"-->\u0033,汉字"我"-->\u6211
        /// 函数decode的过程是encode的逆过程.
        /// </summary>
        /// <param name="strEncode"></param>
        /// <returns></returns>
        public static string StringTo16(string str)
        {
            System.Text.StringBuilder _sb = new StringBuilder();
            foreach (short shortx in str.ToCharArray())
            {
                _sb.Append("\\u" + shortx.ToString("X4"));
            }
            return _sb.ToString();
        }
        ///// <summary>
        /////作用：将16进制数据编码转化为字符串，是Encode的逆过程
        ///// </summary>
        ///// <param name="strDecode"></param>
        ///// <returns></returns>
        //public static string Decode(string strDecode)
        //{
        //    string sResult = "";
        //    for (int i = 0; i < strDecode.Length / 4; i++)
        //    {
        //        sResult += (char)short.Parse(strDecode.Substring(i * 4, 4), global::System.Globalization.NumberStyles.HexNumber);
        //    }
        //    return sResult;
        //}
    }
}
