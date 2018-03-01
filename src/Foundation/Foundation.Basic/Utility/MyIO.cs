using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Collections.Generic;
using System.Security.AccessControl;
using Microsoft.VisualBasic.Devices;

namespace DreamCube.Foundation.Basic.Utility
{
    /// <summary>
    /// IO方面的操作，文件/文件夹
    /// </summary>
    public static class MyIO
    {
        #region "静态方法"

        /// <summary>
        /// 获取文件的属性
        /// </summary>
        /// <param name="fileFullPath"></param>
        /// <returns></returns>
        public static FileDetails GetFileDetails(String fileFullPath)
        {
            FileInfo info = new FileInfo(fileFullPath);
            MyIO.FileDetails details = new FileDetails()
            {
                Attributes = info.Attributes,
                CreationTime = info.CreationTime,
                CreationTimeUtc = info.CreationTimeUtc,
                DirectoryName = info.DirectoryName,
                Extension = info.Extension,
                FullName = info.FullName,
                IsReadOnly = info.IsReadOnly,
                LastAccessTime = info.LastAccessTime,
                LastAccessTimeUtc = info.LastAccessTimeUtc,
                LastWriteTime = info.LastWriteTime,
                Length = info.Length,
                Name = info.Name
            };
            return details;
        }

        /// <summary>
        /// 获取目录盘符的剩余空间大小
        /// （随便传入一个物理路径即可，文件夹路径也行，会自动处理传入的参数，计算文件夹所在的盘符的剩余空间）
        /// </summary>
        /// <param name="dirPath"></param>
        /// <returns></returns>
        public static Objects.FileSystemSize GetDiskFresSize(String dirPath)
        {
            //DriveInfo[] infos = DriveInfo.GetDrives()[0].Name;
            throw new NotImplementedException();
        }

        /// <summary>
        /// 根据文件名，获取下一个文件名。（如果指定的文件名的文件存在了，则从2开始计数，会返回： 文件名（2）.扩展名 ,文件名（3）.扩展名....
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="maxCount">最大计数，以防算法死循环，如果有100个文件都是同名的，则直接返回String.empty了</param>
        /// <param name="newFileNameTemplate">新文件名的模板；{0}表示文件名部分，{1}表示计数部分，{2}表示文件后缀名部分；必须存在三个被格式化的串，否则会报错</param>
        /// <returns></returns>
        public static String GetNextFileName(String fileName, Int32 maxCount = 100, String newFileNameTemplate = "{0}({1}).{2}")
        {
            String fileNewName = fileName;
            if (MyIO.IsFileExists(fileNewName))
            {
                Int32 count = 2;
                String fileNamePart = MyString.LeftOfLast(fileName, ".");
                String fileExtensionPart = MyString.RightOfLast(fileName, ".");
                do
                {
                    fileNewName = String.Format(newFileNameTemplate, fileNamePart, count++, fileExtensionPart);
                } while (MyIO.IsFileExists(fileNewName) && count <= maxCount);
                if (count >= maxCount) return String.Empty;
            }
            return fileNewName;
        }

        /// <summary>
        /// 根据文件后缀名获得一个随机的文件名(后缀名不要加点号）
        /// </summary>
        /// <param name="fileExtension"></param>
        /// <returns></returns>
        public static String GetRandomFileName(String fileExtension)
        {
            return MyGuid.To_N(Guid.NewGuid()) + "." + fileExtension;
        }

        /// <summary>
        /// 移动文件目录
        /// </summary>
        /// <param name="path"></param>
        /// <param name="newPath"></param>
        /// <returns></returns>
#if NET20
        public static Boolean DirMove(String path, String newPath)
#else 
        public static Boolean DirMove(this String path, String newPath)
#endif
        {
            Directory.Move(path, newPath);
            return true;
        }

        /// <summary>
        /// 文件夹重命名
        /// </summary>
        /// <param name="path"></param>
        /// <param name="newName">文件夹的新名，不用输入完整的路径，仅仅输入新的文件名即可</param>
        /// <param name="newFilePath">输出新的文件夹名</param>
        /// <returns></returns>
#if NET20
        public static Boolean DirRename(String path, String newName, out String newPathName)
#else 
        public static Boolean DirRename(this String path, String newName,out String newPathName)
#endif
        {
            newPathName = String.Empty;
            path = path.Replace("/", "\\");
            if (!String.Equals(MyString.RightOfLast(path, "\\"), newName))
            {
                if (path.EndsWith("\\")) path = MyString.LeftOfLast(path, "\\");
                newPathName = MyString.LeftOfLast(path, "\\") + "\\" + newName;
                Directory.Move(path, newPathName);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 文件夹重命名
        /// </summary>
        /// <param name="path"></param>
        /// <param name="newName">文件夹的新名，不用输入完整的路径，仅仅输入新的文件名即可</param>
        /// <returns></returns>
#if NET20
        public static void DirRename(String path, String newName)
#else 
        public static void DirRename(this String path, String newName)
#endif
        {
            path = path.Replace("/", "\\");
            if (!String.Equals(MyString.RightOfLast(path, "\\"), newName, StringComparison.InvariantCultureIgnoreCase))
            {
                Computer myComputer = new Computer();
                myComputer.FileSystem.RenameDirectory(path, newName);
            }
        }

        /// <summary>
        /// 移动文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="newFilePath"></param>
        /// <returns></returns>
#if NET20
        public static Boolean FileMove(String filePath, String newFilePath)
#else
        public static Boolean FileMove(this String filePath, String newFilePath)
#endif
        {
            File.Move(filePath, newFilePath);
            return true;
        }

        /// <summary>
        /// 清空目录
        /// </summary>
        /// <param name="dir"></param>
        /// <remarks></remarks>
        public static void CleanFolder(String dir)
        {
            if (Directory.Exists(dir))
            {
                //如果存在这个文件夹删除之
                foreach (String d in Directory.GetFileSystemEntries(dir))
                {
                    if (File.Exists(d))
                    {
                        File.Delete(d);
                    }
                    else
                    {
                        //直接删除其中的文件 
                        CleanFolder(d);
                    }
                }
            }
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="filePath"></param>
#if NET20
        public static void FileDelete(String filePath)
#else
        public static void FileDelete(this String filePath)
#endif
        {
            try
            {
                if (MyIO.IsFileReadOnly(filePath)) MyIO.MakeFileCanWrite(filePath);
                File.Delete(filePath);
            }
            catch (Exception ex)
            {
                MyLog.MakeLog(ex);
            }
        }

        /// <summary>
        /// 重命名文件名
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="newName">文件的新名，不用输入完整的路径，仅仅输入新的文件名即可</param>
#if NET20
        public static void FileRename(String filePath, String newName)
#else
        public static void FileRename(this String filePath, String newName)
#endif
        {
            filePath = filePath.Replace("/", "\\");
            if (!String.Equals(MyString.RightOfLast(filePath, "\\"), newName, StringComparison.InvariantCultureIgnoreCase))
            {
                Computer myComputer = new Computer();
                myComputer.FileSystem.RenameFile(filePath, newName);
            }
        }

        /// <summary>
        /// 文件重命名
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="newName">指定新的文件名；注意：是传入文件名即可，不是完整的文件路径，此参数可以不包含文件的扩展名，也可以包含</param>
        /// <param name="newFilePath">输出新的文件完整路径</param>
        /// <returns></returns>
#if NET20
        public static Boolean FileRename(String filePath, String newName, out String newFilePath)
#else
        public static Boolean FileRename(this String filePath, String newName, out String newFilePath)
#endif
        {
            newFilePath = String.Empty;
            filePath = filePath.Replace("/", "\\");
            if (!String.Equals(MyString.RightOfLast(filePath, "\\"), newName, StringComparison.InvariantCultureIgnoreCase))
            {
                if (filePath.EndsWith("\\")) filePath = MyString.LeftOfLast(filePath, "\\");
                String extensionName = MyString.RightOfLast(filePath, ".").ToLower();
                if (!newName.ToLower().EndsWith(extensionName)) newName += "." + extensionName;
                newFilePath = Path.Combine(MyString.LeftOfLast(filePath, "\\"), newName);
                File.Move(filePath, newFilePath);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 判断文件是否存在（如果发生异常，则返回false）
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
#if NET20
        public static Boolean IsFileExists(String filePath)
#else
        public static Boolean IsFileExists(this String filePath)
#endif
        {
            if (String.IsNullOrEmpty(filePath)) return false;
            try
            {
                return File.Exists(filePath);
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 判断文件夹是否存在（如果发生异常，则返回false）
        /// </summary>
        /// <returns></returns>
#if NET20
        public static Boolean IsDirExits(String dir)
#else
        public static Boolean IsDirExits(this String dir)
#endif
        {
            if (String.IsNullOrEmpty(dir)) return false;
            try
            {
                return Directory.Exists(dir);
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 获取文件的大小
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
#if NET20
        public static Objects.FileSystemSize GetFileSize(String filePath)
#else
        public static Objects.FileSystemSize GetFileSize(this String filePath)
#endif
        {
            if (File.Exists(filePath))
            {
                FileInfo file = new FileInfo(filePath);
                return new Objects.FileSystemSize((UInt64)file.Length, Objects.FileSystemSize.HardDiskSizeType.B);
            }
            else return new Objects.FileSystemSize(0, Objects.FileSystemSize.HardDiskSizeType.B);
        }

        /// <summary>
        /// 确保给定的目录是有效的，将递归搜索给定字符串来执行分析；
        /// 注意：确保是传入的参数是目录路径而非文件路径，例如：传入：c:\dd.txt；则此方法会创建dd.txt文件夹的
        /// </summary>
        /// <param name="path"></param>
#if NET20
        public static void EnsurePath(String path)
#else
        public static void EnsurePath(this String path)
#endif
        {
            if (String.IsNullOrEmpty(path)) return;
            Directory.CreateDirectory(path);
            //path = path.Replace("/", "\\");
            //while (true)
            //{
            //    if (Directory.Exists(path)) return;
            //    else
            //    {
            //        String parentPath = MyString.LeftOfLast(path, "\\");
            //        EnsurePath(parentPath);
            //        Directory.CreateDirectory(path);
            //    }
            //}
        }

        /// <summary>
        /// 确保给定的目录是有效的，将递归搜索给定字符串来执行分析；
        /// 注意：确保是传入的参数是目录路径而非文件路径，例如：传入：c:\dd.txt；则此方法会创建dd.txt文件夹的
        /// </summary>
        /// <param name="path"></param>
        public static Boolean TryEnsurePath(String path)
        {
            try
            {
                if (String.IsNullOrEmpty(path)) return false;
                Directory.CreateDirectory(path);
                return true;
            }
            catch (Exception ex)
            {
                if (MyLog.SaveFuncCallParamData)
                {
                    MyLog.MakeLog(ex, MyLog.LogTarget.TextFile, path);
                }
                else
                {
                    MyLog.MakeLog(ex);
                }
            }
            return false;
        }

        /// <summary>
        /// 判断指定的文件是否正在使用
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Boolean IsFileUsing(String path)
        {
            if (String.IsNullOrEmpty(path)) return false;
            try
            {
                using (FileStream fs = File.OpenRead(path))
                {
                    Byte[] buffer = new Byte[1];
                    fs.Read(buffer, 0, buffer.Length);
                }
                return false;
            }
            catch (IOException)
            {
                return true;
            }
        }

        /// <summary>
        /// 删除文件，删除成功返回：true，删除失败，返回false
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Boolean TryDeleteFile(String path, Enums.HandleExceptionInTry handleExcp = Enums.HandleExceptionInTry.ReturnAndIgnoreLog)
        {
            try
            {
                File.Delete(path);
                return true;
            }
            catch (Exception ex)
            {
                switch (handleExcp)
                {
                    case Enums.HandleExceptionInTry.ReturnAndIgnoreLog:
                        return false;
                    case Enums.HandleExceptionInTry.ReturnAndMakeLog:
                        MyLog.MakeLog(ex);
                        return false;
                    case Enums.HandleExceptionInTry.ThrowException:
                    default:
                        throw;
                }
            }
        }

        /// <summary>
        /// 根据目录获取所有文件列表
        /// </summary>
        /// <param name="target"></param>
        /// <param name="files"></param>
        /// <param name="searchPattern">
        /// 匹配的通配符，指定多种文件格式时，需要用逗号隔开；
        /// 非常注意：
        /// ①恰好为三个字符的 searchPattern 返回扩展名为三个或三个以上字符的文件。
        /// “*.abc”返回扩展名为 .abc、.abcd、.abcde、.abcdef 等的文件。 
        /// ②一个字符、两个字符或三个以上字符的 searchPattern 只返回扩展名恰好等于该长度的文件
        /// </param>
        /// <param name="searchOption"></param>
        /// <returns></returns>
#if NET20
        public static Boolean TryGetFiles(String target, out String[] files, String searchPattern = "*", SearchOption searchOption = SearchOption.TopDirectoryOnly)
#else
        public static Boolean TryGetFiles(this String target, out String[] files, String searchPattern = "*", SearchOption searchOption = SearchOption.TopDirectoryOnly)
#endif
        {
            files = null;
            Boolean result = false;
            if (String.IsNullOrEmpty(target)) return false;
            if (!Directory.Exists(target)) return false;
            if (searchPattern != "*")
            {
                List<String> fileList = new List<String>();
                String[] searchPatterns = MyString.SplitEx(searchPattern, ",", StringSplitOptions.RemoveEmptyEntries);
                String[] tempFiles = null;
                for (Int32 i = 0; i < searchPatterns.Length; i++)
                {
                    tempFiles = Directory.GetFiles(target, searchPatterns[i], searchOption);
                    if (tempFiles != null && tempFiles.Length > 0)
                    {
                        for (Int32 j = 0; j < tempFiles.Length; j++)
                        {
                            if (!fileList.Contains(tempFiles[j]))
                                fileList.Add(tempFiles[j]);
                        }
                    }
                    tempFiles = null;
                }
                files = fileList.ToArray();
                result = files.Length > 0;
            }
            else
            {
                files = Directory.GetFiles(target, searchPattern, searchOption);
                result = files.Length > 0;
            }
            return result;
        }

        /// <summary>
        /// 根据目录获取所有的子目录信息
        /// </summary>
        /// <param name="target"></param>
        /// <param name="dirs"></param>
        /// <param name="searchPattern">
        /// 匹配的通配符，指定多种文件格式时，需要用逗号隔开；
        /// 非常注意：
        /// ①恰好为三个字符的 searchPattern 返回扩展名为三个或三个以上字符的文件。
        /// “*.abc”返回扩展名为 .abc、.abcd、.abcde、.abcdef 等的文件。 
        /// ②一个字符、两个字符或三个以上字符的 searchPattern 只返回扩展名恰好等于该长度的文件
        /// </param>
        /// <param name="searchOption"></param>
        /// <returns></returns>
#if NET20
        public static Boolean TryGetDirs(String target, out String[] dirs, String searchPattern = "*", SearchOption searchOption = SearchOption.TopDirectoryOnly)
#else
        public static Boolean TryGetDirs(this String target, out String[] dirs, String searchPattern = "*", SearchOption searchOption = SearchOption.TopDirectoryOnly)
#endif
        {
            dirs = null;
            if (String.IsNullOrEmpty(target)) return false;
            if (!Directory.Exists(target)) return false;
            dirs = Directory.GetDirectories(target, searchPattern, searchOption);

            dirs = null;
            Boolean result = false;
            if (String.IsNullOrEmpty(target)) return false;
            if (!Directory.Exists(target)) return false;
            if (searchPattern != "*")
            {
                List<String> dirsList = new List<String>();
                String[] searchPatterns = MyString.SplitEx(searchPattern, ",", StringSplitOptions.RemoveEmptyEntries);
                String[] tempDirs = null;
                for (Int32 i = 0; i < searchPatterns.Length; i++)
                {
                    tempDirs = Directory.GetDirectories(target, searchPatterns[i], searchOption);
                    if (tempDirs != null && tempDirs.Length > 0)
                    {
                        for (Int32 j = 0; j < tempDirs.Length; j++)
                        {
                            if (!dirsList.Contains(tempDirs[j]))
                                dirsList.Add(tempDirs[j]);
                        }
                    }
                    tempDirs = null;
                }
                dirs = dirsList.ToArray();
                result = dirs.Length > 0;
            }
            else
            {
                dirs = Directory.GetDirectories(target, searchPattern, searchOption);
                result = dirs.Length > 0;
            }
            return result;
        }

        /// <summary>
        /// 获取目录下面的子目录和文件，Directory.GetFileSystemEntries()方法获取顶层文件和文件夹数据
        /// </summary>
        /// <param name="target"></param>
        /// <param name="dirs">收集到的所有目录</param>
        /// <param name="files">收集到的所有文件</param>
        /// <param name="searchPattern">
        /// 匹配的通配符，指定多种文件格式时，需要用逗号隔开；
        /// 非常注意：
        /// ①恰好为三个字符的 searchPattern 返回扩展名为三个或三个以上字符的文件。
        /// “*.abc”返回扩展名为 .abc、.abcd、.abcde、.abcdef 等的文件。 
        /// ②一个字符、两个字符或三个以上字符的 searchPattern 只返回扩展名恰好等于该长度的文件
        /// </param>
        /// <param name="searchOption"></param>
        /// <returns></returns>
#if NET20
        public static Boolean TryGetDirsAndFiles(String target,
                                                 out String[] dirs,
                                                 out String[] files,
                                                 String searchPattern = "*",
                                                 SearchOption searchOption = SearchOption.TopDirectoryOnly)
#else
        public static Boolean TryGetDirsAndFiles(this String target,
                                                 out String[] dirs,
                                                 out String[] files,
                                                 String searchPattern = "*",
                                                 SearchOption searchOption = SearchOption.TopDirectoryOnly)
#endif
        {
            dirs = null;
            files = null;
            Boolean getSir = TryGetDirs(target, out dirs, searchPattern, searchOption);
            Boolean getFile = TryGetFiles(target, out files, searchPattern, searchOption);
            return getSir || getFile;
        }

        /// <summary>
        /// 根据目录读取文件内容
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
#if NET20
        public static String ReadText(String filePath, Encoding encoding = null)
#else
        public static String ReadText(this String filePath, Encoding encoding = null)
#endif
        {
            if (String.IsNullOrEmpty(filePath)) return String.Empty;
            if (!File.Exists(filePath)) return String.Empty;
            if (encoding == null)
            {
                return File.ReadAllText(filePath, GetFileEncodeType(filePath));
            }
            return File.ReadAllText(filePath, encoding);
        }

        /// <summary>
        /// ReadText()方法的Try版本
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="encoding"></param>
        /// <param name="fileValue"></param>
        /// <param name="hExInTry"></param>
        /// <returns></returns>
        public static Boolean TryReadText(String filePath, Encoding encoding, out String fileValue, Enums.HandleExceptionInTry hExInTry = Enums.HandleExceptionInTry.ReturnAndIgnoreLog)
        {
            fileValue = "";
            try
            {
                fileValue = ReadText(filePath, encoding);
                return true;
            }
            catch (Exception ex)
            {
                if (hExInTry == Enums.HandleExceptionInTry.ReturnAndIgnoreLog)
                    return false;
                else if (hExInTry == Enums.HandleExceptionInTry.ReturnAndMakeLog)
                {
                    MyLog.MakeLog(ex);
                    return false;
                }
                else if (hExInTry == Enums.HandleExceptionInTry.ThrowException)
                    throw;
            }
            return false;
        }

        /// <summary>
        /// 从文件中读取字节流
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="offset">文件流中从零开始的字节偏移量，从此处开始读取返回字节，必须大于0，小于流的长度</param>
        /// <param name="length">返回字节的长度，如果为小于或等于0时，则返回全部字节，默认为-1</param>
        /// <returns>如果传入文件路径为null或者空串，则返回null</returns>
#if NET20
        public static Byte[] ReadByte(String filePath, Int32 offset = 0, Int32 length = -1)
#else
        public static Byte[] ReadByte(this String filePath, Int32 offset = 0, Int32 length = -1)
#endif
        {
            if (String.IsNullOrEmpty(filePath)) return null;
            if (length == -1) return File.ReadAllBytes(filePath);
            else
            {
                Byte[] fileData = new Byte[length];
                FileStream fs = File.OpenRead(filePath);
                fs.Read(fileData, offset, length);
                return fileData;
            }
        }

        /// <summary>
        /// 判断是不是一个绝对路径
        /// </summary>
        /// <param name="sFilePath"></param>
        /// <returns></returns>
        public static bool IsAbsoluteFilePath(string sFilePath)
        {
            if (sFilePath.Substring(1).StartsWith(":\\"))
                return true;
            if (sFilePath.StartsWith("\\\\"))
                return true;
            return false;
        }

        /// <summary>
        /// 判断目录路径是否合法
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
#if NET20
        public static Boolean IsValidatePath(String path)
#else
        public static Boolean IsValidatePath(this String path)
#endif
        {
            try
            {
                new DirectoryInfo(path);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 根据字符串判断是否合法的文件路径
        /// </summary>
        /// <returns></returns>
#if NET20
        public static Boolean IsValidateFilePath(String filePath)
#else
        public static Boolean IsValidateFilePath(this String filePath)
#endif
        {
            try
            {
                new FileInfo(filePath);
                return true;
            }
            //catch (ArgumentNullException)
            //{
            //    /// fileName 为 nullNothingnullptrnull 引用（在 Visual Basic 中为 Nothing）。
            //    return false;
            //}
            //catch (ArgumentException)
            //{
            //    /// 文件名为空，只包含空白，或包含无效字符。
            //    return false;
            //}
            //catch (PathTooLongException)
            //{
            //    /// 指定的路径、文件名或者两者都超出了系统定义的最大长度。例如，在基于 Windows 的平台上，路径必须小于 248 个字符，文件名必须小于 260 个字符。
            //    return false;
            //}

            //catch (NotSupportedException)
            //{
            //    /// fileName 字符串中间有一个冒号 (:)。
            //    return false;
            //}
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 创建或覆盖具有指定的缓冲区大小、文件选项和文件安全性的指定文件。
        /// 此方法会自动递归创建不存在的目录；例如：传入：c:\dd\aa.txt；当dd目录不存在时，方法会自动创建
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="bufferSize">文件读进缓冲区的字节数</param>
        /// <param name="options">描述如何创建和覆盖文件</param>
        /// <param name="fileSecurity">描述文件的访问控制</param>
        /// <returns></returns>

#if NET20
        public static FileStream CreateFile(String filePath,
                                            Int32 bufferSize = -1,
                                            FileOptions options = FileOptions.None,
                                            FileSecurity fileSecurity = null)
#else
        public static FileStream CreateFile(this String filePath,
                                            Int32 bufferSize = -1,
                                            FileOptions options = FileOptions.None,
                                            FileSecurity fileSecurity = null)
#endif
        {
            if (String.IsNullOrEmpty(filePath)) return null;
            filePath = filePath.Replace("/", "\\");
            MyIO.EnsurePath(MyString.LeftOfLast(filePath, "\\"));
            if (bufferSize < 0)
                return File.Create(filePath, 0, options, fileSecurity);
            else
                return File.Create(filePath, bufferSize, options, fileSecurity);
        }

        /// <summary>
        /// 打开一个现有文件或创建一个文件;
        /// 此方法会自动递归创建不存在的目录；例如：传入：c:\dd\aa.txt；当dd目录不存在时，方法会自动创建
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="fileData">文件的二进制数据</param>
        /// <param name="append">表示是否追加内容，true：追加写入；false：全新写入</param>
        /// <returns></returns>
#if NET20
        public static void Write(String filePath, Byte[] fileData, Boolean append = true)
#else
        public static void Write(this String filePath, Byte[] fileData, Boolean append = true)
#endif
        {
            if (String.IsNullOrEmpty(filePath)) return;
            filePath = filePath.Replace("/", "\\");
            MyIO.EnsurePath(MyString.LeftOfLast(filePath, "\\"));
            FileInfo file = new FileInfo(filePath);
            using (FileStream fs = file.OpenWrite())
            {
                if (append) fs.Position = fs.Length;
                fs.Write(fileData, 0, fileData.Length);
                fs.Close();
            }
        }

        /// <summary>
        /// 打开一个现有文件或创建一个文件(对路径进行了判断处理，如果文件的父目录不存在，则会自动创建，调用方不用判断了)
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="textValue"></param>
        /// <param name="append">表示是否追加内容，true：追加写入；false：全新写入</param>
        /// <param name="encoding">指定写入内容的编码；默认使用utf8编码格式</param>
#if NET20
        public static void Write(String filePath, String textValue, Boolean append = true, Encoding encoding = null)
#else
        public static void Write(this String filePath, String textValue, Boolean append = true, Encoding encoding = null)
#endif
        {
            if (String.IsNullOrEmpty(filePath)) return;
            filePath = filePath.Replace("/", "\\");
            MyIO.EnsurePath(MyString.LeftOfLast(filePath, "\\"));
            using (StreamWriter sw = new StreamWriter(filePath, append, encoding == null ? Encoding.UTF8 : encoding))
            {
                sw.Write(textValue);
            }
        }

        /// <summary>
        /// 打开一个现有文件或创建一个文件(对路径进行了判断处理，如果文件的父目录不存在，则会自动创建，调用方不用判断了)
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="textValue"></param>
        /// <param name="append">表示是否追加内容，true：追加写入；false：全新写入</param>
        /// <param name="encoding">指定写入内容的编码；默认使用utf8编码格式</param>
        /// <param name="needCompression">是否需要压缩文本内容</param>
        public static Boolean TryWrite(String filePath, String textValue, Boolean append = true, Encoding encoding = null, Boolean needCompression = false, Enums.HandleExceptionInTry handleExcp = Enums.HandleExceptionInTry.ThrowException)
        {
            try
            {
                if (needCompression)
                {
                    //使用Using的好处是 可以马上释放文件句柄，即使是异常
                    using (System.IO.FileStream f = File.Open(filePath, append ? FileMode.Append : FileMode.CreateNew))
                    {
                        using (GZipStream oCompress = new GZipStream(f, CompressionMode.Compress))
                        {
                            using (StreamWriter oStreamWriter = new StreamWriter(oCompress))
                            {
                                oStreamWriter.Write(textValue);
                            }
                        }
                    }
                }
                else
                {
                    if (String.IsNullOrEmpty(filePath)) return false;
                    filePath = filePath.Replace("/", "\\");
                    MyIO.EnsurePath(MyString.LeftOfLast(filePath, "\\"));
                    using (StreamWriter sw = new StreamWriter(filePath, append, encoding == null ? Encoding.UTF8 : encoding))
                    {
                        sw.Write(textValue);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                switch (handleExcp)
                {
                    case Enums.HandleExceptionInTry.ReturnAndIgnoreLog:
                        return false;
                    case Enums.HandleExceptionInTry.ReturnAndMakeLog:
                        MyLog.MakeLog(ex);
                        return false;
                    default:
                    case Enums.HandleExceptionInTry.ThrowException:
                        throw;
                }
            }
        }

        /// <summary>
        /// 打开一个现有文件或创建一个文件(对路径进行了判断处理，如果文件的父目录不存在，则会自动创建，调用方不用判断了)
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="textValue"></param>
        /// <param name="append">表示是否追加内容，true：追加写入；false：全新写入</param>
        /// <param name="encoding">指定写入内容的编码；默认使用utf8编码格式</param>
        /// <param name="needCompression">是否需要压缩文本内容</param>
        public static void Write(String filePath, String textValue, Boolean append = true, Encoding encoding = null, Boolean needCompression = false)
        {
            if (needCompression)
            {
                //使用Using的好处是 可以马上释放文件句柄，即使是异常
                using (System.IO.FileStream f = File.Open(filePath, append ? FileMode.Append : FileMode.CreateNew))
                {
                    using (GZipStream oCompress = new GZipStream(f, CompressionMode.Compress))
                    {
                        using (StreamWriter oStreamWriter = new StreamWriter(oCompress))
                        {
                            oStreamWriter.Write(textValue);
                        }
                    }
                }
            }
            else
            {
                if (String.IsNullOrEmpty(filePath)) return;
                filePath = filePath.Replace("/", "\\");
                MyIO.EnsurePath(MyString.LeftOfLast(filePath, "\\"));
                using (StreamWriter sw = new StreamWriter(filePath, append, encoding == null ? Encoding.UTF8 : encoding))
                {
                    sw.Write(textValue);
                }
            }
        }

        /// <summary>
        /// 批量复制文件
        /// </summary>
        /// <param name="filePaths">源文件路径数组</param>
        /// <param name="desPath">目标目录（不要包括文件名）</param>
        /// <param name="desFilePath">输出参数，新文件的完整目录（包括文件名）【与传入的源文件的数组的顺序一一对应】</param>
        /// <param name="overwrite">如果目标目录存在同名的文件，是否替换；true为替换；false不替换；默认替换</param>
        /// <returns></returns>
        public static Boolean TryFileCopy(String[] filePaths, String desPath, out String[] desFilePath, Boolean overwrite = true, Enums.HandleExceptionInTry handleExcp = Enums.HandleExceptionInTry.ThrowException)
        {
            desFilePath = null;
            try
            {
                desFilePath = MyIO.FileCopy(filePaths, desPath, overwrite);
                return true;
            }
            catch (Exception ex)
            {
                switch (handleExcp)
                {
                    case Enums.HandleExceptionInTry.ReturnAndIgnoreLog:
                        return false;
                    case Enums.HandleExceptionInTry.ReturnAndMakeLog:
                        MyLog.MakeLog(ex);
                        return false;
                    default:
                    case Enums.HandleExceptionInTry.ThrowException:
                        throw;
                }
            }
        }

        /// <summary>
        /// 批量复制文件
        /// </summary>
        /// <param name="filePaths">源文件路径数组</param>
        /// <param name="desPath">目标目录（不要包括文件名）</param>
        /// <param name="overwrite">如果目标目录存在同名的文件，是否替换；true为替换；false不替换；默认替换</param>
        /// <returns>新文件的完整目录（包括文件名）【与传入的源文件的数组的顺序一一对应】</returns>
        public static String[] FileCopy(String[] filePaths, String desPath, Boolean overwrite = true)
        {
            MyIO.EnsurePath(desPath);
            List<String> filePath = new List<String>();
            for (Int32 i = 0; i < filePaths.Length; i++)
            {
                filePath.Add(MyIO.FileCopy(filePaths[i], desPath, overwrite, "", true));
            }
            return filePath.ToArray();
        }

        /// <summary>
        /// 打开一个文件，将文件的内容读入一个字符串，然后关闭该文件。
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="result">输出值</param>
        /// <param name="defaultValue">程序异常时返回的默认值</param>
        /// <param name="handleExcp"></param>
        /// <returns></returns>
        public static Boolean TryReadFileAllBytes(String filePath, out byte[] result, Enums.HandleExceptionInTry handleExcp = Enums.HandleExceptionInTry.ReturnAndMakeLog)
        {
            result = null;
            try
            {
                result = File.ReadAllBytes(filePath);
                return true;
            }
            catch (Exception ex)
            {
                switch (handleExcp)
                {
                    case Enums.HandleExceptionInTry.ReturnAndIgnoreLog:
                        return false;
                    case Enums.HandleExceptionInTry.ReturnAndMakeLog:
                        MyLog.MakeLog(ex);
                        return false;
                    default:
                    case Enums.HandleExceptionInTry.ThrowException:
                        throw;
                }
            }
        }

        /// <summary>
        /// 复制文件
        /// </summary>
        /// <param name="filePaths">源文件路径数组</param>
        /// <param name="desPath">目标目录（不要包括文件名）</param>
        /// <param name="desFilePath">输出参数，新文件的完整目录（包括文件名）【与传入的源文件的数组的顺序一一对应】</param>
        /// <param name="overwrite">如果目标目录存在同名的文件，是否替换；true为替换；false不替换；默认替换</param>
        /// <returns></returns>
        public static Boolean TryFileCopy(String filePath, String destPath, Boolean overwrite = true, String newFileName = "", Boolean doNotEnsurePath = false, Enums.HandleExceptionInTry handleExcp = Enums.HandleExceptionInTry.ReturnAndMakeLog)
        {
            try
            {
                if (String.IsNullOrEmpty(filePath) || String.IsNullOrEmpty(destPath))
                    throw new ArgumentNullException("filePath OR destFilePath");
                if (!doNotEnsurePath) MyIO.EnsurePath(destPath);
                filePath = filePath.Replace("/", "\\");
                String destFilePath = "";
                if (String.IsNullOrEmpty(newFileName))
                    destFilePath = Path.Combine(destPath, MyString.RightOfLast(filePath, "\\"));
                else
                    destFilePath = Path.Combine(destPath, newFileName);
                MyIO.MakeFileCanWrite(destFilePath);
                File.Copy(filePath, destFilePath, overwrite);
                return true;
            }
            catch (Exception ex)
            {
                switch (handleExcp)
                {
                    case Enums.HandleExceptionInTry.ReturnAndIgnoreLog:
                        return false;
                    case Enums.HandleExceptionInTry.ReturnAndMakeLog:
                        MyLog.MakeLog(ex);
                        return false;
                    default:
                    case Enums.HandleExceptionInTry.ThrowException:
                        throw;
                }
            }
        }

        /// <summary>
        /// 复制文件
        /// </summary>
        /// <param name="filePath">源文件</param>
        /// <param name="destPath">目标目录（不要包括文件名）</param>
        /// <param name="overwrite">如果目标目录存在同名的文件，是否替换；true为替换；false不替换；默认替换</param>
        /// <param name="newFileName">是否指定新文件名</param>
        /// <param name="doNotEnsurePath">是否需要确保目录存在，默认是false，方法内部会确定目标目录会存在（但是会有效率上的稍差）</param>
        /// <returns>新文件的完整目录（包括文件名）</returns>
#if NET20
        public static String FileCopy(String filePath, String destPath, Boolean overwrite = true, String newFileName = "", Boolean doNotEnsurePath = false)
#else
        public static String FileCopy(this String filePath, String destPath, Boolean overwrite = true, String newFileName = "", Boolean doNotEnsurePath = false)
#endif
        {
            if (String.IsNullOrEmpty(filePath) || String.IsNullOrEmpty(destPath))
                throw new ArgumentNullException("filePath OR destFilePath");
            if (!doNotEnsurePath) MyIO.EnsurePath(destPath);
            filePath = filePath.Replace("/", "\\");
            String destFilePath = "";
            if (String.IsNullOrEmpty(newFileName))
                destFilePath = Path.Combine(destPath, MyString.RightOfLast(filePath, "\\"));
            else
                destFilePath = Path.Combine(destPath, newFileName);
            MyIO.MakeFileCanWrite(destFilePath);
            File.Copy(filePath, destFilePath, overwrite);
            return destFilePath;
        }

        /// <summary>
        /// 复制文件夹
        /// </summary>
        /// <param name="dirPath"></param>
        /// <param name="destFilePath">需要复制到的目标位置路径</param>
        /// <param name="overwrite"></param>
        /// <param name="deleteSourceDir">是否删除原目录</param>
#if NET20
        public static void DirCopy(String dirPath, String destPath, Boolean overwrite = true)
#else
        public static void DirCopy(this String dirPath, String destPath, Boolean overwrite = true)
#endif
        {
            if (String.IsNullOrEmpty(dirPath) || String.IsNullOrEmpty(destPath))
                throw new ArgumentNullException("dirPath OR destPath");

            if (dirPath.EndsWith("\\"))
                dirPath = MyString.LeftOfLast(dirPath, "\\");

            if (destPath.EndsWith("\\"))
                destPath = MyString.LeftOfLast(destPath, "\\");

            String[] dirs = null;
            String[] files = null;
            MyIO.TryGetDirs(dirPath, out dirs, "*", SearchOption.AllDirectories);
            MyIO.TryGetFiles(dirPath, out files, "*", SearchOption.AllDirectories);

            //创建需要复制的根目录
            MyIO.EnsurePath((destPath + "\\" + MyString.RightOfLast(dirPath, "\\")));
            //去掉最后一层目录，获得前面的父目录
            String tempParentDirPath = MyString.LeftOfLast(dirPath, "\\");
            if (dirs != null)
            {
                String newDirPath = null;
                for (Int32 i = 0; i < dirs.Length; i++)
                {
                    newDirPath = destPath + "\\" + MyString.Right(dirs[i], tempParentDirPath);
                    MyIO.EnsurePath(newDirPath);
                }
            }

            if (files != null)
            {
                String newFilePath = null;
                for (Int32 i = 0; i < files.Length; i++)
                {
                    newFilePath = destPath + MyString.Right(files[i], tempParentDirPath);
                    File.Copy(files[i], newFilePath, overwrite);
                }
            }
        }

        /// <summary>
        /// 删除文件夹
        /// </summary>
        /// <param name="dirPath"></param>
        /// <param name="recusive">是否递归删除目录下面的所有子目录以及文件，默认为true，如果为false，当目录不为空时则会抛出异常</param>
#if NET20
        public static void DirDelete(String dirPath, Boolean recursive = true)
#else
        public static void DirDelete(this String dirPath, Boolean recursive = true)
#endif
        {
            if (!Directory.Exists(dirPath)) return;
            if (recursive)
            {
                String[] files = null;
                if (MyIO.TryGetFiles(dirPath, out files, "*", SearchOption.AllDirectories))
                {
                    for (Int32 i = 0, j = files.Length; i < j; i++)
                        MyIO.MakeFileCanWrite(files[i]);
                }
            }
            Directory.Delete(dirPath, recursive);
        }

        /// <summary>
        /// 设置文件的一些属性
        /// </summary>
        /// <param name="filePath"></param>
#if NET20
        public static void SetFileAttributes(String filePath, FileAttributes attributes)
#else
        public static void SetFileAttributes(this String filePath, FileAttributes attributes)
#endif
        {
            File.SetAttributes(filePath, attributes);
        }

        /// <summary>
        /// 判断文件是否只读
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool IsFileReadOnly(string filePath)
        {
            FileInfo fi = new FileInfo(filePath);
            return fi.IsReadOnly;
        }

        /// <summary>
        /// 设置文件只读
        /// </summary>
        /// <param name="filePath"></param>
        public static void MakeFileCannotWrite(String filePath)
        {
            FileInfo fi = new FileInfo(filePath);
            fi.IsReadOnly = true;
        }

        /// <summary>
        /// 设置文件可写
        /// </summary>
        /// <param name="filePath"></param>
#if NET20
        public static void MakeFileCanWrite(String filePath)
#else
        public static void MakeFileCanWrite(this String filePath)
#endif
        {
            if (File.Exists(filePath))
                File.SetAttributes(filePath, File.GetAttributes(filePath) & ~FileAttributes.ReadOnly);
        }

        /// <summary>
        /// 设置文件可写
        /// </summary>
        /// <param name="filePath"></param>
        public static Boolean TryMakeFileCanWrite(String filePath, Enums.HandleExceptionInTry handleExcp = Enums.HandleExceptionInTry.ReturnAndMakeLog)
        {
            try
            {
                MakeFileCanWrite(filePath);
                return true;
            }
            catch (Exception ex)
            {
                switch (handleExcp)
                {
                    case Enums.HandleExceptionInTry.ReturnAndIgnoreLog:
                        return false;
                    case Enums.HandleExceptionInTry.ReturnAndMakeLog:
                        MyLog.MakeLog(ex);
                        return false;
                    default:
                    case Enums.HandleExceptionInTry.ThrowException:
                        throw;
                }
            }
        }

        /// <summary>
        /// 检查文件的真实类型（不是根据后缀名去查的）
        /// </summary>
        /// <param name="path"></param>
        public static String CheckTrueFileName(String path)
        {
            System.IO.FileStream fs = new System.IO.FileStream(path, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            System.IO.BinaryReader r = new System.IO.BinaryReader(fs);
            string bx = " ";
            byte buffer;
            try
            {
                buffer = r.ReadByte();
                bx = buffer.ToString();
                buffer = r.ReadByte();
                bx += buffer.ToString();
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
            r.Close();
            fs.Close();
            //真实的文件类型
            return bx;
        }

        /// <summary>
        /// 获取文件的编码类型
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static Encoding GetFileEncodeType(String filename)
        {
            using (FileStream fs = new FileStream(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                BinaryReader br = new BinaryReader(fs);
                Byte[] buffer = br.ReadBytes(2);
                if (buffer[0] >= 0xEF)
                {
                    if (buffer[0] == 0xEF && buffer[1] == 0xBB)
                    {
                        return System.Text.Encoding.UTF8;
                    }
                    else if (buffer[0] == 0xFE && buffer[1] == 0xFF)
                    {
                        return System.Text.Encoding.BigEndianUnicode;
                    }
                    else if (buffer[0] == 0xFF && buffer[1] == 0xFE)
                    {
                        return System.Text.Encoding.Unicode;
                    }
                    else
                    {
                        return System.Text.Encoding.Default;
                    }
                }
                else
                {
                    return System.Text.Encoding.Default;
                }
            }
        }

        /// <summary>
        ///  文件类型的枚举值
        /// </summary>
        public enum FileExtension
        {
            JPG = 255216,
            GIF = 7173,
            BMP = 6677,
            PNG = 13780,
            COM = 7790,
            EXE = 7790,
            DLL = 7790,
            RAR = 8297,
            ZIP = 8075,
            XML = 6063,
            HTML = 6033,
            ASPX = 239187,
            CS = 117115,
            JS = 119105,
            TXT = 210187,
            SQL = 255254,
            BAT = 64101,
            BTSEED = 10056,
            RDP = 255254,
            PSD = 5666,
            PDF = 3780,
            CHM = 7384,
            LOG = 70105,
            REG = 8269,
            HLP = 6395,
            DOC = 208207,
            XLS = 208207,
            DOCX = 208207,
            XLSX = 208207,
        }

        #endregion

        #region "内部类"

        /// <summary>
        /// 描述文件的基本信息
        /// </summary>
        public class FileDetails
        {
            /// <summary>
            /// 返回和文件相关的属性值，运用了FileAttributes枚举类型值
            /// </summary>
            public FileAttributes Attributes
            {
                get;
                set;
            }

            /// <summary>
            /// 返回文件的创建时间
            /// </summary>
            public DateTime CreationTime
            {
                get;
                set;
            }

            /// <summary>
            /// 返回文件的创建时间
            /// </summary>
            public DateTime CreationTimeUtc
            {
                get;
                set;
            }

            /// <summary>
            /// 目录的完整路径字符串
            /// </summary>
            public String DirectoryName
            {
                get; set;
            }

            /// <summary>
            /// 文件扩展名部分字符串
            /// </summary>
            public String Extension
            {
                get; set;
            }

            /// <summary>
            /// 文件是否为只读
            /// </summary>
            public Boolean IsReadOnly
            {
                get; set;
            }

            /// <summary>
            /// 返回文件的上次访问时间
            /// </summary>
            public DateTime LastAccessTime
            {
                get; set;
            }

            /// <summary>
            /// 返回文件的上次访问时间
            /// </summary>
            public DateTime LastAccessTimeUtc
            {
                get; set;
            }

            /// <summary>
            /// 返回文件的上次写操作时间
            /// </summary>
            public DateTime LastWriteTime
            {
                get; set;
            }

            /// <summary>
            /// 文件的大小
            /// </summary>
            public Single Length
            {
                get; set;
            }

            /// <summary>
            /// 文件的完整路径名和文件名
            /// </summary>
            public String FullName
            {
                get; set;
            }

            /// <summary>
            /// 文件名
            /// </summary>
            public String Name
            {
                get; set;
            }
        }

        #endregion
    }
}
