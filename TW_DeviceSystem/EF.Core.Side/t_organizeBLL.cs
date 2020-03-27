using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EF.Component.Data.EFHelper;
using EF.Application.Model;
using System.Data.SqlClient;
using EF.Application.Model.Custom;
using System.Linq.Expressions;
using EF.Application.Model.zTree;

namespace EF.Core.Side
{
    /// <summary>
    /// 组织架构
    /// </summary>
    public class t_organizeBLL
    {
        /// <summary>
        /// 添加组织架构
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual bool Add(string conStr, t_organize model)
        {
            BaseDAL dal = new BaseDAL(conStr);
            return dal.Add(model);
        }

        /// <summary>
        /// 修改组织架构
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual bool Edit(string conStr, t_organize model)
        {
            BaseDAL dal = new BaseDAL(conStr);
            return dal.Update(model);
        }

        /// <summary>
        /// 根据父节点Id获取子节点
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public virtual List<t_organize> GetNodesByParent(string conStr, long? parentId,int? pageIndex,int? pageSize)
        {
            BaseDAL dal = new BaseDAL(conStr);
            if (pageIndex.HasValue && pageSize.HasValue && pageSize > 0)
            {
                pageIndex = (pageIndex - 1) * pageSize;
                string sql = "select top " + pageSize + " o.* from (select row_number() over(order by orderNo) as rownumber,* from t_organize) as o where o.isDel=0 and o.parentId=" + parentId + " and rownumber>" + pageIndex + "";
                List<t_organize> list = dal.QueryList<t_organize>(sql, null);
                return list;
            }
            else
            {
                string sql = "select * from t_organize where parentId=@parentId and isDel=0 order by orderNo asc";
                SqlParameter[] paras = new SqlParameter[]
                {
                   new SqlParameter("@parentId",parentId),
                };
                List<t_organize> list = dal.QueryList<t_organize>(sql, paras);
                return list;
            }
        }


        public virtual List<t_organize> GetNodesByParent(string conStr, long Id)
        {
            BaseDAL dal = new BaseDAL(conStr);
            OrderModelField[] order = new OrderModelField[]
            {
                new OrderModelField {IsDESC=false,PropertyName="orderNo" }
            };
            List<t_organize> list = dal.GetList<t_organize>(t => t.parentId == Id && t.isDel == false, order);
            return list;
        }

        /// <summary>
        /// 获取树形数据
        /// </summary>
        /// <param name="conStr"></param>
        /// <returns></returns>
        public virtual List<nodes> getAllNodeList(string conStr)
        {
            BaseDAL dal = new BaseDAL(conStr);
            string sql = "select CAST(-2 as bigint) as dId,Id,name,CAST('/Images/treeIcon/1_open.png' as nvarchar(50)) as icon,CAST('/Images/treeIcon/1_open.png' as nvarchar(50)) as iconOpen,CAST('/Images/treeIcon/1_close.png' as nvarchar(50)) as iconClose,nodePath as path,CAST('' as nvarchar(max)) as actionAddress,parentId,orderNo,isElectric,isLine,isJoint,CAST(0 as bit) as isDevice,CreateTime,CAST(null as nvarchar(max)) as TerminalId,CAST(0 as bigint) as ElectricId,CAST(0 as bigint) as LineId from t_organize where isDel = 0 union all select a.Id as dId ,CAST(-2 as bigint) as Id, deviceName as name, b.IconUrl as icon, CAST('' as nvarchar(50)) as iconOpen,CAST('' as nvarchar(50)) as iconClose, nodePath as path, c.actionAddress, parentId, orderNo, CAST(0 as bit) as isElectric, CAST(0 as bit) as isLine,CAST(0 as bit) as isJoint, CAST(1 as bit) as isDevice,a.createTime as CreateTime, a.TerminalId, a.ElectricId, a.LineId from t_Device as a inner join t_deviceBigType as b on a.bigTypeId = b.Id inner join t_deviceSmallType as c on a.smallTypeId = c.Id where a.isDel = 0 order by orderNo asc";
            List<nodes> list = dal.QueryList<nodes>(sql, null);
            return list;
        }

        /// <summary>
        /// 获取权限分配树
        /// </summary>
        /// <param name="conStr"></param>
        /// <returns></returns>
        public virtual List<nodes> getPowerNodes(string conStr)
        {
            BaseDAL dal = new BaseDAL(conStr);
            string sql = "select Id,name,CAST('/Images/treeIcon/1_open.png' as nvarchar(50)) as icon,CAST('/Images/treeIcon/1_open.png' as nvarchar(50)) as iconOpen,CAST('/Images/treeIcon/1_close.png' as nvarchar(50)) as iconClose,nodePath as path,CAST('' as nvarchar(max)) as actionAddress,parentId,orderNo,isElectric,isLine,isJoint,CAST(0 as bit) as isDevice,CreateTime,CAST(null as nvarchar(max)) as TerminalId,CAST(0 as bigint) as ElectricId,CAST(0 as bigint) as LineId from t_organize where isDel = 0 order by orderNo asc ";
            List<nodes> list = dal.QueryList<nodes>(sql, null);
            return list;
        }

        public virtual List<nodes> getNodeByUser(string conStr,long? userId)
        {
            BaseDAL dal = new BaseDAL(conStr);
            string sql = "select CAST(-2 as bigint) as dId,b.Id,name,CAST('/Images/treeIcon/1_open.png' as nvarchar(50)) as icon,CAST('/Images/treeIcon/1_open.png' as nvarchar(50)) as iconOpen,CAST('/Images/treeIcon/1_close.png' as nvarchar(50)) as iconClose,nodePath as path,CAST('' as nvarchar(max)) as actionAddress,parentId,orderNo,isElectric,isLine,isJoint,CAST(0 as bit) as isDevice,b.CreateTime,CAST(null as nvarchar(max)) as TerminalId,CAST(0 as bigint) as ElectricId,CAST(0 as bigint) as LineId from t_organizeDistribute as a inner join t_organize as b on a.nodeId = b.Id where b.isDel = 0 and a.userId =" + userId + " union all select c.Id as dId, CAST(-2 as bigint) as Id,deviceName as name,d.IconUrl as icon, CAST('' as nvarchar(50)) as iconOpen,CAST('' as nvarchar(50)) as iconClose,nodePath as path, e.actionAddress, parentId, orderNo,CAST(0 as bit) as isElectric, CAST(0 as bit) as isLine,CAST(0 as bit) as isJoint, CAST(1 as bit) as isDevice,c.createTime as CreateTime, c.TerminalId, c.ElectricId, c.LineId from t_Device as c inner join t_deviceBigType as d on c.bigTypeId = d.Id inner join t_deviceSmallType as e on c.smallTypeId = e.Id where c.isDel = 0 order by orderNo asc";
            List<nodes> list = dal.QueryList<nodes>(sql, null);
            return list;
        }

        /// <summary>
        /// 根据父节点Id获取子节点数量
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public virtual c_Count GetNodesCountByParent(string conStr, long? parentId)
        {
            BaseDAL dal = new BaseDAL(conStr);
            string sql = "select count(0) as count from t_organize where parentId=@parentId and isDel=0";
            SqlParameter[] paras = new SqlParameter[]
           {
                new SqlParameter("@parentId",parentId)
           };
            c_Count model = dal.QuerySingleOrDefault<c_Count>(sql, paras);
            return model;
        }

        public virtual c_Count GetNodesCountByUser(string conStr,long? parentId,long? userId)
        {
            BaseDAL dal = new BaseDAL(conStr);
            string sql = "select count(0) as count from t_organizeDistribute as b inner join t_organize as a on b.nodeId=a.Id where parentId=@parentId and isDel=0 and b.userId=@userId";
            SqlParameter[] paras = new SqlParameter[]
           {
                new SqlParameter("@parentId",parentId),
                new SqlParameter("@userId",userId)
           };
            c_Count model = dal.QuerySingleOrDefault<c_Count>(sql, paras);
            return model;
        }

        /// <summary>
        /// 组织架构单个查询
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public virtual t_organize getSingle(string conStr, int Id)
        {
            BaseDAL dal = new BaseDAL(conStr);
            t_organize model = dal.GetSingleById<t_organize>(Id);
            return model;
        }

        /// <summary>
        /// 查询所有供电
        /// </summary>
        /// <param name="conStr"></param>
        /// <returns></returns>
        public virtual List<t_organize> GetAllElectric(string conStr)
        {
            BaseDAL dal = new BaseDAL(conStr);
            List<t_organize> list = dal.GetList<t_organize>(t => t.isElectric == true && t.isDel==false);
            return list;
        }

        /// <summary>
        /// 根据用户获取所在供电或公司
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public virtual List<t_organize> GetAllElectric(string conStr, long userId)
        {
            BaseDAL dal = new BaseDAL(conStr);
            string sql = "select a.* from t_organizeDistribute as b inner join t_organize as a on b.nodeId=a.Id where b.userId=@userId and a.isElectric=1 and a.isDel=0";
            SqlParameter[] paras = new SqlParameter[]
            {
                new SqlParameter("@userId",userId)
            };
            return dal.QueryList<t_organize>(sql, paras);
        }

        /// <summary>
        /// 查询线路根据供电Id
        /// </summary>
        /// <param name="conStr"></param>
        /// <returns></returns>
        public virtual List<t_organize> GetAllLine(string conStr, long parentId)
        {
            BaseDAL dal = new BaseDAL(conStr);
            List<t_organize> list = dal.GetList<t_organize>(t => t.parentId == parentId && t.isDel==false);
            return list;
        }

        /// <summary>
        /// 根据用户和父节点获取线路
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="parentId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public virtual List<t_organize> GetAllLine(string conStr, long parentId,long userId)
        {
            BaseDAL dal = new BaseDAL(conStr);
            string sql = "select a.* from t_organizeDistribute as b inner join t_organize as a on b.nodeId=a.Id where b.userId=@userId and a.isLine=1 and a.parentId=@parentId and a.isDel=0";
            SqlParameter[] paras = new SqlParameter[]
            {
                new SqlParameter("@userId",userId),
                new SqlParameter("@parentId",parentId)
            };
            List<t_organize> list = dal.QueryList<t_organize>(sql, paras);
            return list;
        }

        /// <summary>
        /// 查询接头根据线路Id
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public virtual List<t_organize> GetAllJoint(string conStr,long parentId)
        {
            BaseDAL dal = new BaseDAL(conStr);
            List<t_organize> list = dal.GetList<t_organize>(t => t.parentId == parentId && t.isDel==false);
            return list;
        }

        /// <summary>
        /// 查询接头根据父节点Id和用户Id
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="parentId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public virtual List<t_organize> GetAllJoint(string conStr, long parentId,long userId)
        {
            BaseDAL dal = new BaseDAL(conStr);
            string sql = "select a.* from t_organizeDistribute as b inner join t_organize as a on b.nodeId=a.Id where b.userId=@userId and a.isJoint=1 and a.parentId=@parentId and a.isDel=0";
            SqlParameter[] paras = new SqlParameter[]
            {
                  new SqlParameter("@userId",userId),
                new SqlParameter("@parentId",parentId)
            };
            List<t_organize> list = dal.QueryList<t_organize>(sql, paras);
            return list;
        }

        /// <summary>
        /// 组织架构重命名
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="name"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public virtual int ReNameOrganize(string conStr, string name, long Id)
        {
            BaseDAL dal = new BaseDAL(conStr);
            string sql = "update t_organize set name=@name where Id=@Id";
            SqlParameter[] paras = new SqlParameter[]
            {
                new SqlParameter("@name",name),
                new SqlParameter("@Id",Id)
            };
            int number = dal.ExecuteSql(sql, paras);
            return number;
        }

        /// <summary>
        /// 删除组织结构
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public virtual int DeleteOrganize(string conStr, string nodePath, long DelUser)
        {
            BaseDAL dal = new BaseDAL(conStr);
            string sql = "update t_organize set isDel=1,DelUser=@DelUser,DelTime=@DelTime where nodePath like '%" + nodePath + "%'";
            SqlParameter[] paras = new SqlParameter[]
            {
                new SqlParameter("@DelUser",DelUser),
                new SqlParameter("@DelTime",DateTime.Now)
            };
            int delNum = dal.ExecuteSql(sql, paras);
            return delNum;
        }

        /// <summary>
        /// 根据用户权限获取用户树形
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="userId"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public virtual List<t_organize> GetNodeByUser(string conStr, long userId, long parentId,int? pageIndex,int? pageSize)
        {
            BaseDAL dal = new BaseDAL(conStr);
            if(pageIndex.HasValue && pageSize.HasValue)
            {
                pageIndex = ((pageIndex - 1) * pageSize);
                string sql = "select top " + pageSize + " b.* from (select row_number() over(order by b.orderNo) as rownumber,b.* from t_organizeDistribute as a inner join t_organize as b on a.nodeId=b.Id where a.userId=" + userId + ") as b where b.isDel = 0 and b.parentId=" + parentId + " and rownumber>" + pageIndex + "";
                List<t_organize> list = dal.QueryList<t_organize>(sql, null);
                return list;
            }
            else
            {
                string sql = "select b.* from t_organizeDistribute as a inner join t_organize as b on a.nodeId=b.Id where a.userId = @userId and b.parentId = @parentId and isDel=0 order by b.orderNo asc";
                SqlParameter[] paras = new SqlParameter[]
                {
                    new SqlParameter("@userId",userId),
                    new SqlParameter("@parentId",parentId)
                };
                List<t_organize> list = dal.QueryList<t_organize>(sql, paras);
                return list;
            }
        }

        /// <summary>
        /// 根据用户权限获取用户树节点数量
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="userId"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public virtual c_Count GetNodesCountByUserParentId(string conStr, long? userId, long? parentId)
        {
            BaseDAL dal = new BaseDAL(conStr);
            string sql = "select COUNT(0) as count from t_organizeDistribute as a inner join t_organize as b on a.nodeId=b.Id where a.userId = @userId and b.parentId = @parentId and isDel=0";
            SqlParameter[] paras = new SqlParameter[]
            {
                new SqlParameter("@userId",userId),
                new SqlParameter("@parentId",parentId)
            };
            c_Count model = dal.QuerySingleOrDefault<c_Count>(sql, paras);
            return model;
        }

        /// <summary>
        /// 根据用户获取用户结构树
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public virtual List<t_organize> GetOrganizeByUser(string conStr,long? userId, long? parentId,int? pageIndex,int? pageSize)
        {
            BaseDAL dal = new BaseDAL(conStr);
            if(pageIndex.HasValue && pageSize.HasValue)
            {
                pageIndex = ((pageIndex - 1) * pageSize);
                string sql = "select top " + pageSize + " b.* from (select row_number() over(order by b.orderNo) as rownumber,b.* from t_organizeDistribute as a inner join t_organize as b on a.nodeId=b.Id where a.userId=" + userId + ") as b where b.isDel = 0 and b.parentId=" + parentId + " and rownumber>" + pageIndex + "";
                var List = dal.QueryList<t_organize>(sql, null);
                return List;
            }
            else
            {
                string sql = "select a.* from t_organizeDistribute as b inner join t_organize as a on b.nodeId=a.Id where b.userId=@userId and a.parentId=@parentId and a.isDel=0 order by orderNo asc";
                SqlParameter[] paras = new SqlParameter[]
                {
                    new SqlParameter("@userId",userId),
                    new SqlParameter("@parentId",parentId),
                };
                var List = dal.QueryList<t_organize>(sql, paras);
                return List;
            }
        }

        /// <summary>
        /// 根据用户获取用户结构树
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="userId"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public virtual c_Count GetOrganizeCountByUser(string conStr,long? userId,long? parentId)
        {
            BaseDAL dal = new BaseDAL(conStr);
            string sql = "select COUNT(0) as count from t_organizeDistribute as b inner join t_organize as a on b.nodeId=a.Id where b.userId=@userId and a.parentId=@parentId and isDel=0";
            SqlParameter[] paras = new SqlParameter[]
            {
                new SqlParameter("@userId",userId),
                new SqlParameter("@parentId",parentId)
            };
            c_Count model = dal.QuerySingleOrDefault<c_Count>(sql, paras);
            return model;
        }

        /// <summary>
        /// 根据路径获取线路
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public virtual List<t_organize> GetOrganizeByPath(string conStr,string path)
        {
            BaseDAL dal = new BaseDAL(conStr);
            List<t_organize> list = dal.GetList<t_organize>(t =>t.isDel==false && t.nodePath.Contains(path) && t.isLine==true);
            return list;
        }

        /// <summary>
        /// 根据用户查询用户所在线路
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public virtual List<t_organize> GetUserLine(string conStr,long? userId)
        {
            BaseDAL dal = new BaseDAL(conStr);
            string sql = "select a.* from t_organizeDistribute as b inner join t_organize as a on b.nodeId=a.Id and isLine=1 and b.userId=@userId";
            SqlParameter[] paras = new SqlParameter[]
            {
                new SqlParameter("@userId",userId)
            };
            List<t_organize> list = dal.QueryList<t_organize>(sql, paras);
            return list;
        }

        /// <summary>
        /// 组织架构列表分页
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="searchText"></param>
        /// <returns></returns>
        public virtual List<t_organize> GetListPage(string conStr,int pageIndex,int pageSize,out int totalCount,string searchText)
        {
            BaseDAL dal = new BaseDAL(conStr);
            OrderModelField[] order = new OrderModelField[]
            {
                new OrderModelField {IsDESC=false,PropertyName="Id" }
            };
            Expression<Func<t_organize, bool>> seleWhere;
            if (!string.IsNullOrEmpty(searchText))
            {
                seleWhere = t => t.isDel == false && t.name.Contains(searchText);
                List<t_organize> list = dal.GetListPaged<t_organize>(pageIndex, pageSize, seleWhere, out totalCount, order);
                return list;
            }
            else
            {
                seleWhere = t => t.isDel == false;
                List<t_organize> list = dal.GetListPaged<t_organize>(pageIndex, pageSize, seleWhere, out totalCount, order);
                return list;
            }
            
        }

        /// <summary>
        /// 获取已删除组织架构列表
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="searchText"></param>
        /// <returns></returns>
        public virtual List<t_organize> GetDelListPage(string conStr,int pageIndex,int pageSize,out int totalCount,string searchText)
        {
            BaseDAL dal = new BaseDAL(conStr);
            Expression<Func<t_organize, bool>> seleWhere;
            OrderModelField[] order = new OrderModelField[]
            {
                new OrderModelField { IsDESC=true,PropertyName="DelTime"}
            };
            if (string.IsNullOrEmpty(searchText))
            {
                seleWhere = t => t.isDel == true;
                return dal.GetListPaged<t_organize>(pageIndex, pageSize, seleWhere, out totalCount, order);
            }
            else
            {
                seleWhere = t => t.isDel == true && t.name.Contains(searchText);
                return dal.GetListPaged<t_organize>(pageIndex, pageSize, seleWhere, out totalCount, order);
            }
        }

        /// <summary>
        /// 组织架构还原
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public virtual bool UpdataDel(string conStr, int Id)
        {
            BaseDAL dal = new BaseDAL(conStr);
            t_organize model = dal.GetSingleById<t_organize>(Id);
            model.isDel = false;
            return dal.Update(model);
        }
    }
}
