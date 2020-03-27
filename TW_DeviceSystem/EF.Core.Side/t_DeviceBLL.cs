using EF.Application.Model;
using EF.Application.Model.Custom;
using EF.Component.Data.EFHelper;
using EF.Component.Data.SQLHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EF.Core.Side
{
    /// <summary>
    /// 设备列表
    /// </summary>
    public class t_DeviceBLL
    {

        /// <summary>
        /// 添加设备
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual bool Add(string conStr, t_Device model)
        {
            BaseDAL dal = new BaseDAL(conStr);
            return dal.Add(model);
        }

        /// <summary>
        /// 修改设备
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual bool Update(string conStr, t_Device model)
        {
            BaseDAL dal = new BaseDAL(conStr);
            return dal.Update(model);
        }

        /// <summary>
        /// 删除设备
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual bool Delete(string conStr, t_Device model,long DelUser)
        {
            BaseDAL dal = new BaseDAL(conStr);
            model.isDel = true;
            model.DelTime = DateTime.Now;
            model.DelUser = DelUser;
            return dal.Update(model);
        }

        /// <summary>
        /// 获取单个设备
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public virtual t_Device GetSingle(string conStr, int Id)
        {
            BaseDAL dal = new BaseDAL(conStr);
            return dal.GetSingleById<t_Device>(Id);
        }

        /// <summary>
        /// 获取设备分页列表
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="path"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalcount"></param>
        /// <returns></returns>
        public virtual List<c_Device> GetListPage(string conStr, long? userId, int pageIndex, int pageSize, out int totalcount, int? ElectricId, int? LineId, int? JointId, int? bigTypeId, int? smallTypeId, string searchText,int? isError, int? isOnline)
        {
            BaseDAL dal = new BaseDAL(conStr);
            totalcount = GetListPageCount(conStr, userId, ElectricId, LineId, JointId, bigTypeId, smallTypeId, searchText, isError, isOnline);
            StringBuilder sqlStr = new StringBuilder();
            if (userId.HasValue)
            {
                sqlStr.Append("select top " + pageSize + " * from (select ROW_NUMBER() over (order by a.Id asc) as rownumber,a.*,b.typeName as bigTypeName,c.typeName as smalltypeName,d.name as ElectricName, e.name as LineName,f.name as JointName from t_Device as a inner join t_deviceBigType as b on a.bigTypeId = b.Id inner join t_deviceSmallType as c on a.smallTypeId = c.Id inner join t_organize as d on a.ElectricId = d.Id inner join t_organize as e on a.LineId = e.Id inner join t_organize as f on a.parentId=f.Id inner join t_organizeDistribute as g on a.parentId=g.nodeId where a.isDel=0 and g.userId=" + userId + "");
            }
            else
            {
                sqlStr.Append("select top " + pageSize + " * from (select ROW_NUMBER() over (order by a.Id asc) as rownumber,a.*,b.typeName as bigTypeName,c.typeName as smalltypeName,d.name as ElectricName, e.name as LineName,f.name as JointName from t_Device as a inner join t_deviceBigType as b on a.bigTypeId = b.Id inner join t_deviceSmallType as c on a.smallTypeId = c.Id inner join t_organize as d on a.ElectricId = d.Id inner join t_organize as e on a.LineId = e.Id inner join t_organize as f on a.parentId=f.Id where a.isDel=0");
            }
            if (ElectricId.HasValue)
            {
                sqlStr.Append(" and a.ElectricId=" + ElectricId + "");
            }
            if (LineId.HasValue)
            {
                sqlStr.Append(" and a.LineId=" + LineId + "");
            }
            if (JointId.HasValue)
            {
                sqlStr.Append(" and a.parentId=" + JointId + "");
            }
            if (bigTypeId.HasValue)
            {
                sqlStr.Append(" and a.bigTypeId=" + bigTypeId + "");
            }
            if (smallTypeId.HasValue)
            {
                sqlStr.Append(" and a.smallTypeId=" + smallTypeId + "");
            }
            if (isError.HasValue)
            {
                sqlStr.Append(" and a.isError=" + isError + "");
            }
            if (isOnline.HasValue)
            {
                sqlStr.Append(" and a.isOnline=" + isOnline + "");
            }
            if (!string.IsNullOrEmpty(searchText))
            {
                sqlStr.Append("and (a.deviceName like '%" + searchText + "%'");
                sqlStr.Append(" or a.TerminalId like '%" + searchText + "%')");
            }
            sqlStr.Append(")temp_row where rownumber>((" + pageIndex + "-1)*" + pageSize + ")");
            string sql = sqlStr.ToString();
            List<c_Device> list = dal.QueryList<c_Device>(sql, null);
            return list;
        }

        public virtual int GetListPageCount(string conStr, long? userId, int? ElectricId, int? LineId, int? JointId, int? bigTypeId, int? smallTypeId, string searchText,int? isError, int? isOnline)
        {
            BaseDAL dal = new BaseDAL(conStr);
            StringBuilder sqlStr = new StringBuilder();
            if (userId.HasValue)
            {
                sqlStr.Append("select COUNT(0) as count from t_organizeDistribute as b inner join t_Device as a on b.nodeId=a.parentId where b.userId=" + userId + " and isDel=0");
            }
            else
            {
                sqlStr.Append("select COUNT(0) as count from t_Device as a where isDel=0");
            }
            if (ElectricId.HasValue)
            {
                sqlStr.Append(" and a.ElectricId=" + ElectricId + "");
            }
            if (LineId.HasValue)
            {
                sqlStr.Append(" and a.LineId=" + LineId + "");
            }
            if (JointId.HasValue)
            {
                sqlStr.Append(" and a.parentId=" + JointId + "");
            }
            if (bigTypeId.HasValue)
            {
                sqlStr.Append(" and a.bigTypeId=" + bigTypeId + "");
            }
            if (smallTypeId.HasValue)
            {
                sqlStr.Append(" and a.smallTypeId=" + smallTypeId + "");
            }
            if (isError.HasValue)
            {
                sqlStr.Append(" and a.isError=" + isError + "");
            }
            if (isOnline.HasValue)
            {
                sqlStr.Append(" and a.isOnline=" + isOnline + "");
            }
            if (!string.IsNullOrEmpty(searchText))
            {
                sqlStr.Append("and (a.deviceName like '%" + searchText + "%'");
                sqlStr.Append(" or a.TerminalId like '%" + searchText + "%')");
            }
            string sql = sqlStr.ToString();
            c_Count model = dal.QuerySingle<c_Count>(sql, null);
            return model.count;
        }

        /// <summary>
        /// 分页获取设备根
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="ElectricId">供电</param>
        /// <param name="LineId">线路</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="totalcount">总条数</param>
        /// <returns></returns>
        public virtual List<t_Device> GetListPage(string conStr, long? parentId, int pageIndex, int pageSize, out int totalcount,string searchText)
        {
            BaseDAL dal = new BaseDAL(conStr);
            OrderModelField[] orderList = new OrderModelField[]
            {
                new OrderModelField {IsDESC=false,PropertyName="orderNo" },
            };
            Expression<Func<t_Device, bool>> seleWhere;
            Expression<Func<t_Device, bool>> seleWhere1 = t => true;
            if (string.IsNullOrEmpty(searchText))
            {
                seleWhere = t => t.parentId == parentId && t.isDel == false;
                List<t_Device> list = dal.GetListPaged<t_Device>(pageIndex, pageSize, seleWhere, out totalcount, orderList);
                return list;
            }
            else
            {
                seleWhere1 = t => t.TerminalId.Contains(searchText) || t.deviceName.Contains(searchText);
                seleWhere = t => t.parentId == parentId && t.isDel == false;
                seleWhere.AndAlso(seleWhere1);
                List<t_Device> list = dal.GetListPaged<t_Device>(pageIndex, pageSize, seleWhere, out totalcount, orderList);
                return list;
            }
        }

        /// <summary>
        /// 获取设备数量根据供电和线路
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="ElectricId">供电Id</param>
        /// <param name="LineId">线路Id</param>
        /// <returns></returns>
        public virtual c_Count GetDeviceCount(string conStr, long parentId, string searchText)
        {
            BaseDAL dal = new BaseDAL(conStr);
            string sql = null;
            if (string.IsNullOrEmpty(searchText))
            {
                sql = "select count(0) as count from t_Device where parentId=@parentId and isDel=0";
            }
            else
            {
                sql = "select count(0) as count from t_Device where parentId=@parentId and isDel=0 and (deviceName like '%" + searchText + "%' or TerminalId like '%" + searchText + "%')";
            }
            SqlParameter[] paras = new SqlParameter[]
            {
                new SqlParameter("@parentId",parentId)
            };
            c_Count model = dal.QuerySingleOrDefault<c_Count>(sql, paras);
            return model;
        }

        /// <summary>
        /// 根据供电、线路、终端标识获取设备
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="ElectricId"></param>
        /// <param name="LineId"></param>
        /// <param name="TerminalId"></param>
        /// <returns></returns>
        public virtual t_Device GetSingleDevice(string conStr, int ElectricId, int LineId, string TerminalId)
        {
            BaseDAL dal = new BaseDAL(conStr);
            t_Device model = dal.GetSingle<t_Device>(t =>t.isDel==false && t.ElectricId == ElectricId && t.LineId == LineId && t.TerminalId == TerminalId);
            return model;
        }

        /// <summary>
        /// 根据终端Id获取设备
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="TerminalId"></param>
        /// <returns></returns>
        public virtual List<t_Device> GetDeviceByTerId(string conStr,string TerminalId)
        {
            BaseDAL dal = new BaseDAL(conStr);
            List<t_Device> list = dal.GetList<t_Device>(t => t.TerminalId == TerminalId && t.isDel == false);
            return list;
        }

        /// <summary>
        /// 设备重命名
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="deviceName"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public virtual int ReNameDevice(string conStr, string deviceName, long Id)
        {
            BaseDAL dal = new BaseDAL(conStr);
            string sql = "update t_Device set deviceName=@deviceName where Id=@Id";
            SqlParameter[] paras = new SqlParameter[]
            {
                new SqlParameter("@deviceName",deviceName),
                new SqlParameter("@Id",Id)
            };
            int number = dal.ExecuteSql(sql, paras);
            return number;
        }

        /// <summary>
        /// 删除子集设备
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="nodePath"></param>
        /// <returns></returns>
        public virtual int DeleteDeivceByPath(string conStr, string nodePath,long DelUser)
        {
            BaseDAL dal = new BaseDAL(conStr);
            string sql = "update t_Device set isDel=1,DelUser=@DelUser,DelTime=@DelTime where nodePath like '%" + nodePath + "%'";
            SqlParameter[] paras = new SqlParameter[]
            {
                new SqlParameter("@DelUser",DelUser),
                new SqlParameter("@DelTime",DateTime.Now)
            };
            int number = dal.ExecuteSql(sql, paras);
            return number;
        }

        /// <summary>
        /// 根据接头Id获取设备
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="JointId"></param>
        /// <returns></returns>
        public virtual List<t_Device> GetDeviceByJoint(string conStr,List<long> JointId)
        {
            BaseDAL dal = new BaseDAL(conStr);
            StringBuilder strSql = new StringBuilder();
            if (JointId.Count > 0)
            {
                strSql.Append("select * from t_Device");
                for (int i = 0; i < JointId.Count; i++)
                {
                    if (i == 0)
                    {
                        strSql.Append(" where parentId=" + JointId[i] + "");

                    }
                    else
                    {
                        strSql.Append(" or parentId=" + JointId[i] + "");
                    }
                }
                string sql = strSql.ToString();
                return dal.QueryList<t_Device>(sql, null);
            }
            else
            {
                return null;
            }
        }

        public virtual List<t_Device> GetDeviceByUserOrganize(string conStr, int ElectricId, int LineId, int? bigTypeId, int userId, int? jointId, int? smallTypeId, string searchText)
        {
            BaseDAL dal = new BaseDAL(conStr);
            StringBuilder strsql = new StringBuilder();
            strsql.Append("select a.* from t_organizeDistribute as b inner join t_Device as a on b.nodeId=a.parentId where userId=" + userId + " and a.ElectricId=" + ElectricId + " and a.LineId=" + LineId + " and a.isDel=0");
            if (bigTypeId.HasValue)
            {
                strsql.Append(" and a.bigTypeId=" + bigTypeId + "");
            }
            if (jointId.HasValue)
            {
                strsql.Append(" and a.parentId=" + jointId + "");
            }
            if (smallTypeId.HasValue)
            {
                strsql.Append(" and a.smallTypeId=" + smallTypeId + "");
            }
            if (!string.IsNullOrEmpty(searchText))
            {
                strsql.Append(" and a.deviceName like '%" + searchText + "%'");
                strsql.Append(" or a.TerminalId like '%" + searchText + "%'");
            }
            string sql = strsql.ToString();
            List<t_Device> list = dal.QueryList<t_Device>(sql, null);
            return list;
        }

        public virtual List<t_Device> GetDeviceBySuperUser(string conStr,int ElectricId,int LineId,int? bigTypeId,int? jointId,int? smallTypeId,string searchText)
        {
            BaseDAL dal = new BaseDAL(conStr);
            StringBuilder strsql = new StringBuilder();
            strsql.Append("select * from t_Device where ElectricId=" + ElectricId + " and LineId=" + LineId+ " and isDel=0");
            if (bigTypeId.HasValue)
            {
                strsql.Append(" and bigTypeId=" + bigTypeId + "");
            }
            if (jointId.HasValue)
            {
                strsql.Append(" and parentId=" + jointId + "");
            }
            if (smallTypeId.HasValue)
            {
                strsql.Append(" and smallTypeId=" + smallTypeId + "");
            }
            if (!string.IsNullOrEmpty(searchText))
            {
                strsql.Append(" and deviceName like '%" + searchText + "%'");
                strsql.Append(" or TerminalId like '%" + searchText + "%'");
            }
            string sql = strsql.ToString();
            List<t_Device> list = dal.QueryList<t_Device>(sql, null);
            return list;
        }

        /// <summary>
        /// 超级管理员获取所有设备在线数
        /// </summary>
        /// <param name="conStr"></param>
        /// <returns></returns>
        public virtual int GetAllOnline(string conStr)
        {
            BaseDAL dal = new BaseDAL(conStr);
            string sql = "select COUNT(0) as count from t_Device where isDel=0 and isOnline=1";
            c_Count model = dal.QuerySingle<c_Count>(sql, null);
            return model.count;
        }

        /// <summary>
        /// 超级管理员获取所有设备总数
        /// </summary>
        /// <param name="conStr"></param>
        /// <returns></returns>
        public virtual int GetAllCount(string conStr)
        {
            BaseDAL dal = new BaseDAL(conStr);
            string sql = "select COUNT(0) as count from t_Device where isDel=0";
            c_Count model = dal.QuerySingle<c_Count>(sql, null);
            return model.count;
        }

        /// <summary>
        /// 其他用户获取关联设备在线数
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public virtual int GetAllOnlineByUser(string conStr,long userId)
        {
            BaseDAL dal = new BaseDAL(conStr);
            string sql = "select COUNT(0) as count from t_Device as a inner join t_organizeDistribute as b on a.parentId=b.nodeId where a.isDel=0 and b.userId=@userId and a.isOnline = 1";
            SqlParameter[] paras = new SqlParameter[]
            {
                new SqlParameter("@userId",userId)
            };
            c_Count model = dal.QuerySingle<c_Count>(sql, paras);
            return model.count;
        }

        /// <summary>
        /// 其他用户获取关联设备总数
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public virtual int GetAllCountByUser(string conStr,long userId)
        {
            BaseDAL dal = new BaseDAL(conStr);
            string sql = "select COUNT(0) as count from t_Device as a inner join t_organizeDistribute as b on a.parentId=b.nodeId where a.isDel=0 and b.userId=@userId";
            SqlParameter[] paras = new SqlParameter[]
            {
                new SqlParameter("@userId",userId)
            };
            c_Count model = dal.QuerySingle<c_Count>(sql, paras);
            return model.count;
        }

        /// <summary>
        /// 超级管理员根据设备类型获取设备已切入在线数
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="bigTypeId"></param>
        /// <returns></returns>
        public virtual int GetOnlineCountType(string conStr,int bigTypeId)
        {
            BaseDAL dal = new BaseDAL(conStr);
            string sql = "select COUNT(0) as count from t_Device where isDel=0 and bigTypeId=@bigTypeId and isOnline=1";
            SqlParameter[] paras = new SqlParameter[]
            {
                new SqlParameter("@bigTypeId",bigTypeId)
            };
            c_Count model = dal.QuerySingle<c_Count>(sql, paras);
            return model.count;
        }

        /// <summary>
        /// 超级管理员根据设备类型获取设备已切入离线总数
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="bigTypeId"></param>
        /// <returns></returns>
        public virtual int GetOfflineCountType(string conStr, int bigTypeId)
        {
            BaseDAL dal = new BaseDAL(conStr);
            string sql = "select COUNT(0) as count from t_Device where isDel=0 and bigTypeId=@bigTypeId and isOnline=0";
            SqlParameter[] paras = new SqlParameter[]
            {
                new SqlParameter("@bigTypeId",bigTypeId)
            };
            c_Count model = dal.QuerySingle<c_Count>(sql, paras);
            return model.count;
        }

        /// <summary>
        /// 根据用户设备类型获取在线数
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="bigTypeId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public virtual int GetOnlineCountTypeByUser(string conStr,int bigTypeId,long userId)
        {
            BaseDAL dal = new BaseDAL(conStr);
            string sql = "select COUNT(0) as count from t_Device as a inner join t_organizeDistribute as b on a.parentId=b.nodeId where a.isDel=0 and b.userId=@userId and a.bigTypeId =@bigTypeId and a.isOnline=1";
            SqlParameter[] paras = new SqlParameter[]
            {
                new SqlParameter("@userId",userId),
                new SqlParameter("@bigTypeId",bigTypeId)
            };
            c_Count model = dal.QuerySingle<c_Count>(sql, paras);
            return model.count;
        }

        /// <summary>
        /// 根据用户设备类型获取总数
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="bigTypeId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public virtual int GetOfflineCountTypeByUser(string conStr,int bigTypeId,long userId)
        {
            BaseDAL dal = new BaseDAL(conStr);
            string sql = "select COUNT(0) as count from t_Device as a inner join t_organizeDistribute as b on a.parentId=b.nodeId where a.isDel=0 and b.userId=@userId and a.bigTypeId =@bigTypeId and a.isOnline=0";
            SqlParameter[] paras = new SqlParameter[]
            {
                new SqlParameter("@userId",userId),
                new SqlParameter("@bigTypeId",bigTypeId)
            };
            c_Count model = dal.QuerySingle<c_Count>(sql, paras);
            return model.count;
        }

        /// <summary>
        /// 超级管理员获取
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public virtual List<t_Device> GetDeviceErrorList(string conStr,int pageIndex,int pageSize,out int totalCount)
        {
            BaseDAL dal = new BaseDAL(conStr);
            OrderModelField[] order = new OrderModelField[]
            {
                new OrderModelField {IsDESC=false,PropertyName="Id" }
            };
            List<t_Device> list = dal.GetListPaged<t_Device>(pageIndex, pageSize,t=>t.isDel==false && t.isOnline == false, out totalCount, order);
            return list;
        }

        /// <summary>
        /// 其他角色获取
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public virtual List<t_Device> GetDeviceErrorListByUser(string conStr, int pageIndex, int pageSize, out int totalCount, long userId)
        {
            BaseDAL dal = new BaseDAL(conStr);
            totalCount = GetDeviceErrorListCountByUser(conStr, userId);
            pageIndex = (pageIndex - 1) * pageSize;
            string sql = "select top " + pageSize + " o.* from(select row_number() over(order by a.Id) as rownumber,a.* from t_Device as a inner join t_organizeDistribute as b on a.parentId = b.nodeId where a.isOnline=0 and a.isDel=0 and b.userId=" + userId + ") as o where rownumber>" + pageIndex + "";
            List<t_Device> list = dal.QueryList<t_Device>(sql, null);
            return list;
        }

        public virtual int GetDeviceErrorListCountByUser(string conStr, long userId)
        {
            BaseDAL dal = new BaseDAL(conStr);
            string sql = "select COUNT(0) as count from t_Device as a inner join t_organizeDistribute as b on a.parentId=b.nodeId where a.isOnline=0 and a.isDel=0 and b.userId=@userId";
            SqlParameter[] paras = new SqlParameter[]
            {
                new SqlParameter("@userId",userId)
            };
            c_Count model = dal.QuerySingle<c_Count>(sql, paras);
            return model.count;
        }

        public virtual List<t_Device> GetDelListPage(string conStr,int pageIndex,int pageSize,out int totalCount,string searchText)
        {
            BaseDAL dal = new BaseDAL(conStr);
            OrderModelField[] order = new OrderModelField[]
            {
                new OrderModelField {IsDESC=true,PropertyName="DelTime" }
            };
            Expression<Func<t_Device, bool>> seleWhere;
            if (string.IsNullOrEmpty(searchText))
            {
                seleWhere = t => t.isDel == true;
                return dal.GetListPaged<t_Device>(pageIndex, pageSize, seleWhere, out totalCount, order);
            }
            else
            {
                seleWhere = t => t.isDel == true && t.deviceName.Contains(searchText) || t.TerminalId.Contains(searchText);
                return dal.GetListPaged<t_Device>(pageIndex, pageSize, seleWhere, out totalCount, order);
            }
        }

        public virtual bool UpdataDel(string conStr, int Id)
        {
            BaseDAL dal = new BaseDAL(conStr);
            t_Device model = dal.GetSingleById<t_Device>(Id);
            model.isDel = false;
            return dal.Update(model);
        }

        /// <summary>
        /// 获取故障设备
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public virtual DataTable GetErrorDeviceTab(string conStr, long? Id, int? ElectricId, int? LineId, int? JointId, int? bigTypeId, int? smallTypeId, string searchText, int? isError, int? isOnline)
        {
            string[] arycon = conStr.Split(';');
            int i, li_index;
            string dataSource = null, dataName = null, userId = null, password = null;
            for (i = 0; i < arycon.Length; i++)
            {
                if (arycon[i].IndexOf("data source") > -1)
                {
                    li_index = arycon[i].IndexOf("=");
                    dataSource = arycon[i].Substring(li_index + 1);
                }
                if (arycon[i].IndexOf("initial catalog") > -1)
                {
                    li_index = arycon[i].IndexOf("=");
                    dataName = arycon[i].Substring(li_index + 1);
                }
                if (arycon[i].IndexOf("user id") > -1)
                {
                    li_index = arycon[i].IndexOf("=");
                    userId = arycon[i].Substring(li_index + 1);
                }
                if (arycon[i].IndexOf("password") > -1)
                {
                    li_index = arycon[i].IndexOf("=");
                    password = arycon[i].Substring(li_index + 1);
                }
            }
            string conc = string.Format("server={0};uid={1};pwd={2};database={3};Trusted_Connection=no;connect timeout = 60;", dataSource, userId, password, dataName);
            SqlHelper helper = new SqlHelper(conc);
            StringBuilder sqlStr = new StringBuilder();
            if (Id.HasValue)
            {
                sqlStr.Append("select c.name as ElectricName,d.name as LineName,e.name as JointName, b.deviceName, b.TerminalId, f.typeName as bigTypeName, g.typeName as smallTypeName,b.localInstructions, b.longitude, b.latitude, b.manufacturer, b.Installer, b.InstallDate, b.simNo, case b.isError when 1 then '是' when 0 then '否' end as 'isError',case b.isOnline when 1 then '在线' when 0 then '离线' end as 'isOnline', b.createTime from t_organizeDistribute as a inner join t_Device as b on a.nodeId=b.parentId inner join t_organize as c on b.ElectricId=c.Id inner join t_organize as d on b.LineId = d.Id inner join t_organize as e on b.parentId = e.Id inner join t_deviceBigType as f on b.bigTypeId = f.Id inner join t_deviceSmallType as g on b.smallTypeId = g.Id where a.userId = " + Id + " and b.isDel = 0");
                if (ElectricId.HasValue)
                {
                    sqlStr.Append(" and b.ElectricId=" + ElectricId + "");
                }
                if (LineId.HasValue)
                {
                    sqlStr.Append(" and b.LineId=" + LineId + "");
                }
                if (JointId.HasValue)
                {
                    sqlStr.Append(" and b.parentId=" + JointId + "");
                }
                if (bigTypeId.HasValue)
                {
                    sqlStr.Append(" and b.bigTypeId=" + bigTypeId + "");
                }
                if (smallTypeId.HasValue)
                {
                    sqlStr.Append(" and b.smallTypeId=" + smallTypeId + "");
                }
                if (isError.HasValue)
                {
                    sqlStr.Append(" and b.isError=" + isError + "");
                }
                if (isOnline.HasValue)
                {
                    sqlStr.Append(" and b.isOnline=" + isOnline + "");
                }
                if (!string.IsNullOrEmpty(searchText))
                {
                    sqlStr.Append("and (b.deviceName like '%" + searchText + "%'");
                    sqlStr.Append(" or b.TerminalId like '%" + searchText + "%')");
                }
                sqlStr.Append(" order by b.Id asc");
            }
            else
            {
                sqlStr.Append("select b.name as ElectricName,c.name as LineName,d.name as JointName,a.deviceName,a.TerminalId,e.typeName as bigTypeName,f.typeName as smallTypeName,a.localInstructions,a.longitude,a.latitude,a.manufacturer,a.Installer,a.InstallDate, a.simNo, case a.isError when 1 then '是' when 0 then '否' end as 'isError',case a.isOnline when 1 then '在线' when 0 then '离线' end as 'isOnline', a.createTime from t_Device as a inner join t_organize as b on a.ElectricId = b.Id inner join t_organize as c on a.LineId = c.Id inner join t_organize as d on a.parentId = d.Id inner join t_deviceBigType as e on a.bigTypeId = e.Id inner join t_deviceSmallType as f on a.smallTypeId = f.Id where a.isDel = 0");
                if (ElectricId.HasValue)
                {
                    sqlStr.Append(" and a.ElectricId=" + ElectricId + "");
                }
                if (LineId.HasValue)
                {
                    sqlStr.Append(" and a.LineId=" + LineId + "");
                }
                if (JointId.HasValue)
                {
                    sqlStr.Append(" and a.parentId=" + JointId + "");
                }
                if (bigTypeId.HasValue)
                {
                    sqlStr.Append(" and a.bigTypeId=" + bigTypeId + "");
                }
                if (smallTypeId.HasValue)
                {
                    sqlStr.Append(" and a.smallTypeId=" + smallTypeId + "");
                }
                if (isError.HasValue)
                {
                    sqlStr.Append(" and a.isError=" + isError + "");
                }
                if (isOnline.HasValue)
                {
                    sqlStr.Append(" and a.isOnline=" + isOnline + "");
                }
                if (!string.IsNullOrEmpty(searchText))
                {
                    sqlStr.Append("and (a.deviceName like '%" + searchText + "%'");
                    sqlStr.Append(" or a.TerminalId like '%" + searchText + "%')");
                }
                sqlStr.Append(" order by a.Id asc");
            }
            string sql = sqlStr.ToString();
            DataTable table = helper.ExcuteDataTable(sql, null);
            return table;
        }

        /// <summary>
        /// 根据供电和线路获取设备
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="ElectricId"></param>
        /// <param name="LineId"></param>
        /// <returns></returns>
        public virtual List<t_Device> GetListByEIdAndLId(string conStr, long? ElectricId, long? LineId)
        {
            BaseDAL dal = new BaseDAL(conStr);
            List<t_Device> result = new List<t_Device>();
            Expression<Func<t_Device, bool>> seleWhere = t => t.ElectricId == ElectricId && t.LineId == LineId && t.isDel == false;
            result = dal.GetList(seleWhere);
            return result;
        }


        public virtual List<t_Device> GetDeviceByParent(string conStr,long parentId)
        {
            BaseDAL dal = new BaseDAL(conStr);
            List<t_Device> list = dal.GetList<t_Device>(t => t.isDel == false && t.parentId == parentId);
            return list;
        }
    }
}
