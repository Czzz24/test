using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace EF.Component.Tools
{
    public static class DirFileHelper
    {
        /// <summary>
        /// 向文本文件的尾部追加内容
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        /// <param name="content">写入的内容</param>
        public static void AppendText(string filePath, string content)
        {
            File.AppendAllText(filePath, content);
        }
        /// <summary>
        /// 清空指定目录下所有文件及子目录,但该目录依然保存.
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>
        public static void ClearDirectory(string directoryPath)
        {
            if (IsExistDirectory(directoryPath))
            {
                string[] fileNames = GetFileNames(directoryPath);
                for (int i = 0; i < fileNames.Length; i++)
                {
                    DeleteFile(fileNames[i]);
                }
                string[] directories = GetDirectories(directoryPath);
                for (int j = 0; j < directories.Length; j++)
                {
                    DeleteDirectory(directories[j]);
                }
            }
        }

        /// <summary>
        /// 清空文件内容 
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        public static void ClearFile(string filePath)
        {
            File.Delete(filePath);
            CreateFile(filePath);
        }

        /// <summary>
        /// 检测指定目录中是否存在指定的文件,若要搜索子目录请使用重载方法. 
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>
        /// <param name="searchPattern">模式字符串，"*"代表0或N个字符，"?"代表1个字符。 范例："Log*.xml"表示搜索所有以Log开头的Xml文件。</param>
        /// <returns></returns>
        public static bool Contains(string directoryPath, string searchPattern)
        {
            bool flag;
            try
            {
                if (GetFileNames(directoryPath, searchPattern, false).Length == 0) return false;
                flag = true;
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return flag;
        }

        /// <summary>
        /// 检测指定目录中是否存在指定的文件
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>
        /// <param name="searchPattern">模式字符串，"*"代表0或N个字符，"?"代表1个字符。 范例："Log*.xml"表示搜索所有以Log开头的Xml文件。</param>
        /// <param name="isSearchChild">是否搜索子目录</param>
        /// <returns></returns>
        public static bool Contains(string directoryPath, string searchPattern, bool isSearchChild)
        {
            bool flag;
            try
            {
                if (GetFileNames(directoryPath, searchPattern, true).Length == 0) return false;
                flag = true;
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return flag;
        }

        /// <summary>
        /// 将源文件的内容复制到目标文件中
        /// </summary>
        /// <param name="sourceFilePath">源文件的绝对路径</param>
        /// <param name="destFilePath">目标文件的绝对路径</param>
        public static void Copy(string sourceFilePath, string destFilePath)
        {
            File.Copy(sourceFilePath, destFilePath, true);
        }

        /// <summary>
        /// 复制文件
        /// </summary>
        /// <param name="dir1">要复制的文件的路径已经全名(包括后缀)</param>
        /// <param name="dir2">目标位置,并指定新的文件名</param>
        public static void CopyFile(string dir1, string dir2)
        {
            dir1 = dir1.Replace("/", @"\");
            dir2 = dir2.Replace("/", @"\");
            if (File.Exists(HttpContext.Current.Request.PhysicalApplicationPath + @"\" + dir1)) File.Copy(HttpContext.Current.Request.PhysicalApplicationPath + @"\" + dir1, HttpContext.Current.Request.PhysicalApplicationPath + @"\" + dir2, true);
        }

        /// <summary>
        /// 复制文件夹(递归) 
        /// </summary>
        /// <param name="varFromDirectory">源文件夹路径</param>
        /// <param name="varToDirectory">目标文件夹路径</param>
        public static void CopyFolder(string varFromDirectory, string varToDirectory)
        {
            Directory.CreateDirectory(varToDirectory);
            if (Directory.Exists(varFromDirectory))
            {
                string[] directories = Directory.GetDirectories(varFromDirectory);
                if (directories.Length > 0)
                {
                    foreach (string str in directories)
                    {
                        CopyFolder(str, varToDirectory + str.Substring(str.LastIndexOf(@"\")));
                    }
                }
                string[] files = Directory.GetFiles(varFromDirectory);
                if (files.Length > 0)
                {
                    foreach (string str2 in files)
                    {
                        File.Copy(str2, varToDirectory + str2.Substring(str2.LastIndexOf(@"\")), true);
                    }
                }
            }
        }

        /// <summary>
        /// 创建目录 
        /// </summary>
        /// <param name="dir">要创建的目录路径包括目录名</param>
        public static void CreateDir(string dir)
        {
            if (dir.Length != 0 && !Directory.Exists(HttpContext.Current.Request.PhysicalApplicationPath + @"\" + dir)) Directory.CreateDirectory(HttpContext.Current.Request.PhysicalApplicationPath + @"\" + dir);
        }


        /// <summary>
        /// 创建一个目录
        /// </summary>
        /// <param name="directoryPath">目录的绝对路径</param>
        public static void CreateDirectory(string directoryPath)
        {
            if (!IsExistDirectory(directoryPath)) Directory.CreateDirectory(directoryPath);
        }


        /// <summary>
        /// 创建一个文件。
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        public static void CreateFile(string filePath)
        {
            try
            {
                if (!IsExistFile(filePath))
                {
                    FileInfo info = new FileInfo(filePath);
                    info.Create().Close();
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        /// <summary>
        /// 创建一个文件,并将字节流写入文件。 
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        /// <param name="buffer">二进制流数据</param>
        public static void CreateFile(string filePath, byte[] buffer)
        {
            try
            {
                if (!IsExistFile(filePath))
                {
                    FileStream stream = new FileInfo(filePath).Create();
                    stream.Write(buffer, 0, buffer.Length);
                    stream.Close();
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        /// <summary>
        /// 创建文件
        /// </summary>
        /// <param name="dir">带后缀的文件名</param>
        /// <param name="pagestr">文件内容</param>
        public static void CreateFile(string dir, string pagestr)
        {
            dir = dir.Replace("/", @"\");
            if (dir.IndexOf(@"\") > -1) CreateDir(dir.Substring(0, dir.LastIndexOf(@"\")));
            StreamWriter writer = new StreamWriter(HttpContext.Current.Request.PhysicalApplicationPath + @"\" + dir, false, Encoding.UTF8);
            writer.Write(pagestr);
            writer.Close();
        }

        /// <summary>
        /// 删除目录
        /// </summary>
        /// <param name="dir">要删除的目录路径和名称</param>
        public static void DeleteDir(string dir)
        {
            if (dir.Length != 0 && Directory.Exists(HttpContext.Current.Request.PhysicalApplicationPath + @"\" + dir)) Directory.Delete(HttpContext.Current.Request.PhysicalApplicationPath + @"\" + dir);
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="file">要删除的文件路径和名称</param>
        public static void DeleteFile(string file)
        {
            if (File.Exists(HttpContext.Current.Request.PhysicalApplicationPath + file)) File.Delete(HttpContext.Current.Request.PhysicalApplicationPath + file);
        }

        /// <summary>
        /// 删除指定目录及其所有子目录 
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>
        public static void DeleteDirectory(string directoryPath)
        {
            if (IsExistDirectory(directoryPath)) Directory.Delete(directoryPath, true);
        }

        /// <summary>
        /// 删除指定文件夹对应其他文件夹里的文件 
        /// </summary>
        /// <param name="file"></param>
        public static void DeleteFolderFiles(string varFromDirectory, string varToDirectory)
        {
            Directory.CreateDirectory(varToDirectory);
            if (Directory.Exists(varFromDirectory))
            {
                string[] directories = Directory.GetDirectories(varFromDirectory);
                if (directories.Length > 0)
                {
                    foreach (string str in directories)
                    {
                        DeleteFolderFiles(str, varToDirectory + str.Substring(str.LastIndexOf(@"\")));
                    }
                }
                string[] files = Directory.GetFiles(varFromDirectory);
                if (files.Length > 0)
                {
                    foreach (string str2 in files)
                    {
                        File.Delete(varToDirectory + str2.Substring(str2.LastIndexOf(@"\")));
                    }
                }
            }
        }

        /// <summary>
        /// 检查文件,如果文件不存在则创建 
        /// </summary>
        /// <param name="FilePath">路径,包括文件名</param>
        public static void ExistsFile(string FilePath)
        {
            if (!File.Exists(FilePath)) File.Create(FilePath).Close();
        }

        /// <summary>
        /// 根据时间得到目录名yyyyMMdd 
        /// </summary>
        /// <returns></returns>
        public static string GetDateDir()
        {
            return DateTime.Now.ToString("yyyyMMdd");
        }

        /// <summary>
        /// 根据时间得到文件名HHmmssff 
        /// </summary>
        /// <returns></returns>
        public static string GetDateFile()
        {
            return DateTime.Now.ToString("HHmmssff");
        }

        /// <summary>
        /// 获取指定目录中所有子目录列表,若要搜索嵌套的子目录列表,请使用重载方法. 
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>
        /// <returns></returns>
        public static string[] GetDirectories(string directoryPath)
        {
            string[] directories;
            try
            {
                directories = Directory.GetDirectories(directoryPath);
            }
            catch (IOException exception)
            {
                throw exception;
            }
            return directories;
        }

        /// <summary>
        /// 获取指定目录及子目录中所有子目录列表 
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>
        /// <param name="searchPattern">模式字符串，"*"代表0或N个字符，"?"代表1个字符。 范例："Log*.xml"表示搜索所有以Log开头的Xml文件。</param>
        /// <param name="isSearchChild">是否搜索子目录</param>
        /// <returns></returns>
        public static string[] GetDirectories(string directoryPath, string searchPattern, bool isSearchChild)
        {
            string[] strArray;
            try
            {
                if (isSearchChild) return Directory.GetDirectories(directoryPath, searchPattern, SearchOption.AllDirectories);
                strArray = Directory.GetDirectories(directoryPath, searchPattern, SearchOption.TopDirectoryOnly);
            }
            catch (IOException exception)
            {
                throw exception;
            }
            return strArray;
        }

        /// <summary>
        /// 从文件的绝对路径中获取扩展名
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        /// <returns></returns>
        public static string GetExtension(string filePath)
        {
            FileInfo info = new FileInfo(filePath);
            return info.Extension;
        }

        /// <summary>
        /// 从文件的绝对路径中获取文件名( 包含扩展名 ) 
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        /// <returns></returns>
        public static string GetFileName(string filePath)
        {
            FileInfo info = new FileInfo(filePath);
            return info.Name;
        }

        /// <summary>
        /// 从文件的绝对路径中获取文件名( 不包含扩展名 ) 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetFileNameNoExtension(string filePath)
        {
            FileInfo info = new FileInfo(filePath);
            return info.Name.Split(new char[] { '.' })[0];
        }

        /// <summary>
        /// 获取指定目录中所有文件列表
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <returns></returns>
        public static string[] GetFileNames(string directoryPath)
        {
            if (!IsExistDirectory(directoryPath)) throw new FileNotFoundException();
            return Directory.GetFiles(directoryPath);
        }

        /// <summary>
        /// 获取指定目录及子目录中所有文件列表
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>
        /// <param name="searchPattern">模式字符串，"*"代表0或N个字符，"?"代表1个字符。 范例："Log*.xml"表示搜索所有以Log开头的Xml文件。</param>
        /// <param name="isSearchChild">是否搜索子目录</param>
        /// <returns></returns>
        public static string[] GetFileNames(string directoryPath, string searchPattern, bool isSearchChild)
        {
            string[] strArray;
            if (!IsExistDirectory(directoryPath)) throw new FileNotFoundException();
            try
            {
                if (isSearchChild) return Directory.GetFiles(directoryPath, searchPattern, SearchOption.AllDirectories);
                strArray = Directory.GetFiles(directoryPath, searchPattern, SearchOption.TopDirectoryOnly);
            }
            catch (IOException exception)
            {
                throw exception;
            }
            return strArray;
        }

        /// <summary>
        /// 根据时间获取指定路径的 后缀名的 的所有文件
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="Extension">后缀名 比如.txt</param>
        /// <returns></returns>
        public static DataRow[] GetFilesByTime(string path, string Extension)
        {
            if (Directory.Exists(path))
            {
                string searchPattern = string.Format("*{0}", Extension);
                string[] files = Directory.GetFiles(path, searchPattern);
                if (files.Length > 0)
                {
                    DataTable table = new DataTable();
                    table.Columns.Add(new DataColumn("filename", Type.GetType("System.String")));
                    table.Columns.Add(new DataColumn("createtime", Type.GetType("System.DateTime")));
                    for (int i = 0; i < files.Length; i++)
                    {
                        DataRow row = table.NewRow();
                        DateTime creationTime = File.GetCreationTime(files[i]);
                        string fileName = Path.GetFileName(files[i]);
                        row["filename"] = fileName;
                        row["createtime"] = creationTime;
                        table.Rows.Add(row);
                    }
                    return table.Select(string.Empty, "createtime desc");
                }
            }
            return new DataRow[0];
        }

        /// <summary>
        /// 获取一个文件的长度,单位为Byte 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static int GetFileSize(string filePath)
        {
            FileInfo info = new FileInfo(filePath);
            return (int)info.Length;
        }

        /// <summary>
        /// 获取文本文件的行数
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static int GetLineCount(string filePath)
        {
            return File.ReadAllLines(filePath).Length;
        }

        /// <summary>
        /// 检测指定目录是否为空 
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <returns></returns>
        public static bool IsEmptyDirectory(string directoryPath)
        {
            try
            {
                if (GetFileNames(directoryPath).Length > 0) return false;
                if (GetDirectories(directoryPath).Length > 0) return false;
                return true;
            }
            catch
            {
                return true;
            }
        }

        /// <summary>
        /// 检测指定目录是否存在
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <returns></returns>
        public static bool IsExistDirectory(string directoryPath)
        {
            return Directory.Exists(directoryPath);
        }

        /// <summary>
        /// 检测指定文件是否存在,如果存在则返回true。 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool IsExistFile(string filePath)
        {
            return File.Exists(filePath);
        }

        /// <summary>
        /// 将文件移动到指定目录 
        /// </summary>
        /// <param name="sourceFilePath">需要移动的源文件的绝对路径</param>
        /// <param name="descDirectoryPath">移动到的目录的绝对路径</param>
        public static void Move(string sourceFilePath, string descDirectoryPath)
        {
            string fileName = GetFileName(sourceFilePath);
            if (IsExistDirectory(descDirectoryPath))
            {
                if (IsExistFile(descDirectoryPath + @"\" + fileName)) DeleteFile(descDirectoryPath + @"\" + fileName);
                File.Move(sourceFilePath, descDirectoryPath + @"\" + fileName);
            }
        }

        /// <summary>
        /// 移动文件(剪贴--粘贴) 
        /// </summary>
        /// <param name="dir1">要移动的文件的路径及全名(包括后缀)</param>
        /// <param name="dir2">文件移动到新的位置,并指定新的文件名</param>
        public static void MoveFile(string dir1, string dir2)
        {
            dir1 = dir1.Replace("/", @"\");
            dir2 = dir2.Replace("/", @"\");
            if (File.Exists(HttpContext.Current.Request.PhysicalApplicationPath + @"\" + dir1)) File.Move(HttpContext.Current.Request.PhysicalApplicationPath + @"\" + dir1, HttpContext.Current.Request.PhysicalApplicationPath + @"\" + dir2);
        }

        /// <summary>
        /// 向文本文件中写入内容 
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        /// <param name="text">写入的内容</param>
        /// <param name="encoding">编码</param>
        public static void WriteText(string filePath, string text, Encoding encoding)
        {
            File.WriteAllText(filePath, text, encoding);
        }
    }
}
