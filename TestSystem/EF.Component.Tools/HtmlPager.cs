using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Component.Tools
{
    /// <summary>
    /// Html分页
    /// </summary>
    public class HtmlPager
    {
        /// <summary>
        /// 写出分页 
        /// </summary>
        /// <param name="pageCount">总页数</param>
        /// <param name="currentPage">当前页</param>
        /// <param name="prefix">上一页</param>
        /// <param name="suffix">下一页</param>
        /// <returns></returns>
        public static string GetHtmlPager(int pageCount, int currentPage, string prefix, string suffix)
        {
            int num = 4;
            int num2 = 1;
            pageCount = (pageCount == 0) ? 1 : pageCount;
            currentPage = (currentPage == 0) ? 1 : currentPage;
            StringBuilder builder = new StringBuilder();
            builder.Append("<table cellpadding=0 cellspacing=1 class=\"pager\">\r<tr>\r");
            builder.Append("<td class=pagerTitle>&nbsp;分页&nbsp;</td>\r");
            builder.Append("<td class=pagerTitle>&nbsp;" + currentPage.ToString() + "/" + pageCount.ToString() + "&nbsp;</td>\r");
            if (currentPage - num < 2)
                num2 = 1;
            else
                num2 = currentPage - num;
            int num3 = pageCount;
            if (currentPage + num >= pageCount)
                num3 = pageCount;
            else
                num3 = currentPage + num;
            if (num2 == 1)
            {
                if (currentPage > 1)
                {
                    builder.Append("<td>&nbsp;<a href='" + prefix + "1" + suffix + "' title='首页'>首页</a>&nbsp;</td>\r");
                    builder.Append("<td>&nbsp;<a href='" + prefix + Convert.ToString((int)(currentPage - 1)) + suffix + "' title='上页'>上页</a>&nbsp;</td>\r");
                }
            }
            else
            {
                builder.Append("<td>&nbsp;<a href='" + prefix + "1" + suffix + "' title='首页'>首页</a>&nbsp;</td>");
                builder.Append("<td>&nbsp;<a href='" + prefix + Convert.ToString((int)(currentPage - 1)) + suffix + "' title='上页'>上页</a>&nbsp;</td>\r");
            }
            for (int i = num2; i <= num3; i++)
            {
                if (i == currentPage)
                    builder.Append("<td class='current'>&nbsp;" + i.ToString() + "&nbsp;</td>\r");
                else
                    builder.Append("<td>&nbsp;<a href='" + prefix + i.ToString() + suffix + "' title='第" + i.ToString() + "页'>" + i.ToString() + "</a>&nbsp;</td>\r");
                if (i == pageCount) break;
            }
            if (num3 == pageCount)
            {
                if (pageCount > currentPage)
                {
                    builder.Append("<td>&nbsp;<a href='" + prefix + Convert.ToString((int)(currentPage + 1)) + suffix + "' title='下页'>下页</a>&nbsp;</td>\r");
                    builder.Append("<td>&nbsp;<a href='" + prefix + pageCount.ToString() + suffix + "' title='尾页'>尾页</a>&nbsp;</td>\r");
                }
            }
            else
            {
                builder.Append("<td>&nbsp;<a href='" + prefix + Convert.ToString((int)(currentPage + 1)) + suffix + "' title='下页'>下页</a>&nbsp;</td>\r");
                builder.Append("<td>&nbsp;<a href='" + prefix + pageCount.ToString() + suffix + "' title='尾页'>尾页</a>&nbsp;</td>\r");
            }
            builder.Append("</tr>\r</table>");
            return builder.ToString();
        }

        /// <summary>
        /// 写出分页 
        /// </summary>
        /// <param name="pageCount">页数</param>
        /// <param name="currentPage">当前页</param>
        /// <returns></returns>
        public static string GetPager(int pageCount, int currentPage)
        {
            return GetPager(pageCount, currentPage, new string[0], new string[0]);
        }
        /// <summary>
        /// 写出分页 
        /// </summary>
        /// <param name="pageCount">页数</param>
        /// <param name="currentPage">当前页</param>
        /// <param name="FieldName">地址栏参数</param>
        /// <param name="FieldValue">地址栏参数值</param>
        /// <returns></returns>
        public static string GetPager(int pageCount, int currentPage, string[] FieldName, string[] FieldValue)
        {
            string str = "";
            for (int i = 0; i < FieldName.Length; i++)
            {
                string str2 = str;
                str = str2 + "&" + FieldName[i].ToString() + "=" + FieldValue[i].ToString();
            }
            int num2 = 4;
            int num3 = 1;
            pageCount = (pageCount == 0) ? 1 : pageCount;
            currentPage = (currentPage == 0) ? 1 : currentPage;
            StringBuilder builder = new StringBuilder();
            builder.Append("<table cellpadding=0 cellspacing=1 class=\"pager\">\r<tr>\r");
            builder.Append("<td class=pagerTitle>&nbsp;分页&nbsp;</td>\r");
            builder.Append("<td class=pagerTitle>&nbsp;" + currentPage.ToString() + "/" + pageCount.ToString() + "&nbsp;</td>\r");
            if (currentPage - num2 < 2)
                num3 = 1;
            else
                num3 = currentPage - num2;
            int num4 = pageCount;
            if (currentPage + num2 >= pageCount)
                num4 = pageCount;
            else
                num4 = currentPage + num2;
            if (num3 == 1)
            {
                if (currentPage > 1)
                {
                    builder.Append("<td>&nbsp;<a href='?page=1" + str + "' title='首页'>首页</a>&nbsp;</td>\r");
                    builder.Append("<td>&nbsp;<a href='?page=" + Convert.ToString((int)(currentPage - 1)) + str + "' title='上页'>上页</a>&nbsp;</td>\r");
                }
            }
            else
            {
                builder.Append("<td>&nbsp;<a href='?page=1" + str + "' title='首页'>首页</a>&nbsp;</td>");
                builder.Append("<td>&nbsp;<a href='?page=" + Convert.ToString((int)(currentPage - 1)) + str + "' title='上页'>上页</a>&nbsp;</td>\r");
            }
            for (int j = num3; j <= num4; j++)
            {
                if (j == currentPage)
                    builder.Append("<td class='current'>&nbsp;" + j.ToString() + "&nbsp;</td>\r");
                else
                    builder.Append("<td>&nbsp;<a href='?page=" + j.ToString() + str + "' title='第" + j.ToString() + "页'>" + j.ToString() + "</a>&nbsp;</td>\r");
                if (j == pageCount) break;
            }
            if (num4 == pageCount)
            {
                if (pageCount > currentPage)
                {
                    builder.Append("<td>&nbsp;<a href='?page=" + Convert.ToString((int)(currentPage + 1)) + str + "' title='下页'>下页</a>&nbsp;</td>\r");
                    builder.Append("<td>&nbsp;<a href='?page=" + pageCount.ToString() + str + "' title='尾页'>尾页</a>&nbsp;</td>\r");
                }
            }
            else
            {
                builder.Append("<td>&nbsp;<a href='?page=" + Convert.ToString((int)(currentPage + 1)) + str + "' title='下页'>下页</a>&nbsp;</td>\r");
                builder.Append("<td>&nbsp;<a href='?page=" + pageCount.ToString() + str + "' title='尾页'>尾页</a>&nbsp;</td>\r");
            }
            builder.Append("</tr>\r</table>");
            return builder.ToString();
        }
    }
}
