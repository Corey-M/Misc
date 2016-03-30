using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ValidateSPHashes
{
    class Program
    {
        static int _verbosity = 1;

        static string _logFilename = null;
        static void Log(string text)
        {
            if (string.IsNullOrEmpty(_logFilename))
                return;
            try
            {
                File.AppendAllLines(_logFilename, new string[] { text });
            }
            catch { }
        }
        static void Log(string format, params object[] parms)
        {
            try { Log(string.Format(format, parms)); }
            catch { }
        }

        public static HashDataContext DB = null;
        public static Dictionary<string, Hash> hashdata = null;

        static void Main(string[] args)
        {
            try
            {
                Console.CancelKeyPress += Console_CancelKeyPress;
            }
            catch { }

            string dbFile = @"SPHash.sqlite";
            string sourceDir = null;
            bool recurse = false;
            long maxSize = -1;

#if DEBUG
            if (args.Length == 0)
            {
                args = new[] { @"\\HHCRBNNAS01\SPBackup\H-Net", "/s", "/m", "1073741824", "/v" };
            }
#endif

            if (args.Length > 0 && !ParseArgs(args, ref sourceDir, ref recurse, ref maxSize, ref _logFilename, ref dbFile, ref _verbosity))
                return;

            //if (_verbosity > 1)
            {
                Console.WriteLine("Settings:");
                Console.WriteLine("\tScan Path:\t{0}", sourceDir);
                Console.WriteLine("\tMax Len:\t{0}", maxSize == -1 ? "Unlimited" : ReadableFileSize(maxSize));
                Console.WriteLine("\tLogfile:\t{0}", _logFilename);
                Console.WriteLine("\tDBFilename:\t{0}", dbFile);
                Console.WriteLine("\tOptions:\t{0}", string.Join(", ",
                        new string[]
                        {
                            recurse ? "Recurse" : null, 
                            _verbosity == 0 ? "Quiet" : _verbosity == 2 ? "Verbose" : null
                        }.Where(o => !string.IsNullOrEmpty(o))
                    ));
            }

            LoadDatabase(dbFile, sourceDir);

            FileInfo[] files = GatherHashFiles(sourceDir, recurse);
            if (files == null)
                return;

            HashFile[] hashFiles = ReadHashFiles(files, maxSize);

            if (hashFiles != null)
                ProcessHashFiles(hashFiles);
        }

        public static bool exit_loops = false;
        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            e.Cancel = true;
            exit_loops = true;
        }

        private static void ProcessHashFiles(HashFile[] hashFiles)
        {
            DateTime startTime = DateTime.Now;
            Log("Started: {0:yyyy-MM-dd HH:mm:ss}", startTime);
            foreach (var hashfile in hashFiles)
            {
                if (exit_loops)
                {
                    Console.WriteLine("User cancelled scan");
                    break;
                }
                ProcessHashFile(hashfile);
            }
            Log("Finished: {0:yyyy-MM-dd HH:mm:ss}", DateTime.Now);
            TimeSpan elasped = DateTime.Now - startTime;
            Log("Elapsed: {0}", elasped);

            long totalSize = hashFiles.Sum(f => f.SourceFile.Length);
            double rate = (totalSize / (1024 * 1024.0)) / Math.Max(elasped.TotalMinutes, 1.0/ 60);
            Log("Speed: {0:#,0.00} MB/min", rate);

            Log("");
        }

        private static void ProcessHashFile(HashFile hashfile)
        {
            Console.WriteLine("{0}", hashfile.SourceName);
            Console.WriteLine("\tSize: {0}", ReadableFileSize(hashfile.SourceFile.Length));
            Console.Write("\tHash: ");

            string hash = CalculateHash(hashfile.SourceFile);
            if (exit_loops)
                return;

            Console.WriteLine(hash);

            string status = hash == hashfile.Hash ? "OK" : "Invalid Hash";
            Console.WriteLine("\tStatus: {0}", status);

            // update/add database
            var hashent = GetDBHashEntry(hashfile.SourceName);
            if (hashent == null)
            {
                hashent = new Hash { Path = hashfile.SourceName };
                DB.Hashes.InsertOnSubmit(hashent);
            }
            hashent.LastWriteTime = hashfile.SourceFile.LastWriteTime;
            hashent.FileLength = hashfile.SourceFile.Length;
            hashent.SPHash = hashfile.Hash;
            hashent.FileHash = hash;
            hashent.Updated = DateTime.Now;
            DB.SubmitChanges();

            Console.WriteLine();
            Log(string.Join("\t", new string[] { hashfile.SourceName, hashfile.SourceFile.Length.ToString(), hash, status }));
        }

        private static HashFile[] ReadHashFiles(FileInfo[] files, long maxSize)
        {
            Console.Write("Reading Hashfiles");
            var hashFiles = files
                .Select(f => HashFile.Create(f))
                .Where(h => h != null && h.SourceFile != null && h.SourceFile.Exists && (maxSize == -1 || h.SourceFile.Length <= maxSize))
                .ToArray();

            var filtered =
            (
                from file in hashFiles
                join hash in hashdata.Values
                    on file.SourceName.ToLower()
                    equals hash.Path.ToLower()
                    into hashlist
                from h in hashlist.DefaultIfEmpty()
                where 
                    h == null || 
                    // h.Updated.AddMinutes(20) < DateTime.Now || 
                    (file.SourceFile.Length != h.FileLength || file.SourceFile.LastWriteTime != h.LastWriteTime)
                select file
            ).ToArray();

            Console.WriteLine(" - Loaded {0} of {2} Hashfile{1}", filtered.Length, hashFiles.Length == 1 ? "" : "s", hashFiles.Length);

            return filtered.Length < 1 ? null : filtered;
        }

        private static MD5 _md5 = null;

        private static string CalculateHash(FileInfo sourceFile)
        {
            if (_md5 == null)
                _md5 = MD5.Create();
            using (var src = new ProgressStream(sourceFile.OpenRead()))
            {
                var hashBytes = _md5.ComputeHash(src);
                if (exit_loops)
                    return string.Empty;
                var res = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                return res;
            }
        }

        static string[] sizeTags = new[] { "", "KB", "MB", "GB", "TB", "EB" };
        private static string ReadableFileSize(long length)
        {
            double scaled = length;
            int shift = 0;

            while (scaled > 4096)
            {
                scaled /= 1024;
                shift++;
            }
            string res = String.Format(shift == 0 ? "{0:#,0}" : "{0:#,0.00}", scaled);
            if (shift >= sizeTags.Length)
                res += string.Format("x2^{0}", shift * 10);
            else
                res += sizeTags[shift];
            return res;
        }

        private static FileInfo[] GatherHashFiles(string sourceDir, bool recurse)
        {
            try
            {
                Console.Write("Locating Hashfiles");
                FileInfo[] result = new DirectoryInfo(sourceDir)
                    .EnumerateFiles("*.md5", recurse ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly)
                    .OrderBy(f => f.FullName)
                    .ToArray();

                if (result.Length < 1)
                {
                    Console.WriteLine(" - No files found to check.");
                    return null;
                }
                Console.WriteLine();
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.Message);
                return null;
            }
        }

        private static void LoadDatabase(string filename, string pathFilter = null)
        {
            DB = new HashDataContext(filename);
            var src = DB.Hashes.AsQueryable();
            if (pathFilter != null)
                src = src.Where(h => h.Path.StartsWith(pathFilter));
            hashdata = src.ToDictionary(h => h.Path.ToLower());
        }

        private static Hash GetDBHashEntry(string path)
        {
            var res = hashdata.FirstOrDefault(h => h.Key == path.ToLower()); //?.OrderByDescending(h => h.LastWriteTime)?.FirstOrDefault();
            return res.Value;
        }

        private static bool ParseArgs(string[] args, ref string sourceDir, ref bool recurse, ref long maxSize, ref string logName, ref string dbFile, ref int verbosity)
        {
            string pname = System.AppDomain.CurrentDomain.FriendlyName;
            int i = 0;
            while (i < args.Length)
            {
                string arg = args[i];
                string nextarg = (args.Length > i + 1) ? args[i + 1] : null;

                if (arg.StartsWith("-") || arg.StartsWith("/"))
                {
                    string a = arg.Substring(1).ToLower();
                    bool neg = arg.EndsWith("-");
                    if (neg)
                        a = a.Substring(0, a.Length - 1);

                    if (a == "h" || a == "?")
                    {
                        Console.WriteLine("Usage: {0} <Scan Path> [/s] [/m <maxLength>] [/l <logFile>] [/d <dbfile>] [/v[-]]", pname);
                        Console.WriteLine("\t/s\tScan subdirectories");
                        Console.WriteLine("\t/m\tMaximum length of source files to hash");
                        Console.WriteLine("\t/l\tFile to log results to");
                        Console.WriteLine("\t/d\tFilename of database file to use");
                        Console.WriteLine("\t/v[-]\tMore or less output");
                        return false;
                    }
                    else if (a.ToLower() == "s")
                    {
                        recurse = !neg;
                    }
                    else if (a == "m")
                    {
                        if (!long.TryParse(nextarg, out maxSize))
                        {
                            Console.WriteLine("Failed to parse '{0}' as file length", nextarg);
                            return false;
                        }
                        i++;
                    }
                    else if (a == "l")
                    {
                        if (string.IsNullOrWhiteSpace(nextarg))
                        {
                            Console.WriteLine("Missing log filename for '-L' argument");
                            return false;
                        }
                        logName = nextarg;
                        i++;
                    }
                    else if (a == "d")
                    {
                        dbFile = nextarg;
                        i++;
                    }
                    else if (a == "v")
                    {
                        verbosity += neg ? -1 : 1;
                    }
                }
                else if (string.IsNullOrEmpty(sourceDir))
                    sourceDir = arg;
                else
                {
                    Console.WriteLine("Unexpected argument: {0}", arg);
                    return false;
                }

                i++;
            }

#if DEBUG
            if (string.IsNullOrWhiteSpace(sourceDir))
            {
                //Console.WriteLine("Source directory not specified");
                // return;
                sourceDir = @"\\HHCRBNNAS01\SPBackup\HHCSQL07";
                maxSize = 1000 * 1024 * 1024; // * 1024;
                logName = @"C:\Temp\SPHashes.log";
            }
#endif

            return true;
        }
    }
}
