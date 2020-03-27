using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Component.Tools
{
    public class FolderHelper
    {

        /// <summary>
        /// 添加文件夹
        /// </summary>
        /// <returns></returns>
        public static bool AddDirectory(string StrPath, string DirectoryName)
        {
            if (Directory.Exists(StrPath + "\\" + DirectoryName))
            {
                return false;
                throw new IOException("此文件夹已经存在");
            }
            else
            {
                try
                {
                    Directory.CreateDirectory(StrPath + "\\" + DirectoryName.Replace(".", ""));
                    return true;
                }
                catch (System.IO.IOException IOEc)
                {
                    throw new IOException(IOEc.Message);
                }
            }
        }

        /// <summary>
        /// 检测文件路径是否存在
        /// </summary>
        /// <param name="StrPath"></param>
        /// <returns></returns>
        public static bool CheckFolderExists(string StrPath)
        {
            return Directory.Exists(StrPath);
        }

        /// <summary>
        /// 获取指定路径下的所有的文件信息
        /// </summary>
        /// <param name="FilePath"></param>
        /// <param name="Folder"></param>
        /// <param name="FloterFileStr"></param>
        /// <returns></returns>
        public static DataTable GetDirectoryInfo(string FilePath, string FloterFileStr)
        {
            DataTable Dtable = new DataTable();
            //创建列 FileName
            Dtable.Columns.Add(new DataColumn("FileName", typeof(string)));
            //创建列 ReadOnly
            Dtable.Columns.Add(new DataColumn("ReadOnly", typeof(bool)));
            //创建列 CreateTime
            Dtable.Columns.Add(new DataColumn("CreateTime", typeof(DateTime)));
            //创建列 FullName
            Dtable.Columns.Add(new DataColumn("FullName", typeof(string)));
            //创建列 IsDirectoru
            Dtable.Columns.Add(new DataColumn("IsDirectoru", typeof(bool)));

            //文件夹的所有的目录
            DirectoryInfo DriInfo = new DirectoryInfo(FilePath);

            //过滤文件
            FileSystemInfo[] FileSystemList = DriInfo.GetFileSystemInfos();

            DataRow Drow = null;

            foreach (FileSystemInfo FileSin in FileSystemList)
            {
                Drow = Dtable.NewRow();
                Drow["FileName"] = FileSin.Name;
                Drow["ReadOnly"] = (FileSin.Attributes & FileAttributes.ReadOnly) == 0 ? false : true;
                Drow["CreateTime"] = FileSin.CreationTime;
                Drow["FullName"] = FileSin.FullName;
                Drow["IsDirectoru"] = (FileSin.Attributes & FileAttributes.Directory) == 0 ? false : true;

                Dtable.Rows.Add(Drow);
            }
            return Dtable;
        }

        public static DataTable GetFilesInfo(string FilePath, string FloterFileStr)
        {
            DataTable Dtable = new DataTable();
            //创建列 FileName
            Dtable.Columns.Add(new DataColumn("FileName", typeof(string)));
            //创建列FileSize
            Dtable.Columns.Add(new DataColumn("FileSize", typeof(string)));
            //创建列 ReadOnly
            Dtable.Columns.Add(new DataColumn("ReadOnly", typeof(bool)));
            //创建列 CreateTime
            Dtable.Columns.Add(new DataColumn("CreateTime", typeof(DateTime)));
            //创建列 FullName
            Dtable.Columns.Add(new DataColumn("FullName", typeof(string)));


            //文件夹的所有的目录
            DirectoryInfo DriInfo = new DirectoryInfo(FilePath);

            //过滤文件
            FileInfo[] FileSystemList = DriInfo.GetFiles();

            DataRow Drow = null;

            foreach (FileInfo FileSin in FileSystemList)
            {
                Drow = Dtable.NewRow();
                Drow["FileName"] = FileSin.Name;
                Drow["FileSize"] = (FileSin.Length / 1024) + "KB";
                Drow["ReadOnly"] = (FileSin.Attributes & FileAttributes.ReadOnly) == 0 ? false : true;
                Drow["CreateTime"] = FileSin.CreationTime;
                Drow["FullName"] = FileSin.FullName;

                Dtable.Rows.Add(Drow);
            }
            return Dtable;
        }

        /// <summary>
        /// 删除文件或者文件夹
        /// </summary>
        /// <param name="StrPath">路径</param>
        /// <param name="Type">0代表文件夹,1代表文件</param>
        public static void DeleteFileOrDirectory(string StrPath, int Type)
        {
            if (Type == 0)
            {
                if (Directory.Exists(StrPath))
                {
                    try
                    {
                        Directory.Delete(StrPath, true);
                    }
                    catch (System.IO.IOException IOEx)
                    {
                        throw new IOException(IOEx.Message);
                    }
                }
                else
                {
                    throw new IOException("文件路径不存在" + StrPath);
                }
            }
            else
            {
                if (File.Exists(StrPath))
                {
                    try
                    {
                        File.Delete(StrPath);
                    }
                    catch (System.IO.IOException ExIo)
                    {
                        throw new IOException(ExIo.Message);
                    }
                }
                else
                {
                    throw new IOException("文件路径不存在" + StrPath);
                }

            }
        }

        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="StrPath"></param>
        /// <returns></returns>
        public static string ReadFile(string StrPath)
        {
            string Content = "";
            try
            {
                StreamReader Sreader = new StreamReader(StrPath, System.Text.Encoding.GetEncoding("GB2312"));
                Content = Sreader.ReadToEnd();
                Sreader.Close();
                Sreader.Dispose();

            }
            catch (System.IO.IOException IoEx)
            {
                throw new IOException(IoEx.ToString());
            }
            return Content;
        }

        /// <summary>
        /// 写文件
        /// </summary>
        /// <param name="StrPath"></param>
        /// <param name="Content"></param>
        /// <param name="IsAppend"></param>
        public static void WriterFile(string StrPath, string Content, bool IsAppend)
        {
            try
            {
                StreamWriter SWriter = new StreamWriter(StrPath, IsAppend, System.Text.Encoding.GetEncoding("GB2312"));
                SWriter.Write(Content);
                SWriter.Close();
                SWriter.Dispose();
            }
            catch (System.IO.IOException IOEX)
            {
                throw new IOException(IOEX.Message);
            }
        }

        /// <summary>
        /// 获取当前PC上的磁盘驱动信息
        /// </summary>
        /// <returns></returns>
        public static DataTable DriceInfo()
        {
            DriveInfo[] Drivers = DriveInfo.GetDrives();
            DataTable Dtable = new DataTable();
            Dtable.Columns.Add(new DataColumn("DriverName", typeof(string)));
            Dtable.Columns.Add(new DataColumn("DriverType", typeof(string)));
            Dtable.Columns.Add(new DataColumn("DriverFomrat", typeof(string)));
            Dtable.Columns.Add(new DataColumn("VolumeLable", typeof(string)));
            Dtable.Columns.Add(new DataColumn("TotalFreeSpace", typeof(long)));
            Dtable.Columns.Add(new DataColumn("TotalSize", typeof(long)));
            Dtable.Columns.Add(new DataColumn("Percent", typeof(float)));
            DataRow Drow = null;
            foreach (DriveInfo Dinfo in Drivers)
            {
                if (Dinfo.DriveType == DriveType.Fixed)
                {
                    Drow = Dtable.NewRow();
                    Drow["DriverName"] = Dinfo.Name;
                    Drow["DriverType"] = Dinfo.DriveType;
                    Drow["DriverFomrat"] = Dinfo.DriveFormat;
                    Drow["VolumeLable"] = Dinfo.VolumeLabel;
                    Drow["TotalFreeSpace"] = Dinfo.TotalFreeSpace;
                    Drow["TotalSize"] = Dinfo.TotalSize;
                    Drow["Percent"] = (Dinfo.TotalFreeSpace * 100) / Dinfo.TotalSize;
                    Dtable.Rows.Add(Drow);
                }

            }
            return Dtable;
        }
    }
}
