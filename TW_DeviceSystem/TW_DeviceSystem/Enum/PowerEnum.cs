using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TW_DeviceSystem.Enum
{
    public enum PowerEnum
    {
        /// <summary>
        /// 添加设备
        /// </summary>
        addDevice = 1,

        /// <summary>
        /// 修改设备
        /// </summary>
        updataDevice=2,

        /// <summary>
        /// 查看设备
        /// </summary>
        selectDevice=3,

        /// <summary>
        /// 删除设备
        /// </summary>
        delDevice =4,

        /// <summary>
        /// 添加树节
        /// </summary>
        addOrganize=5,

        /// <summary>
        /// 修改树节
        /// </summary>
        updateOrganize=6,

        /// <summary>
        /// 删除树节
        /// </summary>
        delOrganize=7,

        /// <summary>
        /// 添加用户
        /// </summary>
        addUser=8,

        /// <summary>
        /// 修改用户
        /// </summary>
        updateUser=9,

        /// <summary>
        /// 查看用户
        /// </summary>
        selectUser=10,

        /// <summary>
        /// 删除用户
        /// </summary>
        delUser=11,
    }
}