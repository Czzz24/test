using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Component.Tools
{
    /// <summary>
    /// ClassUpload 的摘要说明。
    /// </summary>
    /*
     wenconfig中添加元素
     <httpRuntime maxRequestLength="1048576" executionTimeout="3600" />
     <!-- maxRequestLength：指示 ASP.NET 
     支持的HTTP方式上载的最大字节数。该限制可用于防止因用户将大量文件传递到该服务器而导致的拒绝服务攻击。指定的大小以 KB 为单位。默认值为 4096 KB 
     (4 MB)。executionTimeout：指示在被 ASP.NET 自动关闭前，允许执行请求的最大秒数。在当文件超出指定的大小时，如果浏览器中会产生 
     DNS 错误或者出现服务不可得到的情况，也请修改以上的配置，把配置数加大。 
         -->
     */
    /*
     Machine.config中添加元素
     <processModel 
            enable="true"
            timeout="15" 
            idleTimeout="25"
            shutdownTimeout="5"
            requestLimit="1000"
            requestQueueLimit="500"
            responseDeadlockInterval="00:03:00"              
            responseRestartDeadlockInterval="Infinite"
            memoryLimit="80"
            webGarden="true"
            maxWorkerThreads="25"
            maxIoThreads="25"/>
      <!-- 
     上载大文件时，还可能会收到以下错误信息：</P><XMP> aspnet_wp.exe (PID: 1520) 被回收，因为内存消耗超过了 460 MB（可用 RAM 的百分之 60）。
    </XMP>
    <P>如果遇到此错误信息，请增加应用程序的 Machine.config 文件的 <processModel>元素中 memoryLimit 属性的值。 
       -->
     */
    public class UploadfilesHelper
    {
        private List<string> _fileName = new List<string>();        //上传文件扩展名
        private List<string> _saveName = new List<string>();        //重命名后的上传文件
        private string _LocalPath;        //获取本地上传文件路径(完整本地目录)
        private List<int> _length = new List<int>();      //保存文件长度(字节)
        private List<string> _ext_name = new List<string>();//文件扩展名
        private List<string> _error_msg = new List<string>();        //错误信息

        /// <summary>
        /// 上传文件成功后,文件名;多文件
        /// </summary>
        public List<string> FileName { get { return _fileName; } }
        /// <summary>
        /// 上传文件成功后文件的长度(字节);多文件
        /// </summary>
        public List<int> Length { get { return _length; } }
        /// <summary>
        /// 文件扩展名
        /// </summary>
        public List<string> ExtName { get { return _ext_name; } }
        /// <summary>
        /// 获取重命名后的上传文件;
        /// </summary>
        public List<string> SaveName { get { return _saveName; } }
        /// <summary>
        /// 获取本地上传文件路径(完整目录)
        /// </summary>
        public string LocalPath { get { return _LocalPath; } }
        /// <summary>
        /// 错误信息
        /// </summary>
        public List<string> ErrorMsg { get { return _error_msg; } }

        /// <summary>
        /// 上传文件方法
        /// </summary>
        /// <param name="files">文件</param>
        /// <param name="diy_name">自定义格式的名字,后面加+下划线+序号,预先判断</param>
        /// <param name="random_name">是否采用随机名称</param>
        /// <param name="savePath">保存的路径(虚拟目录)</param>
        /// <param name="maxlength">文件最大长度(字节)</param>
        public string upload(System.Web.HttpFileCollectionBase files, string savePath, string diy_name, bool random_name, int maxlength)
        {
            //得到上载文件信息和文件流
            if (files != null && files.Count > 0)
            {
                if (string.IsNullOrEmpty(savePath))
                { return "请指定保存路径"; }
                //检测是否是绝对路径
                if (savePath.Trim().IsMatch(@"^[a-oA-O]\:"))
                { this._LocalPath = savePath; }
                else
                { this._LocalPath = System.Web.HttpContext.Current.Server.MapPath(savePath); }
                //判断目录
                if (!System.IO.Directory.Exists(_LocalPath)) { System.IO.Directory.CreateDirectory(this._LocalPath); }

                for (int i = 0; i < files.Count; i++)
                {
                    if (files[i].ContentLength > maxlength)
                    {
                        _error_msg.Add(files[i].FileName + ":文件长度过大");
                        continue;
                    }
                    string _new_name = string.Empty;
                    if (random_name)
                    {
                        _new_name = this.RandomName() + "." + this.GetExtName(files[i].FileName);
                    }
                    else if (string.IsNullOrEmpty(diy_name))
                    {
                        _new_name = files[i].FileName;
                        while (File.Exists(this._LocalPath + _new_name))
                        {
                            _new_name = _new_name.Insert(_new_name.LastIndexOf('.'), "_" + i);
                        }
                    }
                    else
                    {
                        _new_name = diy_name + "." + this.GetExtName(files[0].FileName);
                        while (File.Exists(this._LocalPath + _new_name))
                        {
                            _new_name = _new_name.Insert(_new_name.LastIndexOf('.'), "_" + i);
                        }
                    }

                    Stream fs = null;
                    try
                    {
                        //处理上载的文件流信息。
                        byte[] b = new byte[files[0].ContentLength];
                        //System.IO.Stream fs;
                        fs = (System.IO.Stream)files[0].InputStream;
                        fs.Read(b, 0, files[0].ContentLength);

                        //调用处理内存流方法UploadFile
                        return this.UploadFile(b, files[0].FileName, _new_name, this._LocalPath);
                        //int value = Convert.ToInt32(asy.AsyncState);

                    }
                    catch (Exception ex)
                    { _error_msg.Add(files[i].FileName + ":" + ex.Message); }
                    finally
                    { fs.Close(); }
                }
                return "OK";
            }
            else { return "请选择文件"; }
        }

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="fs">文件流</param>
        /// <param name="old_Name">原文件名</param>
        /// <param name="fileName">新文件名</param>
        /// <param name="localPath">保存路径(本地)</param>
        /// <returns></returns>
        private string UploadFile(byte[] fs, string old_Name, string fileName, string localPath)
        {
            try
            {
                //定义并实例化一个内存流，以存放提交上来的字节数组。
                MemoryStream m = new MemoryStream(fs);
                FileStream f = new FileStream(localPath + (localPath.EndsWith("/") ? "/" : "") + fileName, FileMode.Create);
                //把内内存里的数据写入物理文件
                m.WriteTo(f);
                m.Close();
                f.Close();

                this._fileName.Add(old_Name);
                this._ext_name.Add(this.GetExtName(old_Name));
                this._length.Add(fs.Length);
                this._saveName.Add(fileName);

                return "OK";
            }
            catch (Exception ex)
            {
                string _msg = string.Format("文件{0}上传错误:{1}", old_Name, ex.Message);
                this._error_msg.Add(_msg);
                return _msg;
            }
        }
        //生成 (年,月,日,时,分,秒)+随机数的文件名
        private string RandomName()
        {
            Random rm = new Random(System.Environment.TickCount);
            return System.DateTime.Now.ToString("yyyyMMddHHmmss_") + rm.Next(1000, 9999).ToString();
        }
        /// <summary>
        /// 文件扩展名
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private string GetExtName(string fileName)
        {
            return fileName.Substring(fileName.LastIndexOf(".") + 1);
        }
    }
}
