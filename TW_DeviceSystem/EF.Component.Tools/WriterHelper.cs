using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace EF.Component.Tools
{
    public class WriterHelper
    {
        XmlDocument doc = null;

        XmlDocument XmlDoc
        {
            get
            {
                string strPath = System.AppDomain.CurrentDomain.BaseDirectory + "\\web.config";
                doc = new XmlDocument();
                doc.Load(strPath);
                return doc;
            }
            set { doc = value; }
        }
        /// <summary>
        /// 根据编号获取路径
        /// </summary>
        /// <param name="strid"></param>
        /// <returns></returns>
        public string RewriterUrl(string strid)
        {
            string strUrl = "";
            string strSelectUrl = "/configuration/RewriterConfig/Rules/RewriterRule[@id='" + strid + "']";
            XmlNode node = XmlDoc.SelectSingleNode(strSelectUrl);
            if (node != null)
            {
                strUrl = node.ChildNodes[0].InnerText.Replace("~", "");
            }
            return strUrl;
        }

        /// <summary>
        /// 根据编号获取路径
        /// </summary>
        /// <param name="strid"></param>
        /// <returns></returns>
        public DataTable RewriterTable(string strid)
        {
            string strSelectUrl = "/configuration/RewriterConfig/Rules/RewriterRule[@id='" + strid + "']";
            DataTable dtRewriter = RewriterList(strSelectUrl);
            return dtRewriter;
        }

        /// <summary>
        /// 根据编号获取路径 英文
        /// </summary>
        /// <param name="strid"></param>
        /// <returns></returns>
        public string RewriterUrl_en(string strid)
        {
            string strUrl = "";
            string strSelectUrl = "/configuration/RewriterConfig/Rules/RewriterRule[@key='" + strid + "']";
            XmlNode node = XmlDoc.SelectSingleNode(strSelectUrl);
            if (node != null)
            {
                strUrl = node.ChildNodes[0].InnerText.Replace("~", "");
            }
            return strUrl;
        }


        /// <summary>
        /// 根据编号获取路径 英文
        /// </summary>
        /// <param name="strid"></param>
        /// <returns></returns>
        public DataTable RewriterTable_en(string strid)
        {
            string strSelectUrl = "/configuration/RewriterConfig/Rules/RewriterRule[@key='" + strid + "']";
            DataTable dtRewriter = RewriterList(strSelectUrl);
            return dtRewriter;
        }
        private DataTable RewriterList(string strSelectUrl)
        {
            XmlNode node = XmlDoc.SelectSingleNode(strSelectUrl);
            DataTable dtRewriter = new DataTable();
            dtRewriter.Columns.Add(new DataColumn("LookFor"));
            dtRewriter.Columns.Add(new DataColumn("SendTo"));
            dtRewriter.Columns.Add(new DataColumn("PageTitle"));
            dtRewriter.Columns.Add(new DataColumn("PageKeywords"));
            dtRewriter.Columns.Add(new DataColumn("PageDescription"));
            if (node != null)
            {
                DataRow dr = dtRewriter.NewRow();
                dr["LookFor"] = node.ChildNodes[0].InnerText.Replace("~", "");
                dr["SendTo"] = node.ChildNodes[1].InnerText;
                dr["PageTitle"] = node.ChildNodes[2].InnerText;
                dr["PageKeywords"] = node.ChildNodes[3].InnerText;
                dr["PageDescription"] = node.ChildNodes[4].InnerText;
                dtRewriter.Rows.Add(dr);
            }
            return dtRewriter;
        }


    }
}
