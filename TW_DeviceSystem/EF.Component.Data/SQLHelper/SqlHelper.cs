using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Component.Data.SQLHelper
{
    public class SqlHelper
    {
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        static string connstr;


        public SqlHelper(string con)
        {
            connstr = con;
        }

        /// <summary>
        /// 创建SqlCommand对象
        /// </summary>
        /// <returns></returns>
        public SqlCommand CreateCommand(string sql, SqlConnection con, SqlParameter[] paras = null, CommandType comtype = CommandType.Text)
        {
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.CommandType = comtype;
            if (paras != null)
            {
                cmd.Parameters.AddRange(paras);
            }
            return cmd;
        }
        /// <summary>
        /// 返回执行行数
        /// </summary>
        /// <returns></returns>
        public int ExecuteNonQuery(string sql, SqlParameter[] paras = null, CommandType comtype = CommandType.Text)
        {
            int result = -1;
            using (SqlConnection con = new SqlConnection(connstr))
            {
                try
                {
                    SqlCommand cmd = CreateCommand(sql, con, paras, comtype);
                    result = cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw;
                }

            }
            return result;
        }
        /// <summary>
        /// 返回单个的执行对象
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paras"></param>
        /// <param name="comtype"></param>
        /// <returns></returns>
        public int ExecuteScalar(string sql, SqlParameter[] paras = null, CommandType comtype = CommandType.Text)
        {
            int result = -1;
            using (SqlConnection con = new SqlConnection(connstr))
            {
                try
                {
                    SqlCommand cmd = CreateCommand(sql, con, paras, comtype);
                    result = Convert.ToInt32(cmd.ExecuteScalar());
                }
                catch (Exception ex)
                {
                    throw;
                }

            }
            return result;
        }

        /// <summary>
        /// 返回DataTable对象
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paras"></param>
        /// <param name="comtype"></param>
        /// <returns></returns>
        public DataTable ExcuteDataTable(string sql, SqlParameter[] paras = null, CommandType comtype = CommandType.Text)
        {
            DataTable table = new DataTable();
            using (SqlConnection con = new SqlConnection(connstr))
            {
                try
                {
                    SqlCommand cmd = CreateCommand(sql, con, paras, comtype);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(table);
                }
                catch (Exception ex)
                {
                    throw;
                }
                return table;
            }
        }
        /// <summary>
        /// 返回sqlDataReder对象
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paras"></param>
        /// <param name="comtype"></param>
        /// <returns></returns>
        public SqlDataReader ExecuteReader(string sql, SqlParameter[] paras = null, CommandType comtype = CommandType.Text)
        {
            SqlDataReader dr = null;
            SqlConnection con = new SqlConnection(connstr);
            try
            {
                SqlCommand cmd = CreateCommand(sql, con, paras, comtype);
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                con.Close();
                throw;
            }
            return dr;
        }
    }
}
