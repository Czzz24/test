using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Component.Data.SQLHelper
{
     public static class Mapper
    {
        /// <summary>
        ///利用反射把DataTable的数据写到单个实体类
        /// </summary>
        /// <typeparam name="T">实体类(model)</typeparam>
        /// <param name="dtSource">DataTable数据源</param>
        /// <returns>返回的实体类对象</returns>
        public static T ToSingleEntity<T>(this System.Data.DataTable dtSource) where T : class, new()
        {
            if (dtSource == null)
            {
                return default(T);
            }

            if (dtSource.Rows.Count != 0)
            {
                Type type = typeof(T);
                Object entity = Activator.CreateInstance(type);         //创建实例               
                foreach (System.Reflection.PropertyInfo entityCols in type.GetProperties())
                {
                    if (!string.IsNullOrEmpty(dtSource.Rows[0][entityCols.Name].ToString()))
                    {

                        Type valType = entityCols.PropertyType;
                        if (valType.IsGenericType && valType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))//判断convertsionType是否为nullable泛型类  
                        {
                            //如果type为nullable类，声明一个NullableConverter类，该类提供从Nullable类到基础基元类型的转换  
                            System.ComponentModel.NullableConverter nullableConverter = new System.ComponentModel.NullableConverter(valType);
                            //将type转换为nullable对的基础基元类型  
                            valType = nullableConverter.UnderlyingType;
                        }
                        entityCols.SetValue(entity, Convert.ChangeType(dtSource.Rows[0][entityCols.Name], valType), null);
                        //entityCols.SetValue(entity, dtSource.Rows[0][entityCols.Name], null);
                    }
                }
                return (T)entity;
            }
            return default(T);
        }

        /// <summary>
        /// 利用反射把DataTable的数据写到集合实体类里
        /// </summary>
        /// <typeparam name="T">实体类(model)</typeparam>
        /// <param name="dtSource">DataTable数据源</param>
        /// <returns>返回IEnumerable的实体类对象</returns>
        public static IEnumerable<T> ToListEntity<T>(this System.Data.DataTable dtSource) where T : class, new()
        {
            if (dtSource == null)
            {
                return null;
            }

            List<T> list = new List<T>();
            Type type = typeof(T);
            foreach (System.Data.DataRow dataRow in dtSource.Rows)
            {
                Object entity = Activator.CreateInstance(type);         //创建实例               
                foreach (System.Reflection.PropertyInfo entityCols in type.GetProperties())
                {
                    if (!string.IsNullOrEmpty(dataRow[entityCols.Name].ToString()))
                    {
                        Type valType = entityCols.PropertyType;
                        if (valType.IsGenericType && valType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))//判断convertsionType是否为nullable泛型类  
                        {
                            //如果type为nullable类，声明一个NullableConverter类，该类提供从Nullable类到基础基元类型的转换  
                            System.ComponentModel.NullableConverter nullableConverter = new System.ComponentModel.NullableConverter(valType);
                            //将type转换为nullable对的基础基元类型  
                            valType = nullableConverter.UnderlyingType;
                        }
                        entityCols.SetValue(entity, Convert.ChangeType(dataRow[entityCols.Name], valType), null);
                    }
                }
                list.Add((T)entity);
            }
            return list;
        }
    }
}
