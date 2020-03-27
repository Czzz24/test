using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EF.Core.Side;
using EF.Application.Model;
using System.Configuration;
using EF.Application.Model.Dtos;
using EF.Application.Model.zTree;
using EF.Component.Tools;
using EF.Application.Model.Custom;
using TW_DeviceSystem.Common;
using TW_DeviceSystem.App_Start;
using TW_DeviceSystem.Enum;

namespace TW_DeviceSystem.Controllers
{
    [AccountAuthorize]
    public class DeviceManagerController : Controller
    {
        private readonly static string conStr = ConfigurationManager.ConnectionStrings["constrMain"].ToString();
        private readonly static t_organizeBLL bll = new t_organizeBLL();
        private readonly static t_dataBaseManagerBLL basebll = new t_dataBaseManagerBLL();
        private readonly static t_DeviceBLL devicebll = new t_DeviceBLL();
        private readonly static t_deviceBigTypeBLL bigTypebll = new t_deviceBigTypeBLL();
        private readonly static t_deviceSmallTypeBLL smallTypebll = new t_deviceSmallTypeBLL();
        private readonly static t_rolePowerBLL rolePowerbll = new t_rolePowerBLL();

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
            t_userInfo loginUser = CookieUserHelper.getCookieUser();
            List<t_organize> list = new List<t_organize>();
            //如果是超级管理员可看见所有树形节点
            if (loginUser.roleId == 1 || loginUser.roleId==3)
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

        public JsonResult getNodes(int parentId, int pageIndex, int pageSize)
        {
            List<zNodes> nodeList = new List<zNodes>();
            t_userInfo loginUser = CookieUserHelper.getCookieUser();
            List<t_organize> list = new List<t_organize>();
            //如果是超级管理员可看见所有树形节点
            if (loginUser.roleId == 1 || loginUser.roleId==3)
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
                    c_Count cCount = devicebll.GetDeviceCount(conStr, nodes.Id, null);
                    nodes.count = cCount.count;
                }
                else if (loginUser.roleId == 1 || loginUser.roleId==3)
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
        /// 递归获取子节点
        /// </summary>
        /// <param name="nodeList"></param>
        /// <param name="node"></param>
        public void getChildNodes(List<zNodes> nodeList, zNodes node)
        {
            List<t_organize> list = bll.GetNodesByParent(conStr, node.Id,null,null);
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    zNodes nodes = new zNodes();
                    nodes.Id = list[i].Id;
                    nodes.path = list[i].nodePath;
                    nodes.name = list[i].name;
                    nodes.isJoint = list[i].isJoint;
                    List<t_organize> lists = bll.GetNodesByParent(conStr, nodes.Id, null, null);
                    if (lists.Count > 0)
                    {
                        nodes.open = true;
                    }
                    nodes.parentId = list[i].parentId;
                    nodes.isElectric = list[i].isElectric;
                    nodes.isLine = list[i].isLine;
                    nodes.CreateTime = list[i].CreateTime.ToString("yyyy-MM-dd HH:mm:ss");
                    node.children.Add(nodes);
                    nodes.children = new List<zNodes>();
                    getChildNodes(nodeList, nodes);
                }
            }
        }

        /// <summary>
        /// 添加根节点
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult addRootNode(t_organize model)
        {
            t_userInfo loginUser = CookieUserHelper.getCookieUser();
            bool authority = false;
            if (loginUser.roleId == 1)
            {
                authority = true;
            }
            else
            {
                authority = rolePowerbll.GetUserRolePower(conStr, (long)loginUser.roleId, (long)PowerEnum.addOrganize);
            }
            if (authority == false)
            {
                uniteModel<zNodes> models = new uniteModel<zNodes>
                {
                    code = 1,
                    msg = "添加根节点失败!,无权限",
                    count = 0,
                    data = null
                };
                return Json(models, JsonRequestBehavior.AllowGet);
            }
            model.isElectric = false;
            model.isLine = false;
            model.parentId = 0;
            model.CreateTime = DateTime.Now;
            bool isAdd = bll.Add(conStr, model);
            model.nodePath = model.Id + "\\";
            bool isEdit = bll.Edit(conStr, model);

            if (isAdd && isEdit)
            {
                List<zNodes> result = new List<zNodes>();
                zNodes nodes = new zNodes();
                nodes.Id = model.Id;
                nodes.path = model.nodePath;
                nodes.name = model.name;
                c_Count counts = bll.GetNodesCountByParent(conStr, nodes.Id);
                nodes.count = counts.count;
                nodes.open = false;
                nodes.parentId = model.parentId;
                nodes.icon = "/Images/treeIcon/1_open.png";
                nodes.iconOpen = "/Images/treeIcon/1_open.png";
                nodes.iconClose = "/Images/treeIcon/1_close.png";
                nodes.isElectric = model.isElectric;
                nodes.isLine = model.isLine;
                nodes.isJoint = model.isJoint;
                nodes.CreateTime = model.CreateTime.ToString("yyyy-MM-dd HH:mm:ss");
                nodes.children = new List<zNodes>();
                result.Add(nodes);
                uniteModel<zNodes> models = new uniteModel<zNodes>
                {
                    code = 0,
                    msg = "添加根节点成功!",
                    count = 1,
                    data = result
                };

                return Json(models, JsonRequestBehavior.AllowGet);
            }
            else
            {
                uniteModel<zNodes> models = new uniteModel<zNodes>
                {
                    code = 1,
                    msg = "添加根节点失败!",
                    count = 0,
                    data = null
                };
                return Json(models, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 添加同级节点
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult addSameNode(string organizeModel, string manager)
        {
            t_userInfo loginUser = CookieUserHelper.getCookieUser();
            bool authority = false;
            if (loginUser.roleId == 1)
            {
                authority = true;
            }
            else
            {
                authority = rolePowerbll.GetUserRolePower(conStr, (long)loginUser.roleId, (long)PowerEnum.addOrganize);
            }
            if (authority == false)
            {
                uniteModel<zNodes> models = new uniteModel<zNodes>
                {
                    code = 1,
                    msg = "添加同级节点失败!,无权限",
                    count = 1,
                    data = null
                };
                return Json(models, JsonRequestBehavior.AllowGet);
            }
            t_organize model = JsonHelper.JsonDeserialize<t_organize>(organizeModel);
            t_dataBaseManager managerModel = JsonHelper.JsonDeserialize<t_dataBaseManager>(manager);
            model.CreateTime = DateTime.Now;
            bool isAdd = false;
            bool isEdit = false;
            bool isBaseAdd = false;
            if (model.parentId == 0)
            {
                isAdd = bll.Add(conStr, model);
                model.nodePath = model.Id + "\\";
                isEdit = bll.Edit(conStr, model);
                managerModel.attributeElectricId = model.parentId;
                managerModel.attributeLineId = model.Id;
                managerModel.CreateTime = DateTime.Now;
                if (model.isLine)
                {
                    isBaseAdd = basebll.Add(conStr, managerModel);
                    if (isAdd && isEdit && isBaseAdd)
                    {
                        List<zNodes> result = new List<zNodes>();
                        zNodes nodes = new zNodes();
                        nodes.Id = model.Id;
                        nodes.path = model.nodePath;
                        nodes.name = model.name;
                        c_Count counts = bll.GetNodesCountByParent(conStr, nodes.Id);
                        nodes.count = counts.count;
                        nodes.open = false;
                        nodes.parentId = model.parentId;
                        nodes.icon = "/Images/treeIcon/1_open.png";
                        nodes.iconOpen = "/Images/treeIcon/1_open.png";
                        nodes.iconClose = "/Images/treeIcon/1_close.png";
                        nodes.isElectric = model.isElectric;
                        nodes.isLine = model.isLine;
                        nodes.isJoint = model.isJoint;
                        nodes.CreateTime = model.CreateTime.ToString("yyyy-MM-dd HH:mm:ss");
                        nodes.children = new List<zNodes>();
                        result.Add(nodes);
                        uniteModel<zNodes> models = new uniteModel<zNodes>
                        {
                            code = 0,
                            msg = "添加同级节点成功!",
                            count = 1,
                            data = result
                        };

                        return Json(models, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        uniteModel<zNodes> models = new uniteModel<zNodes>
                        {
                            code = 1,
                            msg = "添加同级节点失败!",
                            count = 0,
                            data = null
                        };
                        return Json(models, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    if (isAdd && isEdit)
                    {
                        List<zNodes> result = new List<zNodes>();
                        zNodes nodes = new zNodes();
                        nodes.Id = model.Id;
                        nodes.path = model.nodePath;
                        nodes.name = model.name;
                        c_Count counts = bll.GetNodesCountByParent(conStr, nodes.Id);
                        nodes.count = counts.count;
                        nodes.open = false;
                        nodes.parentId = model.parentId;
                        nodes.icon = "/Images/treeIcon/1_open.png";
                        nodes.iconOpen = "/Images/treeIcon/1_open.png";
                        nodes.iconClose = "/Images/treeIcon/1_close.png";
                        nodes.isElectric = model.isElectric;
                        nodes.isLine = model.isLine;
                        nodes.isJoint = model.isJoint;
                        nodes.CreateTime = model.CreateTime.ToString("yyyy-MM-dd HH:mm:ss");
                        nodes.children = new List<zNodes>();
                        result.Add(nodes);
                        uniteModel<zNodes> models = new uniteModel<zNodes>
                        {
                            code = 0,
                            msg = "添加同级节点成功!",
                            count = 1,
                            data = result
                        };

                        return Json(models, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        uniteModel<zNodes> models = new uniteModel<zNodes>
                        {
                            code = 1,
                            msg = "添加同级节点失败!",
                            count = 0,
                            data = null
                        };
                        return Json(models, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            else
            {
                t_organize singleMoldel = bll.getSingle(conStr, Convert.ToInt32(model.parentId));
                model.nodePath = singleMoldel.nodePath;
                isAdd = bll.Add(conStr, model);
                model.nodePath = model.nodePath + model.Id + "\\";
                isEdit = bll.Edit(conStr, model);
                managerModel.attributeElectricId = model.parentId;
                managerModel.attributeLineId = model.Id;
                managerModel.CreateTime = DateTime.Now;
                if (model.isLine)
                {
                    isBaseAdd = basebll.Add(conStr, managerModel);
                    if (isAdd && isEdit && isBaseAdd)
                    {
                        List<zNodes> result = new List<zNodes>();
                        zNodes nodes = new zNodes();
                        nodes.Id = model.Id;
                        nodes.path = model.nodePath;
                        nodes.name = model.name;
                        c_Count counts = bll.GetNodesCountByParent(conStr, nodes.Id);
                        nodes.count = counts.count;
                        nodes.open = false;
                        nodes.parentId = model.parentId;
                        nodes.icon = "/Images/treeIcon/1_open.png";
                        nodes.iconOpen = "/Images/treeIcon/1_open.png";
                        nodes.iconClose = "/Images/treeIcon/1_close.png";
                        nodes.isElectric = model.isElectric;
                        nodes.isLine = model.isLine;
                        nodes.isJoint = model.isJoint;
                        nodes.CreateTime = model.CreateTime.ToString("yyyy-MM-dd HH:mm:ss");
                        nodes.children = new List<zNodes>();
                        result.Add(nodes);
                        uniteModel<zNodes> models = new uniteModel<zNodes>
                        {
                            code = 0,
                            msg = "添加同级节点成功!",
                            count = 1,
                            data = result
                        };

                        return Json(models, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        uniteModel<t_organize> models = new uniteModel<t_organize>
                        {
                            code = 1,
                            msg = "添加同级节点失败!",
                            count = 0,
                            data = null
                        };
                        return Json(models, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    if (isAdd && isEdit)
                    {
                        List<zNodes> result = new List<zNodes>();
                        zNodes nodes = new zNodes();
                        nodes.Id = model.Id;
                        nodes.path = model.nodePath;
                        nodes.name = model.name;
                        c_Count counts = bll.GetNodesCountByParent(conStr, nodes.Id);
                        nodes.count = counts.count;
                        nodes.open = false;
                        nodes.parentId = model.parentId;
                        nodes.icon = "/Images/treeIcon/1_open.png";
                        nodes.iconOpen = "/Images/treeIcon/1_open.png";
                        nodes.iconClose = "/Images/treeIcon/1_close.png";
                        nodes.isElectric = model.isElectric;
                        nodes.isLine = model.isLine;
                        nodes.isJoint = model.isJoint;
                        nodes.CreateTime = model.CreateTime.ToString("yyyy-MM-dd HH:mm:ss");
                        nodes.children = new List<zNodes>();
                        result.Add(nodes);
                        uniteModel<zNodes> models = new uniteModel<zNodes>
                        {
                            code = 0,
                            msg = "添加同级节点成功!",
                            count = 1,
                            data = result
                        };

                        return Json(models, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        uniteModel<zNodes> models = new uniteModel<zNodes>
                        {
                            code = 1,
                            msg = "添加同级节点失败!",
                            count = 0,
                            data = null
                        };
                        return Json(models, JsonRequestBehavior.AllowGet);
                    }
                }
            }
        }

        /// <summary>
        /// 添加子节点
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult addChildNode(string organizeModel, string manager)
        {
            t_userInfo loginUser = CookieUserHelper.getCookieUser();
            bool authority = false;
            if (loginUser.roleId == 1)
            {
                authority = true;
            }
            else
            {
                authority = rolePowerbll.GetUserRolePower(conStr, (long)loginUser.roleId, (long)PowerEnum.addOrganize);
            }
            if (authority == false)
            {
                uniteModel<zNodes> models = new uniteModel<zNodes>
                {
                    code = 1,
                    msg = "添加子级节点失败!,无权限",
                    count = 0,
                    data = null
                };
                return Json(models, JsonRequestBehavior.AllowGet);
            }
            t_organize model = JsonHelper.JsonDeserialize<t_organize>(organizeModel);
            t_dataBaseManager managerModel = JsonHelper.JsonDeserialize<t_dataBaseManager>(manager);
            model.CreateTime = DateTime.Now;
            bool isAdd = false;
            bool isEdit = false;
            bool isBaseAdd = false;
            t_organize singleMoldel = bll.getSingle(conStr, Convert.ToInt32(model.parentId));
            model.nodePath = singleMoldel.nodePath;
            model.CreateTime = DateTime.Now;
            isAdd = bll.Add(conStr, model);
            model.nodePath = model.nodePath + model.Id + "\\";
            isEdit = bll.Edit(conStr, model);
            managerModel.attributeElectricId = model.parentId;
            managerModel.attributeLineId = model.Id;
            managerModel.CreateTime = DateTime.Now;
            if (model.isLine)
            {
                isBaseAdd = basebll.Add(conStr, managerModel);
                if (isAdd && isEdit && isBaseAdd)
                {
                    List<zNodes> result = new List<zNodes>();
                    zNodes nodes = new zNodes();
                    nodes.Id = model.Id;
                    nodes.path = model.nodePath;
                    nodes.name = model.name;
                    c_Count counts = bll.GetNodesCountByParent(conStr, nodes.Id);
                    nodes.count = counts.count;
                    nodes.open = false;
                    nodes.parentId = model.parentId;
                    nodes.icon = "/Images/treeIcon/1_open.png";
                    nodes.iconOpen = "/Images/treeIcon/1_open.png";
                    nodes.iconClose = "/Images/treeIcon/1_close.png";
                    nodes.isElectric = model.isElectric;
                    nodes.isLine = model.isLine;
                    nodes.isJoint = model.isJoint;
                    nodes.CreateTime = model.CreateTime.ToString("yyyy-MM-dd HH:mm:ss");
                    nodes.children = new List<zNodes>();
                    result.Add(nodes);
                    uniteModel<zNodes> models = new uniteModel<zNodes>
                    {
                        code = 0,
                        msg = "添加子级节点成功!",
                        count = 1,
                        data = result
                    };

                    return Json(models, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    uniteModel<zNodes> models = new uniteModel<zNodes>
                    {
                        code = 1,
                        msg = "添加子级节点失败!",
                        count = 0,
                        data = null
                    };
                    return Json(models, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                if (isAdd && isEdit)
                {
                    List<zNodes> result = new List<zNodes>();
                    zNodes nodes = new zNodes();
                    nodes.Id = model.Id;
                    nodes.path = model.nodePath;
                    nodes.name = model.name;
                    c_Count counts = bll.GetNodesCountByParent(conStr, nodes.Id);
                    nodes.count = counts.count;
                    nodes.open = false;
                    nodes.parentId = model.parentId;
                    nodes.icon = "/Images/treeIcon/1_open.png";
                    nodes.iconOpen = "/Images/treeIcon/1_open.png";
                    nodes.iconClose = "/Images/treeIcon/1_close.png";
                    nodes.isElectric = model.isElectric;
                    nodes.isLine = model.isLine;
                    nodes.isJoint = model.isJoint;
                    nodes.CreateTime = model.CreateTime.ToString("yyyy-MM-dd HH:mm:ss");
                    nodes.children = new List<zNodes>();
                    result.Add(nodes);
                    uniteModel<zNodes> models = new uniteModel<zNodes>
                    {
                        code = 0,
                        msg = "添加子级节点成功!",
                        count = 1,
                        data = result
                    };

                    return Json(models, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    uniteModel<t_organize> models = new uniteModel<t_organize>
                    {
                        code = 1,
                        msg = "添加子级节点失败!",
                        count = 0,
                        data = null
                    };
                    return Json(models, JsonRequestBehavior.AllowGet);
                }
            }
        }

        /// <summary>
        /// 设备添加
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddDevice(t_Device model)
        {
            List<t_Device> list = devicebll.GetDeviceByTerId(conStr, model.TerminalId);
            if (list.Count > 0)
            {
                uniteModel<zNodes> models = new uniteModel<zNodes>
                {
                    code = 1,
                    msg = string.Format("已存在相同终端ID{0}!", model.TerminalId),
                    count = 0,
                    data = null
                };
                return Json(models, JsonRequestBehavior.AllowGet);
            }
            else
            {
                t_organize singleMoldel = bll.getSingle(conStr, Convert.ToInt32(model.LineId));
                string deviceId = "0";
                //AI防外破终端Id不用转
                if (model.bigTypeId != 11)
                {
                    deviceId = HexTo.HexToFloat(model.TerminalId);
                }
                model.deviceId = long.Parse(deviceId);
                model.ElectricId = singleMoldel.parentId;
                model.createTime = DateTime.Now;
                bool isAdd = devicebll.Add(conStr, model);
                model.nodePath = model.nodePath + model.Id + "\\";
                bool isEdit = devicebll.Update(conStr, model);
                if (isAdd && isEdit)
                {
                    List<zNodes> result = new List<zNodes>();
                    zNodes node = new zNodes();
                    node.Id = model.Id;
                    t_deviceBigType deviceBigType = bigTypebll.GetSingle(conStr, Convert.ToInt32(model.bigTypeId));
                    t_deviceSmallType deviceSmallType = smallTypebll.GetSingle(conStr, Convert.ToInt32(model.smallTypeId));
                    node.name = model.deviceName;
                    node.path = model.nodePath;
                    node.actionAddress = deviceSmallType.actionAddress;
                    node.icon = deviceBigType.IconUrl;
                    node.isDevice = true;
                    node.isElectric = false;
                    node.isLine = false;
                    node.isJoint = false;
                    node.TerminalId = model.TerminalId;
                    node.ElectricId = model.ElectricId;
                    node.LineId = model.LineId;
                    result.Add(node);
                    uniteModel<zNodes> models = new uniteModel<zNodes>
                    {
                        code = 0,
                        msg = "添加成功!",
                        count = 1,
                        data = result
                    };
                    return Json(models, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    uniteModel<zNodes> models = new uniteModel<zNodes>
                    {
                        code = 1,
                        msg = "添加失败!",
                        count = 0,
                        data = null
                    };
                    return Json(models, JsonRequestBehavior.AllowGet);
                }
            }
        }

        /// <summary>
        /// 获取设备大类
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetDeviceBigType()
        {
            List<t_deviceBigType> list = bigTypebll.GetAll(conStr);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取设备小类
        /// </summary>
        /// <param name="bigTypeId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetDeviceSmallType(int bigTypeId)
        {
            List<t_deviceSmallType> list = smallTypebll.GetByBigTypeId(conStr, bigTypeId);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 根据供电和线路获取设备
        /// </summary>
        /// <param name="ElectricId">供电Id</param>
        /// <param name="LineId">线路Id</param>
        /// <param name="count">获取数量</param>
        /// <returns></returns>
        public JsonResult GetDevice(int parentId, int pageIndex, int pageSize)
        {
            int totalcount = 0;
            List<t_Device> list = devicebll.GetListPage(conStr, parentId, pageIndex, pageSize, out totalcount, null);
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
        /// 组织架构重命名
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Name"></param>
        /// <param name="isDevice"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual JsonResult ReNameNode(string TerminalId, string Name, bool isDevice,long Id)
        {
            t_userInfo loginUser = CookieUserHelper.getCookieUser();
            bool authority = false;
            if (isDevice)
            {
                if (loginUser.roleId == 1)
                {
                    authority = true;
                }
                else
                {
                    authority = rolePowerbll.GetUserRolePower(conStr, (long)loginUser.roleId, (long)PowerEnum.updataDevice);
                }
                if (authority == false)
                {
                    uniteModel<t_Device> models = new uniteModel<t_Device>
                    {
                        code = 1,
                        msg = "设备重命名失败!,无权限",
                        count = 0,
                        data = null
                    };
                    return Json(models, JsonRequestBehavior.AllowGet);
                }
                List<t_Device> device = devicebll.GetDeviceByTerId(conStr, TerminalId);
                int number = devicebll.ReNameDevice(conStr, Name, device[0].Id);
                if (number > 0)
                {
                    uniteModel<t_Device> model = new uniteModel<t_Device>
                    {
                        code = 0,
                        msg = "设备重命名成功!",
                        count = number,
                        data = null
                    };
                    return Json(model, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    uniteModel<t_Device> model = new uniteModel<t_Device>
                    {
                        code = 1,
                        msg = "设备重命名失败!",
                        count = number,
                        data = null
                    };
                    return Json(model, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                if (loginUser.roleId == 1)
                {
                    authority = true;
                }
                else
                {
                    authority = rolePowerbll.GetUserRolePower(conStr, (long)loginUser.roleId, (long)PowerEnum.updateOrganize);
                }
                if (authority == false)
                {
                    uniteModel<t_organize> models = new uniteModel<t_organize>
                    {
                        code = 1,
                        msg = "节点重命名失败!,无权限",
                        count = 0,
                        data = null
                    };
                    return Json(models, JsonRequestBehavior.AllowGet);
                }
                int number = bll.ReNameOrganize(conStr, Name, Id);
                if (number > 0)
                {
                    uniteModel<t_organize> model = new uniteModel<t_organize>
                    {
                        code = 0,
                        msg = "节点重命名成功!",
                        count = number,
                        data = null
                    };
                    return Json(model, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    uniteModel<t_organize> model = new uniteModel<t_organize>
                    {
                        code = 1,
                        msg = "节点重命名失败!",
                        count = number,
                        data = null
                    };
                    return Json(model, JsonRequestBehavior.AllowGet);
                }
            }
        }

        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="isDevice"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual JsonResult DelNode(long Id, bool isDevice, string nodePath)
        {
            t_userInfo loginUser = CookieUserHelper.getCookieUser();
            bool authority = false;
            if (isDevice)
            {
                if (loginUser.roleId == 1)
                {
                    authority = true;
                }
                else
                {
                    authority = rolePowerbll.GetUserRolePower(conStr, (long)loginUser.roleId, (long)PowerEnum.delDevice);
                }
                if (authority == false)
                {
                    uniteModel<t_Device> models = new uniteModel<t_Device>
                    {
                        code = 1,
                        msg = "删除设备失败!,无权限",
                        count = 0,
                        data = null
                    };
                    return Json(models, JsonRequestBehavior.AllowGet);
                }
                t_Device modelDevice = devicebll.GetSingle(conStr, (int)Id);
                bool DelStatus = devicebll.Delete(conStr, modelDevice, loginUser.Id);
                if (DelStatus)
                {
                    uniteModel<t_Device> model = new uniteModel<t_Device>
                    {
                        code = 0,
                        msg = "删除设备成功!",
                        count = 1,
                        data = null
                    };
                    return Json(model, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    uniteModel<t_Device> model = new uniteModel<t_Device>
                    {
                        code = 1,
                        msg = "删除设备失败!",
                        count = 0,
                        data = null
                    };
                    return Json(model, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                if (loginUser.roleId == 1)
                {
                    authority = true;
                }
                else
                {
                    authority = rolePowerbll.GetUserRolePower(conStr, (long)loginUser.roleId, (long)PowerEnum.delOrganize);
                }
                if (authority == false)
                {
                    uniteModel<t_organize> models = new uniteModel<t_organize>
                    {
                        code = 1,
                        msg = "删除节点失败!,无权限",
                        count = 0,
                        data = null
                    };
                    return Json(models, JsonRequestBehavior.AllowGet);
                }
                List<t_organize> list = bll.GetOrganizeByPath(conStr, nodePath);
                foreach (var item in list)
                {
                    t_dataBaseManager databaseModel = basebll.GetModel(conStr, (int)item.parentId, (int)item.Id);
                    basebll.DeleteDataBaseManager(conStr, databaseModel, loginUser.Id);
                }
                int delOrganNum = bll.DeleteOrganize(conStr, nodePath, loginUser.Id);
                int delDeviceNum = devicebll.DeleteDeivceByPath(conStr, nodePath, loginUser.Id);
                if (delOrganNum > 0 || delOrganNum > 0)
                {
                    uniteModel<t_organize> model = new uniteModel<t_organize>
                    {
                        code = 0,
                        msg = "删除节点成功!",
                        count = 0,
                        data = null
                    };
                    return Json(model, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    uniteModel<t_organize> model = new uniteModel<t_organize>
                    {
                        code = 1,
                        msg = "删除节点失败!",
                        count = 0,
                        data = null
                    };
                    return Json(model, JsonRequestBehavior.AllowGet);
                }
            }
        }

        /// <summary>
        /// 根据Id获取单个组织结构
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual JsonResult GetOrganizeSingle(int Id)
        {
            t_organize model=  bll.getSingle(conStr, Id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取树形
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public virtual JsonResult GetOrganizeTree()
        {
            List<nodes> list = bll.getAllNodeList(conStr);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}