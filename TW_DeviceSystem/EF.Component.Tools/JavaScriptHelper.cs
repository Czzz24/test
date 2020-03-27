using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Component.Tools
{
    public class JavaScriptHelper
    {
        #region 弹出JavaScript小窗口
        /// <summary>
        /// 弹出JavaScript小窗口
        /// </summary>
        /// <param name="strMessage">提示信息</param>
        public static void Alert(string strMessage)
        {
            System.Web.HttpContext.Current.Response.Write("<script language=\"javascript\">alert(\"" + strMessage + "\");</script>");
        }
        #endregion

        #region 弹出JavaScript小窗口并重新定向到新的URL
        /// <summary>
        /// 弹出JavaScript小窗口并重新定向到新的URL
        /// </summary>
        /// <param name="strMessage">提示信息</param>
        /// <param name="strRedirectUrl">新URL</param>
        public static void AlertAndRedirect(string strMessage, string strRedirectUrl)
        {
            System.Web.HttpContext.Current.Response.Write(String.Format("<script language=\"javascript\">alert(\"{0}\");window.location.replace(\"{1}\")</script>", strMessage, strRedirectUrl));
        }
        #endregion

        #region 重新定向到新的页面
        /// <summary>
        /// 重新定向到新的页面
        /// </summary>
        /// <param name="strRedirectUrl">新页面的URL</param>
        public static void Redirect(string strRedirectUrl)
        {
            System.Web.HttpContext.Current.Response.Write("<script language=\"javascript\">window.location=\"" + strRedirectUrl + "\";</script>");
        }
        #endregion

        #region 出对话框并且回到历史页面
        /// <summary>
        /// 弹出对话框并且回到历史页面
        /// </summary>
        /// <param name="iValue">-1/1</param>
        /// <param name="strMessage"></param>
        public static void AlertAndGoHistory(string strMessage, int iValue)
        {
            System.Web.HttpContext.Current.Response.Write(String.Format("<script language=\"JavaScript\">alert('{0}');history.go({1});</script>", strMessage, iValue));
            System.Web.HttpContext.Current.Response.End();
        }
        #endregion

        #region 根据对话框重定义到指定的页面中
        /// <summary>
        /// 根据对话框重定义到指定的页面中
        /// </summary>
        /// <param name="strMsg">信息对话框</param>
        /// <param name="TrueUrl">如果选择[是]所指向的页面地址</param>
        /// <param name="FalseUrl">如果选择[否]所指向的页面地址</param>
        public static void ConfirmGoTo(string strMsg, string TrueUrl, string FalseUrl)
        {
            System.Web.HttpContext.Current.Response.Write("<SCRIPT language=\"javascript\">");
            System.Web.HttpContext.Current.Response.Write("if (confirm(\"" + strMsg + "\"))");
            System.Web.HttpContext.Current.Response.Write("{window.location='" + TrueUrl + "';}");
            System.Web.HttpContext.Current.Response.Write("else");
            System.Web.HttpContext.Current.Response.Write("{window.location='" + FalseUrl + "';}");
            System.Web.HttpContext.Current.Response.Write("</SCRIPT>");
        }
        #endregion

        #region 回到历史页面
        /// <summary>
        /// 回到历史页面
        /// </summary>
        /// <param name="iValue">-1/1</param>
        public static void GoHistory(int iValue)
        {
            System.Web.HttpContext.Current.Response.Write(String.Format("<script language=\"JavaScript\">history.go({0});</script>", iValue));
        }
        #endregion

        #region 打开新窗口
        /// <summary>
        /// 打开新窗口
        /// </summary>
        /// <param name="Url">窗口网址</param>
        /// <param name="hieght">窗口高</param>
        /// <param name="width">窗口宽</param>
        public static void OpenWindow(string Url, int hieght, int width)
        {
            System.Web.HttpContext.Current.Response.Write(String.Format("<script type=\"text/javascript\">window.open(\"{0}\",NULL,\"height={1},width={2}\")</script>", Url, hieght, width));
            System.Web.HttpContext.Current.Response.End();
        }
        #endregion

        #region 打开指定大小的新窗体
        /// <summary>
        /// 打开指定大小的新窗体
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="width">宽</param>
        /// <param name="heigth">高</param>
        /// <param name="top">头位置</param>
        /// <param name="left">左位置</param>
        public static void OpenWebFormSize(string url, int width, int heigth, int top, int left)
        {
            #region
            string js = @"<Script language='JavaScript'>window.open('" + url + @"','','height=" + heigth + ",width=" + width + ",top=" + top + ",left=" + left + ",location=no,menubar=no,resizable=yes,scrollbars=yes,status=yes,titlebar=no,toolbar=no,directories=no');</Script>";

            System.Web.HttpContext.Current.Response.Write(js);
            System.Web.HttpContext.Current.Response.End();


            #endregion
        }
        #endregion

        #region 关闭当前窗体
        /// <summary>
        /// 关闭当前窗体
        /// </summary>
        public static void CloseWindow()
        {
            System.Web.HttpContext.Current.Response.Write("<script language=\"JavaScript\">parent.opener=null;window.close();</script>");
            System.Web.HttpContext.Current.Response.End();
        }
        #endregion

        #region 刷新父窗口
        /// <summary>
        /// 刷新父窗口
        /// </summary>
        public static void RefreshParent(string url)
        {
            System.Web.HttpContext.Current.Response.Write("<Script language='JavaScript'>window.opener.location.href='" + url + "';window.close();</Script>");
        }
        #endregion

        #region 返回值
        /// <summary>
        /// 返回值
        /// </summary>
        public static void ReturnValue(string bl)
        {
            System.Web.HttpContext.Current.Response.Write("<SCRIPT language=\"javascript\">");
            System.Web.HttpContext.Current.Response.Write("window.returnValue='" + bl + "';");
            System.Web.HttpContext.Current.Response.Write("</SCRIPT>");
        }
        #endregion

        #region 刷新父窗体Dialog
        /// <summary>
        /// 刷新父窗体Dialog
        /// </summary>
        public static void RefreshDialogParent()
        {
            System.Web.HttpContext.Current.Response.Write("<SCRIPT language=\"javascript\">");
            System.Web.HttpContext.Current.Response.Write("var args = window.dialogArguments;");
            System.Web.HttpContext.Current.Response.Write("args.location.reload();");
            System.Web.HttpContext.Current.Response.Write("</SCRIPT>");
        }
        #endregion

        #region 将换行，空格进行转换对字付串进行处理
        /// <summary>
        /// 将换行，空格进行转换对字付串进行处理
        /// </summary>
        /// <param name="strText">要处理的字符串</param>
        /// <returns></returns>
        public static string FormatStringToHTML(string strText)
        {
            strText = strText.Replace("\n", "<br/>");
            strText = strText.Replace("<", "&lt;");
            strText = strText.Replace(">", "&gt;");
            strText = strText.Replace(" ", "&nbsp;&nbsp;");

            return strText;
        }

        public static string FormatStringToCHAR(string strText)
        {
            strText = strText.Replace("&nbsp;&nbsp;", " ");
            strText = strText.Replace("&lt;", "<");
            strText = strText.Replace("&gt;", ">");
            strText = strText.Replace("<br/>", "\n");

            return strText;
        }
        #endregion

        #region 打开指定大小位置的模式对话框
        /// <summary>
        /// 打开指定大小位置的模式对话框
        /// </summary>
        /// <param name="webFormUrl">连接地址</param>
        /// <param name="width">宽</param>
        /// <param name="height">高</param>
        /// <param name="top">距离上位置</param>
        /// <param name="left">距离左位置</param>
        public static void ShowModalDialogWindow(string webFormUrl, int width, int height, int top, int left)
        {
            #region
            string features = "dialogWidth:" + width.ToString() + "px"
                + ";dialogHeight:" + height.ToString() + "px"
                + ";dialogLeft:" + left.ToString() + "px"
                + ";dialogTop:" + top.ToString() + "px"
                + ";center:yes;help=no;resizable:no;status:no;scroll=yes";
            ShowModalDialogWindow(webFormUrl, features);
            #endregion
        }

        public static void ShowModalDialogWindow(string webFormUrl, string features)
        {
            string js = ShowModalDialogJavascript(webFormUrl, features);
            System.Web.HttpContext.Current.Response.Write(js);
        }

        public static string ShowModalDialogJavascript(string webFormUrl, string features)
        {
            #region
            string js = @"<script language=javascript>							
							showModalDialog('" + webFormUrl + "','','" + features + "');</script>";
            return js;
            #endregion
        }
        #endregion
    }
}
