using EF.Application.Model;
using EF.Application.Model.Custom;
using EF.Application.Model.Dtos;
using EF.Application.Model.zTree;
using EF.Core.Side;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TW_DeviceSystem.App_Start;

namespace TW_DeviceSystem.Controllers
{
    [AccountAuthorize]
    public class PowerController : Controller
    {
        private readonly static string conStr = ConfigurationManager.ConnectionStrings["constrMain"].ToString();
        private readonly static t_organizeBLL bll = new t_organizeBLL();
        private readonly static t_organizeDistributeBLL organozeDisbll = new t_organizeDistributeBLL();

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获取树在Tree
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult initTree()
        {
            List<zNodes> nodeList = new List<zNodes>();
            getFirstNodes(nodeList, 0);
            uniteModel<zNodes> models = new uniteModel<zNodes>
            {
                code = 0,
                msg = "查询成功!",
                count = nodeList.Count,
                data = nodeList
            };
            return Json(models, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取父节点
        /// </summary>
        /// <param name="nodeList"></param>
        /// <param name="parentId"></param>
        public void getFirstNodes(List<zNodes> nodeList, int parentId)
        {
            List<t_organize> list = bll.GetNodesByParent(conStr, parentId, null, null);
            for (int i = 0; i < list.Count; i++)
            {
                zNodes nodes = new zNodes();
                nodes.Id = list[i].Id;
                nodes.path = list[i].nodePath;
                nodes.name = list[i].name;
                c_Count counts = bll.GetNodesCountByParent(conStr, nodes.Id);
                nodes.open = false;
                nodes.parentId = list[i].parentId;
                nodes.isElectric = list[i].isElectric;
                nodes.isLine = list[i].isLine;
                nodes.CreateTime = list[i].CreateTime.ToString("yyyy-MM-dd HH:mm:ss");
                nodes.children = new List<zNodes>();
                nodes.count = counts.count;
                nodeList.Add(nodes);
            }
        }

        /// <summary>
        /// 根据父级节点获取子节点分页
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public JsonResult getNodes(int parentId, int pageIndex, int pageSize)
        {
            List<zNodes> nodeList = new List<zNodes>();
            List<t_organize> list = bll.GetNodesByParent(conStr, parentId, pageIndex, pageSize);
            for (int i = 0; i < list.Count; i++)
            {
                zNodes nodes = new zNodes();
                nodes.Id = list[i].Id;
                nodes.path = list[i].nodePath;
                nodes.name = list[i].name;
                nodes.open = false;
                nodes.parentId = list[i].parentId;
                nodes.isElectric = list[i].isElectric;
                nodes.isLine = list[i].isLine;
                nodes.CreateTime = list[i].CreateTime.ToString("yyyy-MM-dd HH:mm:ss");
                c_Count counts = bll.GetNodesCountByParent(conStr, nodes.Id);
                nodes.count = counts.count;
                if (nodes.count > 0)
                {
                    nodes.children = new List<zNodes>();
                }
                nodeList.Add(nodes);
            }
            return Json(nodeList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 添加用户权限
        /// </summary>
        /// <param name="roleArray"></param>
        /// <param name="nodeArray"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual JsonResult AddUserOrganize(List<long> userArray, List<long> nodeArray)
        {
            List<long> organizeId = new List<long>();
            for (int i = 0; i < userArray.Count; i++)
            {
                List<t_organizeDistribute> userOrganizeList = organozeDisbll.GetUserOrganize(conStr, userArray[i]);
                for (int j = 0; j < userOrganizeList.Count; j++)
                {
                    organizeId.Add(userOrganizeList[j].Id);
                }
                organozeDisbll.Delete(conStr, organizeId);
            }
            organozeDisbll.Add(conStr, nodeArray, userArray);

            uniteModel<t_organizeDistribute> model = new uniteModel<t_organizeDistribute>
            {
                code = 0,
                msg = string.Format("用户权限分配成功{0}项!", nodeArray.Count),
                count = nodeArray.Count,
                data = null
            };
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public virtual JsonResult getPowerTree()
        {
            List<nodes> list = bll.getPowerNodes(conStr);
            return Json(list, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public virtual JsonResult getUserTree(long userId)
        {
            List<nodes> list=  bll.getNodeByUser(conStr, userId);
            return Json(list, JsonRequestBehavior.AllowGet);
        }


        #region 获取用户权限树
        [HttpPost]
        public virtual JsonResult initUserTree(int userId)
        {
            List<zNodes> nodeList = new List<zNodes>();
            GetUserFirstNode(nodeList, userId, 0);
            uniteModel<zNodes> models = new uniteModel<zNodes>
            {
                code = 0,
                msg = "查询成功!",
                count = nodeList.Count,
                data = nodeList
            };
            return Json(models, JsonRequestBehavior.AllowGet);
        }

        public virtual void GetUserFirstNode(List<zNodes> nodeList, int userId, int parentId)
        {
            List<t_organize> list = bll.GetNodeByUser(conStr, userId, parentId, null, null);
            for (int i = 0; i < list.Count; i++)
            {
                zNodes nodes = new zNodes();
                nodes.Id = list[i].Id;
                nodes.path = list[i].nodePath;
                nodes.name = list[i].name;
                c_Count counts = bll.GetNodesCountByUserParentId(conStr, userId, nodes.Id);
                nodes.count = counts.count;
                nodes.open = false;
                nodes.parentId = list[i].parentId;
                nodes.isElectric = list[i].isElectric;
                nodes.isLine = list[i].isLine;
                nodes.CreateTime = list[i].CreateTime.ToString("yyyy-MM-dd HH:mm:ss");
                if (nodes.count > 0)
                {
                    nodes.children = new List<zNodes>();
                }
                nodeList.Add(nodes);
            }
        }

        public virtual JsonResult getUserNodes(int parentId, int userId, int pageIndex, int pageSize)
        {
            List<zNodes> nodeList = new List<zNodes>();
            List<t_organize> list = bll.GetNodeByUser(conStr, userId, parentId, pageIndex, pageSize);
            for (int i = 0; i < list.Count; i++)
            {
                zNodes nodes = new zNodes();
                nodes.Id = list[i].Id;
                nodes.path = list[i].nodePath;
                nodes.name = list[i].name;
                nodes.open = false;
                nodes.parentId = list[i].parentId;
                nodes.isElectric = list[i].isElectric;
                nodes.isLine = list[i].isLine;
                nodes.isJoint = list[i].isJoint;
                nodes.CreateTime = list[i].CreateTime.ToString("yyyy-MM-dd HH:mm:ss");
                c_Count counts = bll.GetNodesCountByUserParentId(conStr, userId, nodes.Id);
                nodes.count = counts.count;
                if (nodes.count > 0)
                {
                    nodes.children = new List<zNodes>();
                }
                nodeList.Add(nodes);
            }
            return Json(nodeList, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}