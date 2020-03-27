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
using TW_DeviceSystem.Common;

namespace TW_DeviceSystem.Controllers
{
    [AccountAuthorize]
    public class OrganizeController : Controller
    {
        private readonly static string conStr = ConfigurationManager.ConnectionStrings["constrMain"].ToString();
        private readonly static t_organizeBLL bll = new t_organizeBLL();
        private readonly static t_DeviceBLL devicebll = new t_DeviceBLL();
        private readonly static t_deviceBigTypeBLL bigTypebll = new t_deviceBigTypeBLL();
        private readonly static t_deviceSmallTypeBLL smallTypebll = new t_deviceSmallTypeBLL();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Edit(int Id)
        {
            t_organize model = bll.getSingle(conStr, Id);
            return View(model);
        }

        public ActionResult EditCommit(t_organize model)
        {
            bll.Edit(conStr, model);
            return RedirectToAction("Index", "Organize");
        }

        public virtual JsonResult GetOrganizePage(int page,int limit,string searchText)
        {
            int totalcount = 0;
            List<t_organize> list = bll.GetListPage(conStr, page, limit, out totalcount, searchText);
            uniteModel<t_organize> model = new uniteModel<t_organize>
            {
                code = 0,
                msg = "组织架构列表查询成功!",
                count = totalcount,
                data = list
            };
            return Json(model, JsonRequestBehavior.AllowGet);
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
            t_userInfo loginUser = CookieUserHelper.getCookieUser();
            List<t_organize> list = new List<t_organize>();
            //如果是超级管理员可看见所有树形节点
            if (loginUser.roleId == 1 || loginUser.roleId == 3)
            {
                list = bll.GetNodesByParent(conStr, parentId, null, null);
                for (int i = 0; i < list.Count; i++)
                {
                    zNodes nodes = new zNodes();
                    nodes.Id = list[i].Id;
                    nodes.path = list[i].nodePath;
                    nodes.name = list[i].name;
                    c_Count counts = bll.GetNodesCountByParent(conStr, nodes.Id);
                    nodes.count = counts.count;
                    nodes.open = false;
                    nodes.parentId = list[i].parentId;
                    nodes.icon = "/Images/treeIcon/1_open.png";
                    nodes.iconOpen = "/Images/treeIcon/1_open.png";
                    nodes.iconClose = "/Images/treeIcon/1_close.png";
                    nodes.isElectric = list[i].isElectric;
                    nodes.isLine = list[i].isLine;
                    nodes.isJoint = list[i].isJoint;
                    nodes.CreateTime = list[i].CreateTime.ToString("yyyy-MM-dd HH:mm:ss");
                    nodes.children = new List<zNodes>();
                    nodeList.Add(nodes);
                    //getChildNodes(nodeList, nodes);
                }
            }
            else
            {
                list = bll.GetOrganizeByUser(conStr, loginUser.Id, parentId, null, null);
                for (int i = 0; i < list.Count; i++)
                {
                    zNodes nodes = new zNodes();
                    nodes.Id = list[i].Id;
                    nodes.path = list[i].nodePath;
                    nodes.name = list[i].name;
                    c_Count counts = bll.GetNodesCountByUser(conStr, nodes.Id, loginUser.Id);
                    nodes.count = counts.count;
                    nodes.open = false;
                    nodes.parentId = list[i].parentId;
                    nodes.icon = "/Images/treeIcon/1_open.png";
                    nodes.iconOpen = "/Images/treeIcon/1_open.png";
                    nodes.iconClose = "/Images/treeIcon/1_close.png";
                    nodes.isElectric = list[i].isElectric;
                    nodes.isLine = list[i].isLine;
                    nodes.isJoint = list[i].isJoint;
                    nodes.CreateTime = list[i].CreateTime.ToString("yyyy-MM-dd HH:mm:ss");
                    nodes.children = new List<zNodes>();
                    nodeList.Add(nodes);
                    //getChildNodes(nodeList, nodes);
                }
            }
        }

        public JsonResult getNodes(int parentId, int pageIndex, int pageSize, string searchText)
        {
            List<zNodes> nodeList = new List<zNodes>();
            t_userInfo loginUser = CookieUserHelper.getCookieUser();
            List<t_organize> list = new List<t_organize>();
            //如果是超级管理员可看见所有树形节点
            if (loginUser.roleId == 1 || loginUser.roleId == 3)
            {
                list = bll.GetNodesByParent(conStr, parentId, pageIndex, pageSize);
            }
            else
            {
                list = bll.GetOrganizeByUser(conStr, loginUser.Id, parentId, pageIndex, pageSize);
            }
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
                nodes.icon = "/Images/treeIcon/1_open.png";
                nodes.iconOpen = "/Images/treeIcon/1_open.png";
                nodes.iconClose = "/Images/treeIcon/1_close.png";
                nodes.CreateTime = list[i].CreateTime.ToString("yyyy-MM-dd HH:mm:ss");
                if (list[i].isJoint)
                {
                    c_Count cCount = devicebll.GetDeviceCount(conStr, nodes.Id, searchText);
                    nodes.count = cCount.count;
                }
                else if (loginUser.roleId == 1 || loginUser.roleId == 3)
                {
                    c_Count counts = bll.GetNodesCountByParent(conStr, nodes.Id);
                    nodes.count = counts.count;
                }
                else
                {
                    c_Count counts = bll.GetOrganizeCountByUser(conStr, loginUser.Id, nodes.Id);
                    nodes.count = counts.count;
                }
                if (nodes.count > 0)
                {
                    nodes.children = new List<zNodes>();
                }
                nodeList.Add(nodes);
            }
            return Json(nodeList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 根据供电和线路获取设备
        /// </summary>
        /// <param name="ElectricId">供电Id</param>
        /// <param name="LineId">线路Id</param>
        /// <param name="count">获取数量</param>
        /// <returns></returns>
        public JsonResult GetDevice(int parentId, int pageIndex, int pageSize, string searchText)
        {
            int totalcount = 0;
            List<t_Device> list = devicebll.GetListPage(conStr, parentId, pageIndex, pageSize, out totalcount, searchText);
            List<zNodes> nodeList = new List<zNodes>();
            foreach (var item in list)
            {
                zNodes node = new zNodes();
                node.Id = item.Id;
                t_deviceBigType deviceBigType = bigTypebll.GetSingle(conStr, Convert.ToInt32(item.bigTypeId));
                t_deviceSmallType deviceSmallType = smallTypebll.GetSingle(conStr, Convert.ToInt32(item.smallTypeId));
                node.name = item.deviceName;
                node.path = item.nodePath;
                node.actionAddress = deviceSmallType.actionAddress + "?TerminalId=" + item.TerminalId + "&ElectricId=" + item.ElectricId + "&LineId=" + item.LineId;
                node.icon = deviceBigType.IconUrl;
                node.isDevice = true;
                node.isElectric = false;
                node.isLine = false;
                node.isJoint = false;
                nodeList.Add(node);
            }
            return Json(nodeList, JsonRequestBehavior.AllowGet);
        }



        /// <summary>
        /// 获取zTree树Json数据
        /// </summary>
        /// <returns></returns>
        public JsonResult GetzTreeData()
        {
            List<zNodes> nodeList = new List<zNodes>();
            zNodes node = new zNodes();
            node.Id = 0;
            node.isElectric = false;
            node.isLine = false;
            node.isJoint = false;
            node.isDevice = false;
            node.children = new List<zNodes>();
            GetOrganizeNodes(nodeList, node);
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
        /// 递归获取子节点
        /// </summary>
        /// <param name="nodeList"></param>
        /// <param name="node"></param>
        public void  GetOrganizeNodes(List<zNodes> nodeList, zNodes node)
        {
            if (node.isJoint)
            {
                List<t_Device> dlist = devicebll.GetDeviceByParent(conStr, node.Id);
                for (int j = 0; j < dlist.Count; j++)
                {
                    zNodes dnodes = new zNodes();
                    dnodes.Id = dlist[j].Id;
                    dnodes.path = dlist[j].nodePath;
                    dnodes.name = dlist[j].deviceName;
                    dnodes.parentId = dlist[j].parentId;
                    dnodes.isElectric = false;
                    dnodes.isLine = false;
                    dnodes.isJoint = false;
                    dnodes.isDevice = true;
                    dnodes.CreateTime = Convert.ToDateTime(dlist[j].createTime).ToString("yyyy-MM-dd HH:mm:ss");
                    node.children.Add(dnodes);
                }
            }
            List<t_organize> list = bll.GetNodesByParent(conStr, node.Id);
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    zNodes nodes = new zNodes();
                    nodes.Id = list[i].Id;
                    nodes.path = list[i].nodePath;
                    nodes.name = list[i].name;
                    nodes.parentId = list[i].parentId;
                    nodes.isElectric = list[i].isElectric;
                    nodes.isLine = list[i].isLine;
                    nodes.isJoint = list[i].isJoint;
                    nodes.isDevice = false;
                    nodes.CreateTime = list[i].CreateTime.ToString("yyyy-MM-dd HH:mm:ss");
                    nodes.children = new List<zNodes>();
                    if (list[i].parentId > 0)
                    {
                        node.children.Add(nodes);
                    }
                    else
                    {
                        nodeList.Add(nodes);
                    }
                    GetOrganizeNodes(nodeList, nodes);
                }
            }
        }

        
        /// <summary>
        /// 获取树形数组数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public virtual JsonResult getNodeList()
        {
            t_userInfo loginUser = CookieUserHelper.getCookieUser();
            if (loginUser.roleId==1 || loginUser.roleId == 3)
            {
                List<nodes> list = bll.getAllNodeList(conStr);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            else
            {
                List<nodes> list = bll.getNodeByUser(conStr, loginUser.Id);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
   
        }
    }
}