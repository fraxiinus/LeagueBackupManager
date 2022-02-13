using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace JVCS
{
    public class Patch
    {
        public Dictionary<string, string> FileDict { get; set; }
        public int Header { get; set; }
    }

    public class VCS
    {
        #region Filed

        private readonly string _repositoryPath;
        private const string DB_NAME = "vcs.json";
        private List<Regex> _filters = new();

        #endregion

        #region Public Method

        public VCS(string repoPath)
        {
            this._repositoryPath = repoPath;
        }

        /// <summary>
        /// Store a patch into the vcs repository.
        /// </summary>
        /// <param name="lolPath">The game folder path.</param>
        /// <param name="regexFilter">Regex strings which are used to filter files which you don't want to store.</param>
        /// <exception cref="VCSException">Exception</exception>
        public void ImportPatch(string lolPath, List<string>? regexFilter)
        {
            IProgress<int> progress;
            _filters.Clear();
            CheckAndCreateRepo(_repositoryPath);
            /*
             * 0.Get version information to see if it already exists
             *1.Calculate Md5
             *2.Go to the library to find out if it exists
             *3.Record to the patch file
             *4.Skip if it exists
             */
            var fileVersionInfo = FileVersionInfo.GetVersionInfo(Path.Combine(lolPath, "League of Legends.exe"));
            var patchVersion = fileVersionInfo.ProductVersion;
            var patchFile = Path.Combine(_repositoryPath, "patch", patchVersion + ".json");
            if (File.Exists(patchFile))
            {
                //already exists
                throw new VCSException($"this patch already exists, ver:{patchVersion}");
            }


            //regex filters
            if (regexFilter != null)
                foreach (var filter in regexFilter)
                {
                    Regex r = new Regex(filter);
                    this._filters.Add(r);
                }

            //travers the files
            Dictionary<string, string> filesDict = new Dictionary<string, string>(); // <relative path to the root folder,md5>

            List<Tuple<string, DirectoryInfo[], FileInfo[]>> results = Utils.Walk(lolPath);
            int fileHeader = 0;
            foreach (var tuple in results)
            {
                Console.WriteLine("Current Directory:" + tuple.Item1);
                // foreach (var d in tuple.Item2)
                // {
                //     Console.WriteLine(d.Name);
                // }

                //check by filter
                foreach (var f in tuple.Item3)
                {
                    bool isContinue = false;
                    foreach (var filter in _filters)
                    {
                        var relativePath = Utils.GetRelativePath(lolPath, f.FullName);
                        if (filter.IsMatch(relativePath))
                        {
                            isContinue = true;
                            break;
                        }
                    }

                    if (isContinue)
                    {
                        continue;
                    }

                    if (fileHeader == 0 && f.Name.EndsWith(".wad.client"))
                    {
                        byte[] headBytes = new byte[4];
                        using FileStream fs = new FileStream(f.FullName, FileMode.Open);
                        fs.Read(headBytes, 0, 4);
                        int int32 = BitConverter.ToInt32(headBytes, 0);
                        fileHeader = int32;
                    }

                    //hash
                    string md5HashFromFile = Utils.GetMD5HashFromFile(f.FullName, f.Name.EndsWith(".wad.client"));
                    string md5File = Path.Combine(Path.Combine(_repositoryPath, "files", md5HashFromFile));
                    bool exists = File.Exists(md5File);
                    if (!exists)
                    {
                        File.Copy(f.FullName, md5File);
                    }

                    filesDict.Add(Utils.GetRelativePath(lolPath, f.FullName), md5HashFromFile);
                    Console.WriteLine($"{Utils.GetRelativePath(lolPath, f.FullName)} {md5HashFromFile}");
                }
            }

            Patch p = new Patch();
            p.Header = fileHeader;
            p.FileDict = filesDict;
            //写入patch文件
            string serializeObject = JsonConvert.SerializeObject(p);
            File.WriteAllText(patchFile, serializeObject);
            Console.WriteLine(serializeObject);
        }

        /// <summary>
        /// Export a client patch to the specific folder, the folder will be delete first.
        /// </summary>
        /// <param name="patchName">Patch version.</param>
        /// <param name="dstPath">Folder you want to export to.</param>
        /// <param name="useHardLink">Using hard links can greatly accelerate the process.</param>
        /// <exception cref="VCSException"></exception>
        public void ExportPatch(string patchName, string dstPath, bool useHardLink)
        {
            string readAllText = File.ReadAllText(GetPatchFilePath(patchName));
            Patch patch = JsonConvert.DeserializeObject<Patch>(readAllText) ??
                          throw new VCSException("the patch file has wrong json format");
            DeleteFolder(dstPath);
            foreach (var kv in patch.FileDict)
            {
                //check and create folder
                string dstFilePath = Path.Combine(dstPath, kv.Key);
                string directoryName = Path.GetDirectoryName(dstFilePath);
                if (!Directory.Exists(directoryName))
                {
                    Directory.CreateDirectory(directoryName);
                }

                if (File.Exists(dstFilePath))
                {
                    File.Delete(dstFilePath);
                }

                if (useHardLink)
                {
                    Utils.CreateHardLink(dstFilePath, GetStoredFilePath(kv.Value), IntPtr.Zero);
                }
                else
                {
                    File.Copy(GetStoredFilePath(kv.Value), dstFilePath);
                }

                if (dstPath.EndsWith(".wad.client"))
                {
                    using FileStream fs = new FileStream(dstFilePath, System.IO.FileMode.Open);
                    using BinaryWriter bw = new BinaryWriter(fs);
                    bw.Write(patch.Header);
                }
            }
        }

        /// <summary>
        /// Fetch all stored patch's name.
        /// </summary>
        /// <returns>List that contains all patch's name.</returns>
        public List<string> GetPatchList()
        {
            List<string> res = new List<string>();
            List<Tuple<string, DirectoryInfo[], FileInfo[]>> tuples = Utils.Walk(GetPatchFolder());
            foreach (Tuple<string, DirectoryInfo[], FileInfo[]> tuple in tuples)
            {
                foreach (var f in tuple.Item3)
                {
                    res.Add(Path.GetFileNameWithoutExtension(f.FullName));
                }
            }

            return res;
        }

        #endregion


        #region Private Method

        private void CheckAndCreateRepo(string repoPath)
        {
            string cfgPath = Path.Combine(repoPath, DB_NAME);
            bool exists = File.Exists(cfgPath);
            if (exists)
            {
                return;
            }

            File.WriteAllText(cfgPath, JsonConvert.SerializeObject(new { }, Formatting.Indented));
            Directory.CreateDirectory(Path.Combine(repoPath, "files"));
            Directory.CreateDirectory(Path.Combine(repoPath, "patch"));
        }

        private string GetPatchFilePath(string version)
        {
            return Path.Combine(_repositoryPath, "patch", version + ".json");
        }

        private string GetPatchFolder()
        {
            return Path.Combine(_repositoryPath, "patch");
        }

        private string GetStoredFilePath(string md5Str)
        {
            return Path.Combine(_repositoryPath, "files", md5Str);
        }

        private void DeleteFolder(string folder)
        {
            if (Directory.Exists(folder))
            {
                Directory.Delete(folder, true);
            }
        }

        #endregion
    }
}