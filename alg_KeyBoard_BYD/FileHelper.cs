﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace alg_KeyBoard_BYD
{
    ///<summary>  
    ///文件夹及文件操作类
    ///</summary>  
    public static class FileHelper
    {
        #region 检测指定目录是否存在
        /// <summary>  
        /// 检测指定目录是否存在  
        /// </summary>  
        /// <param name="directoryPath">目录的绝对路径</param>          
        public static bool IsExistDirectory(string directoryPath)
        {
            return Directory.Exists(directoryPath);
        }
        #endregion

        #region 检测指定文件是否存在
        /// <summary>  
        /// 检测指定文件是否存在,如果存在则返回true。  
        /// </summary>  
        /// <param name="filePath">文件的绝对路径</param>          
        public static bool IsExistFile(string filePath)
        {
            return File.Exists(filePath);
        }
        #endregion

        #region 检测指定目录是否为空
        /// <summary>  
        /// 检测指定目录是否为空  
        /// </summary>  
        /// <param name="directoryPath">指定目录的绝对路径</param>          
        public static bool IsEmptyDirectory(string directoryPath)
        {
            try
            {
                //判断是否存在文件  
                string[] fileNames = GetFileNames(directoryPath);
                if (fileNames.Length > 0)
                {
                    return false;
                }

                //判断是否存在文件夹  
                string[] directoryNames = GetDirectories(directoryPath);
                return directoryNames.Length <= 0;
            }
            catch(Exception ex)
            {
                LogHelper.RecordErrorLog(ex.Message, "FileHelper——IsEmptyDirectory");
                return false;
            }
        }
        #endregion

        #region 检测指定目录中是否存在指定的文件
        /// <summary>  
        /// 检测指定目录中是否存在指定的文件,若要搜索子目录请使用重载方法.  
        /// </summary>  
        /// <param name="directoryPath">指定目录的绝对路径</param>  
        /// <param name="searchPattern">模式字符串，"*"代表0或N个字符，"?"代表1个字符。  
        /// 范例："Log*.xml"表示搜索所有以Log开头的Xml文件。</param>          
        public static bool Contains(string directoryPath, string searchPattern)
        {
            try
            {
                //获取指定的文件列表  
                string[] fileNames = GetFileNames(directoryPath, searchPattern, false);

                //判断指定文件是否存在  
                return fileNames.Length != 0;
            }
            catch(Exception ex)
            {
                LogHelper.RecordErrorLog(ex.Message, "FileHelper——Contains");
                return false;
            }
        }

        /// <summary>  
        /// 检测指定目录中是否存在指定的文件  
        /// </summary>  
        /// <param name="directoryPath">指定目录的绝对路径</param>  
        /// <param name="searchPattern">模式字符串，"*"代表0或N个字符，"?"代表1个字符。  
        /// 范例："Log*.xml"表示搜索所有以Log开头的Xml文件。</param>   
        /// <param name="isSearchChild">是否搜索子目录</param>  
        public static bool Contains(string directoryPath, string searchPattern, bool isSearchChild)
        {
            try
            {
                //获取指定的文件列表  
                string[] fileNames = GetFileNames(directoryPath, searchPattern, true);

                //判断指定文件是否存在  
                return fileNames.Length != 0;
            }
            catch(Exception ex)
            {
                LogHelper.RecordErrorLog(ex.Message, "FileHelper——Contains");
                return false;
            }
        }
        #endregion

        #region 创建一个目录
        /// <summary>  
        /// 创建一个目录  
        /// </summary>  
        /// <param name="directoryPath">目录的绝对路径</param>  
        public static void CreateDirectory(string directoryPath)
        {
            //如果目录不存在则创建该目录  
            if (!IsExistDirectory(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }
        #endregion

        #region 创建一个文件
        /// <summary>  
        /// 创建一个文件。  
        /// </summary>  
        /// <param name="filePath">文件的绝对路径</param>  
        public static bool CreateFile(string filePath)
        {
            try
            {
                //如果文件不存在则创建该文件  
                if (!IsExistFile(filePath))
                {
                    string directoryPath = Path.GetDirectoryName(filePath);
                    if (!IsExistDirectory(directoryPath))
                    {
                        CreateDirectory(directoryPath);
                    }
                    //创建一个FileInfo对象  
                    FileInfo file = new FileInfo(filePath);
                    //创建文件  
                    FileStream fs = file.Create();
                    //关闭文件流  
                    fs.Close();
                }
            }
            catch(Exception ex)
            {
                LogHelper.RecordErrorLog(ex.Message, "FileHelper——CreateFile");
                return false;
            }

            return true;
        }

        /// <summary>  
        /// 创建一个文件,并将字节流写入文件。  
        /// </summary>  
        /// <param name="filePath">文件的绝对路径</param>  
        /// <param name="buffer">二进制流数据</param>  
        public static bool CreateFile(string filePath, byte[] buffer)
        {
            try
            {
                //如果文件不存在则创建该文件  
                if (!IsExistFile(filePath))
                {
                    string directoryPath = Path.GetFileNameWithoutExtension(filePath);
                    if (IsEmptyDirectory(directoryPath))
                    {
                        CreateDirectory(directoryPath);
                    }
                    //创建一个FileInfo对象  
                    FileInfo file = new FileInfo(filePath);

                    //创建文件  
                    FileStream fs = file.Create();

                    //写入二进制流  
                    fs.Write(buffer, 0, buffer.Length);

                    //关闭文件流  
                    fs.Close();
                }
            }
            catch(Exception ex)
            {
                LogHelper.RecordErrorLog(ex.Message, "FileHelper——CreateFile");
                return false;
            }
            return true;
        }
        #endregion

        #region 获取文本文件的行数
        /// <summary>  
        /// 获取文本文件的行数  
        /// </summary>  
        /// <param name="filePath">文件的绝对路径</param>          
        public static int GetLineCount(string filePath)
        {
            //将文本文件的各行读到一个字符串数组中  
            string[] rows = File.ReadAllLines(filePath);

            //返回行数  
            return rows.Length;
        }
        #endregion

        #region 获取一个文件的长度
        /// <summary>  
        /// 获取一个文件的长度,单位为Byte  
        /// </summary>  
        /// <param name="filePath">文件的绝对路径</param>          
        public static int GetFileSize(string filePath)
        {
            //创建一个文件对象  
            FileInfo fi = new FileInfo(filePath);

            //获取文件的大小  
            return (int)fi.Length;
        }

        /// <summary>  
        /// 获取一个文件的长度,单位为KB  
        /// </summary>  
        /// <param name="filePath">文件的路径</param>          
        public static double GetFileSizeByKB(string filePath)
        {
            //创建一个文件对象  
            FileInfo fi = new FileInfo(filePath);
            long size = fi.Length / 1024;
            //获取文件的大小  
            return double.Parse(size.ToString());
        }

        /// <summary>  
        /// 获取一个文件的长度,单位为MB  
        /// </summary>  
        /// <param name="filePath">文件的路径</param>          
        public static double GetFileSizeByMB(string filePath)
        {
            //创建一个文件对象  
            FileInfo fi = new FileInfo(filePath);
            long size = fi.Length / 1024 / 1024;
            //获取文件的大小  
            return double.Parse(size.ToString());
        }
        #endregion

        #region 获取指定目录中的文件列表
        /// <summary>  
        /// 获取指定目录中所有文件列表  
        /// </summary>  
        /// <param name="directoryPath">指定目录的绝对路径</param>          
        public static string[] GetFileNames(string directoryPath)
        {
            //如果目录不存在，则抛出异常  
            if (!IsExistDirectory(directoryPath))
            {
                throw new FileNotFoundException();
            }

            //获取文件列表  
            return Directory.GetFiles(directoryPath);
        }

        /// <summary>  
        /// 获取指定目录及子目录中所有文件列表  
        /// </summary>  
        /// <param name="directoryPath">指定目录的绝对路径</param>  
        /// <param name="searchPattern">模式字符串，"*"代表0或N个字符，"?"代表1个字符。  
        /// 范例："Log*.xml"表示搜索所有以Log开头的Xml文件。</param>  
        /// <param name="isSearchChild">是否搜索子目录</param>  
        public static string[] GetFileNames(string directoryPath, string searchPattern, bool isSearchChild)
        {
            //如果目录不存在，则抛出异常  
            if (!IsExistDirectory(directoryPath))
            {
                throw new FileNotFoundException();
            }

            try
            {
                return Directory.GetFiles(directoryPath, searchPattern, isSearchChild ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
            }
            catch(Exception ex)
            {
                LogHelper.RecordErrorLog(ex.Message, "FileHelper——GetFileNames");
                return null;
            }
        }
        #endregion

        #region 获取指定目录中的子目录列表
        /// <summary>  
        /// 获取指定目录中所有子目录列表,若要搜索嵌套的子目录列表,请使用重载方法.  
        /// </summary>  
        /// <param name="directoryPath">指定目录的绝对路径</param>          
        public static string[] GetDirectories(string directoryPath)
        {
            try
            {
                return Directory.GetDirectories(directoryPath);
            }
            catch(Exception ex)
            {
                LogHelper.RecordErrorLog(ex.Message, "FileHelper——GetDirectories");
                return null;
            }
        }

        /// <summary>  
        /// 获取指定目录及子目录中所有子目录列表  
        /// </summary>  
        /// <param name="directoryPath">指定目录的绝对路径</param>  
        /// <param name="searchPattern">模式字符串，"*"代表0或N个字符，"?"代表1个字符。  
        /// 范例："Log*.xml"表示搜索所有以Log开头的Xml文件。</param>  
        /// <param name="isSearchChild">是否搜索子目录</param>  
        public static string[] GetDirectories(string directoryPath, string searchPattern, bool isSearchChild)
        {
            try
            {
                return Directory.GetDirectories(directoryPath, searchPattern, isSearchChild ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
            }
            catch(Exception ex)
            {
                LogHelper.RecordErrorLog(ex.Message, "FileHelper——GetDirectories");
                throw null;
            }
        }
        #endregion

        #region 向文本文件写入内容
        /// <summary>  
        /// 向文本文件中写入内容  
        /// </summary>  
        /// <param name="filePath">文件的绝对路径</param>  
        /// <param name="content">写入的内容</param>          
        public static void WriteText(string filePath, string content)
        {
            //向文件写入内容  
            File.WriteAllText(filePath, content);
        }
        #endregion

        #region 向文本文件的尾部追加内容
        /// <summary>  
        /// 向文本文件的尾部追加内容  
        /// </summary>  
        /// <param name="filePath">文件的绝对路径</param>  
        /// <param name="content">写入的内容</param>  
        public static void AppendText(string filePath, string content)
        {
            try
            {
                if (!IsExistFile(filePath))
                {
                    CreateFile(filePath);
                }
                File.AppendAllText(filePath, content + "\r\n", Encoding.UTF8);
                
            } 
            catch (Exception ex)
            {
                
            }
            
        }
        #endregion

        #region 将现有文件的内容复制到新文件中
        /// <summary>  
        /// 将源文件的内容复制到目标文件中  
        /// </summary>  
        /// <param name="sourceFilePath">源文件的绝对路径</param>  
        /// <param name="destFilePath">目标文件的绝对路径</param>  
        public static void Copy(string sourceFilePath, string destFilePath)
        {
            File.Copy(sourceFilePath, destFilePath, true);
        }
        #endregion

        #region 将文件移动到指定目录
        /// <summary>  
        /// 将文件移动到指定目录  
        /// </summary>  
        /// <param name="sourceFilePath">需要移动的源文件的绝对路径</param>  
        /// <param name="descDirectoryPath">移动到的目录的绝对路径</param>  
        public static void Move(string sourceFilePath, string descDirectoryPath)
        {
            //获取源文件的名称  
            string sourceFileName = GetFileName(sourceFilePath);

            if (IsExistDirectory(descDirectoryPath))
            {
                //如果目标中存在同名文件,则删除  
                if (IsExistFile(descDirectoryPath + "\\" + sourceFileName))
                {
                    DeleteFile(descDirectoryPath + "\\" + sourceFileName);
                }
                //将文件移动到指定目录  
                File.Move(sourceFilePath, descDirectoryPath + "\\" + sourceFileName);
            }
        }
        #endregion

        #region 将流读取到缓冲区中
        /// <summary>  
        /// 将流读取到缓冲区中  
        /// </summary>  
        /// <param name="stream">原始流</param>  
        public static byte[] StreamToBytes(Stream stream)
        {
            try
            {
                //创建缓冲区  
                byte[] buffer = new byte[stream.Length];

                //读取流  
                stream.Read(buffer, 0, int.Parse(stream.Length.ToString()));

                //返回流  
                return buffer;
            }
            catch(Exception ex)
            {
                LogHelper.RecordErrorLog(ex.Message, "FileHelper——StreamToBytes");
                return null;
            }
            finally
            {
                //关闭流  
                stream.Close();
            }
        }
        #endregion

        #region 将文件读取到缓冲区中
        /// <summary>  
        /// 将文件读取到缓冲区中  
        /// </summary>  
        /// <param name="filePath">文件的绝对路径</param>  
        public static byte[] FileToBytes(string filePath)
        {
            //获取文件的大小   
            int fileSize = GetFileSize(filePath);

            //创建一个临时缓冲区  
            byte[] buffer = new byte[fileSize];

            //创建一个文件流  
            FileInfo fi = new FileInfo(filePath);
            FileStream fs = fi.Open(FileMode.Open);

            try
            {
                //将文件流读入缓冲区  
                fs.Read(buffer, 0, fileSize);

                return buffer;
            }
            catch(Exception ex)
            {
                LogHelper.RecordErrorLog(ex.Message, "FileHelper——FileToBytes");
                return null;
            }
            finally
            {
                //关闭文件流  
                fs.Close();
            }
        }
        #endregion

        #region 将文件读取到字符串中
        /// <summary>  
        /// 将文件读取到字符串中  
        /// </summary>  
        /// <param name="filePath">文件的绝对路径</param>  
        public static string FileToString(string filePath)
        {
            return FileToString(filePath, Encoding.Default);
        }
        /// <summary>  
        /// 将文件读取到字符串中  
        /// </summary>  
        /// <param name="filePath">文件的绝对路径</param>  
        /// <param name="encoding">字符编码</param>  
        public static string FileToString(string filePath, Encoding encoding)
        {
            //创建流读取器  
            StreamReader reader = new StreamReader(filePath, encoding);
            try
            {
                //读取流  
                return reader.ReadToEnd();
            }
            catch(Exception ex)
            {
                LogHelper.RecordErrorLog(ex.Message, "FileHelper——FileToString");
                return string.Empty;
            }
            finally
            {
                //关闭流读取器  
                reader.Close();
            }
        }
        #endregion

        #region 将缓冲区流写入文件中
        /// <summary>
        /// 将缓冲区流写入文件中
        /// </summary>
        /// <param name="stream">流数据</param>
        /// <param name="path">文件地址</param>
        /// <param name="lenth">流大小</param>
        public static void StreamToFile(byte[] stream, string path, int lenth)
        {
            FileStream fs = new FileStream(path, FileMode.Create);
            try
            {
                fs.Write(stream, 0, (int)lenth);//将流写到文件当中去
                fs.Flush();//清除缓冲区
                fs.Close();//关闭
            }
            catch (Exception ex)
            {
                LogHelper.RecordErrorLog(ex.Message, "FileHelper——StreamToFile");
            }


        } 
        #endregion

        #region 从文件的绝对路径中获取文件名( 包含扩展名 )
        /// <summary>  
        /// 从文件的绝对路径中获取文件名( 包含扩展名 )  
        /// </summary>  
        /// <param name="filePath">文件的绝对路径</param>          
        public static string GetFileName(string filePath)
        {
            //获取文件的名称  
            FileInfo fi = new FileInfo(filePath);
            return fi.Name;
        }
        #endregion

        #region 从文件的绝对路径中获取文件名( 不包含扩展名 )
        /// <summary>  
        /// 从文件的绝对路径中获取文件名( 不包含扩展名 )  
        /// </summary>  
        /// <param name="filePath">文件的绝对路径</param>          
        public static string GetFileNameNoExtension(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath)) return null;

            //获取文件的名称  
            FileInfo fi = new FileInfo(filePath);
            string[] fn = fi.Name.Split('.');

            string name = fi.Name.Substring(0, fi.Name.Length - fn[fn.Count() - 1].Length-1);
            return name;
        }
        #endregion

        #region 从文件的绝对路径中获取扩展名
        /// <summary>  
        /// 从文件的绝对路径中获取扩展名  
        /// </summary>  
        /// <param name="filePath">文件的绝对路径</param>          
        public static string GetExtension(string filePath)
        {
            //获取文件的名称  
            FileInfo fi = new FileInfo(filePath);
            return fi.Extension;
        }
        #endregion

        #region 清空指定目录
        /// <summary>  
        /// 清空指定目录下所有文件及子目录,但该目录依然保存.  
        /// </summary>  
        /// <param name="directoryPath">指定目录的绝对路径</param>  
        public static void ClearDirectory(string directoryPath)
        {
            if (IsExistDirectory(directoryPath))
            {
                //删除目录中所有的文件  
                string[] fileNames = GetFileNames(directoryPath);
                foreach (string t in fileNames)
                {
                    DeleteFile(t);
                }

                //删除目录中所有的子目录  
                string[] directoryNames = GetDirectories(directoryPath);
                foreach (string t in directoryNames)
                {
                    DeleteDirectory(t);
                }
            }
        }
        #endregion

        #region 清空文件内容
        /// <summary>  
        /// 清空文件内容  
        /// </summary>  
        /// <param name="filePath">文件的绝对路径</param>  
        public static void ClearFile(string filePath)
        {
            //删除文件  
            File.Delete(filePath);

            //重新创建该文件  
            CreateFile(filePath);
        }
        #endregion

        #region 删除指定文件
        /// <summary>  
        /// 删除指定文件  
        /// </summary>  
        /// <param name="filePath">文件的绝对路径</param>  
        public static void DeleteFile(string filePath)
        {
            if (IsExistFile(filePath))
            {
                File.Delete(filePath);
            }
        }
        #endregion

        #region 删除指定目录
        /// <summary>  
        /// 删除指定目录及其所有子目录  
        /// </summary>  
        /// <param name="directoryPath">指定目录的绝对路径</param>  
        public static void DeleteDirectory(string directoryPath)
        {
            if (IsExistDirectory(directoryPath))
            {
                Directory.Delete(directoryPath, true);
            }
        }
        #endregion

    }
}