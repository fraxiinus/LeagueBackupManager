using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace JVCS
{
    public static class Utils
    {
        [DllImport("Kernel32", CharSet = CharSet.Unicode)]
        public static extern bool CreateHardLink(string linkName, string sourceName, IntPtr attribute);

        private static readonly List<Tuple<String, DirectoryInfo[], FileInfo[]>> DirInfo
            = new List<Tuple<String, DirectoryInfo[], FileInfo[]>>();

        /// <summary>
        /// Get the MD5 code of a file.
        /// </summary>
        /// <param name="fileName">File's full path</param>
        /// <param name="ignoreHeader"> A parameter which allow you to ignore a 4-byte header of wad.client files when compute md5</param>
        /// <returns>MD5 value of the input file</returns>
        public static string GetMD5HashFromFile(string fileName, bool ignoreHeader)
        {
            try
            {
                using FileStream file = new FileStream(fileName, System.IO.FileMode.Open);
                if (ignoreHeader)
                {
                    file.Seek(4, SeekOrigin.Begin);
                }

                MD5 md5 = MD5.Create();
                byte[] retVal = md5.ComputeHash(file);
                file.Close();
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }

                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("GetMD5HashFromFile() fail,error:" + ex.Message);
            }
        }

        /// <summary>
        /// Traverse the specific folder in a python-style way.
        /// </summary>
        /// <param name="rootPath">Folder you want to traverse</param>
        /// <returns>Traverse result</returns>
        public static List<Tuple<String, DirectoryInfo[], FileInfo[]>> Walk(String rootPath)
        {
            DirInfo.Clear();
            return Walk_internal(rootPath);
        }

        /// <summary>
        /// Get path's relative path to a  ancestor sub path. 
        /// </summary>
        public static string GetRelativePath(string relativeTo, string fullPath)
        {
            int relativePathLen = relativeTo.Length;
            int fullPathLen = fullPath.Length;
            string substring = fullPath.Substring(relativePathLen + 1, fullPathLen - 1 - relativePathLen);
            return substring;
        }

        private static List<Tuple<String, DirectoryInfo[], FileInfo[]>> Walk_internal(String rootPath)
        {
            var root = new DirectoryInfo(rootPath);
            var directories = root.GetDirectories();
            var files = root.GetFiles();
            var currentDirInfo
                = Tuple.Create<String, DirectoryInfo[], FileInfo[]>(root.FullName, directories, files);
            DirInfo.Add(currentDirInfo);

            // 递归调用子文件夹
            foreach (var dir in directories)
            {
                Walk_internal(dir.FullName);
            }

            return DirInfo;
        }

        public static void Compare()
        {
            string folder1 = @"D:\LOLBackup\11.23\DATA\FINAL\Champions";
            string folder2 = @"D:\LOLBackup\11.24\DATA\FINAL\Champions";
            List<Tuple<string, DirectoryInfo[], FileInfo[]>> tuples = Walk(folder1);
            foreach (var tuple in tuples)
            {
                foreach (var f in tuple.Item3)
                {
                    string fName = f.Name;
                    FileStream fs = new FileStream(f.FullName, FileMode.Open);
                    FileStream fs2 = new FileStream(Path.Combine(folder2, fName), FileMode.Open);
                    byte[] bytsize = new byte[1];
                    byte[] bytsize2 = new byte[1];

                    int start = 0;
                    bool b1 = false, b2 = false;
                    int dirrcount = 0;
                    while (true)
                    {
                        start++;
                        int r = 0, r2 = 0;
                        if (!b1)
                        {
                            r = fs.Read(bytsize, 0, bytsize.Length);
                        }

                        if (!b2)
                        {
                            r2 = fs2.Read(bytsize2, 0, bytsize2.Length);
                        }

                        //如果读取到的字节数为0，说明已到达文件结尾，则退出while循
                        if (r == 0)
                        {
                            // Console.WriteLine("1 finish");
                            b1 = true;
                        }

                        if (r2 == 0)
                        {
                            // Console.WriteLine("2 finish");
                            b2 = true;
                        }

                        if (b1 ^ b2)
                        {
                            Console.WriteLine(f.FullName + "  不同长度");
                            break;
                        }

                        if (b1 && b2)
                        {
                            break;
                        }

                        if (!b1 & !b2)
                        {
                            if (bytsize[0] != bytsize2[0])
                            {
                                dirrcount++;
                            }
                        }
                    }

                    Console.WriteLine($"{dirrcount}");
                }
            }
        }
    }
}