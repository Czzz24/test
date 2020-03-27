using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Component.Tools
{
    /// <summary>
    /// 日志
    /// </summary>
    public static class Logger
    {
        private static string filePath = AppDomain.CurrentDomain.BaseDirectory + "\\log\\";
        /// <summary>
        /// 写入日志到文件
        /// </summary>
        /// <param name="ex"></param>
        public static void LoggerToFile(Exception ex, string log_prefix = "")
        {
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            StringBuilder _sb = new StringBuilder();
            _sb.Append(ex.Message);
            _sb.Append("\r\n");
            _sb.Append(ex.TargetSite);
            _sb.Append("\r\n");
            _sb.Append(ex.Source);
            _sb.Append("\r\n");
            _sb.Append(ex.StackTrace);
            _sb.Append("\r\n");
            _sb.Append("\r\n");
            _sb.Append("=============================================================================");
            _sb.Append(ex.ToJson());
            _sb.Append("\r\n");
            _sb.Append("\r\n");
            try
            {
                Random random = new Random();

                string fileName = string.Format("{2}{0:yyyyMMddHHmmss}_{1}.txt", DateTime.Now, random.Next(1000, 9999), (string.IsNullOrEmpty(log_prefix) ? "" : log_prefix));

                StreamWriter SWriter = new StreamWriter(filePath + fileName, false, System.Text.Encoding.UTF8);
                SWriter.Write(_sb);
                SWriter.Close();
                SWriter.Dispose();
            }
            catch (System.IO.IOException IOEX)
            {
                throw new IOException(IOEX.Message);
            }
        }

        public static void LoggerToFile(string msg, string log_prefix = "")
        {
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            try
            {
                Random random = new Random();

                string fileName = string.Format("{2}{0:yyyyMMddHHmmss}_{1}.txt", DateTime.Now, random.Next(1000, 9999), (string.IsNullOrEmpty(log_prefix) ? "" : log_prefix));

                StreamWriter SWriter = new StreamWriter(filePath + fileName, false, System.Text.Encoding.UTF8);
                SWriter.Write(msg);
                SWriter.Close();
                SWriter.Dispose();
            }
            catch (System.IO.IOException IOEX)
            {
                throw new IOException(IOEX.Message);
            }
        }
    }
}
