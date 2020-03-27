using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Web.Script.Serialization;

namespace EF.Component.Tools
{
    /// <summary>
    /// 转换为Json的帮助类
    /// </summary>
    public class JsonHelper
    {
        /// <summary>  
        /// 对象转换为Json字符串  
        /// </summary>  
        /// <param name="jsonObject">对象</param>  
        /// <returns>Json字符串</returns>  
        public static string ToJsonAll(object jsonObject)
        {
            string jsonString = "{";
            PropertyInfo[] propertyInfo = jsonObject.GetType().GetProperties();
            for (int i = 0; i < propertyInfo.Length; i++)
            {
                object objectValue = propertyInfo[i].GetGetMethod().Invoke(jsonObject, null);
                string value = string.Empty;
                if (objectValue is DateTime || objectValue is string || objectValue is Guid || objectValue is TimeSpan)
                {
                    value = "'" + objectValue + "'";
                }
                else if (objectValue is IEnumerable)
                {
                    value = ToString((IEnumerable)objectValue);
                }
                else
                {
                    value = objectValue.ToString();
                }
                jsonString += "\"" + propertyInfo[i].Name + "\":" + value + ",";
            }
            return JsonHelper.DeleteLast(jsonString) + "}";
        }

        /// <summary>  
        /// 对象集合转换Json  
        /// </summary>  
        /// <param name="array">集合对象</param>  
        /// <returns>Json字符串</returns>  
        public static string ToString(IEnumerable array)
        {
            string jsonString = "[";
            foreach (object item in array)
            {
                jsonString += JsonHelper.ToJsonAll(item) + ",";
            }
            return JsonHelper.DeleteLast(jsonString) + "]";
        }

        /// <summary>  
        /// 普通集合转换Json  
        /// </summary>  
        /// <param name="array">集合对象</param>  
        /// <returns>Json字符串</returns>  
        public static string ToArrayString(IEnumerable array)
        {
            string jsonString = "[";
            foreach (object item in array)
            {
                jsonString = item + ",";
            }
            return JsonHelper.DeleteLast(jsonString) + "]";
        }

        /// <summary>  
        /// 删除结尾字符  
        /// </summary>  
        /// <param name="str">需要删除的字符</param>  
        /// <returns>完成后的字符串</returns>  
        private static string DeleteLast(string str)
        {
            if (str.Length > 1)
            {
                return str.Substring(0, str.Length - 1);
            }
            return str;
        }

        /// <summary>  
        /// Datatable转换为Json  
        /// </summary>  
        /// <param name="table">Datatable对象</param>  
        /// <returns>Json字符串</returns>  
        public static string ToString(DataTable table)
        {
            string jsonString = "[";
            DataRowCollection drc = table.Rows;
            for (int i = 0; i < drc.Count; i++)
            {
                jsonString += "{";
                foreach (DataColumn column in table.Columns)
                {
                    jsonString += "\"" + column.ColumnName + "\":";
                    if (column.DataType == typeof(DateTime) || column.DataType == typeof(string))
                    {
                        jsonString += "\"" + drc[i][column.ColumnName] + "\",";
                    }
                    else
                    {
                        jsonString += drc[i][column.ColumnName] + ",";
                    }
                }
                jsonString = DeleteLast(jsonString) + "},";
            }
            return DeleteLast(jsonString) + "]";
        }

        /// <summary>  
        /// DataReader转换为Json  
        /// </summary>  
        /// <param name="dataReader">DataReader对象</param>  
        /// <returns>Json字符串</returns>  
        public static string ToString(DbDataReader dataReader)
        {
            string jsonString = "[";
            while (dataReader.Read())
            {
                jsonString += "{";
                for (int i = 0; i < dataReader.FieldCount; i++)
                {
                    jsonString += "\"" + dataReader.GetName(i) + "\":";
                    if (dataReader.GetFieldType(i) == typeof(DateTime) || dataReader.GetFieldType(i) == typeof(string))
                    {
                        jsonString += "\"" + dataReader[i] + "\",";
                    }
                    else
                    {
                        jsonString += dataReader[i] + ",";
                    }
                }
                jsonString = DeleteLast(jsonString) + "}";
            }
            dataReader.Close();
            return DeleteLast(jsonString) + "]";
        }

        /// <summary>  
        /// DataSet转换为Json  
        /// </summary>  
        /// <param name="dataSet">DataSet对象</param>  
        /// <returns>Json字符串</returns>  
        public static string ToString(DataSet dataSet)
        {
            string jsonString = "{";
            foreach (DataTable table in dataSet.Tables)
            {
                jsonString += "\"" + table.TableName + "\":" + ToString(table) + ",";
            }
            return jsonString = DeleteLast(jsonString) + "}";
        }

        /// <summary>
        /// json序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static string JsonSerializer<T>(T entity)
        {
            var serializer = new JavaScriptSerializer();
            return serializer.Serialize(entity);
        }

        /// <summary>
        /// json反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static T JsonDeserialize<T>(string jsonString)
        {
            var serializer = new JavaScriptSerializer();
            return serializer.Deserialize<T>(jsonString);
        }
    }
}
