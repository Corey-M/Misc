using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ValidateSPHashes
{
    public class HashFile
    {
        public static HashFile Create(FileInfo file)
        {
            if (!file.Exists)
                return null;
            return new HashFile { _file = file };
        }

        public static HashFile Create(string filename)
        {
            return Create(new FileInfo(filename));
        }

        private HashFile() { }

        protected FileInfo _file;
        public string FullName => _file?.FullName;
        public string Name => _file?.Name;

        private string _hash = null;
        private string _srcspec = null;
        private string _updateTag = null;
        private DateTime? _updateDate = null;

        public string Hash
        {
            get
            {
                if (_hash == null)
                    ReadContent();
                return _hash;
            }
        }

        public string SourceSpec
        {
            get
            {
                if (_srcspec == null)
                    ReadContent();
                return _srcspec;
            }
        }

        public string SourceName
        {
            get
            {
                if (_srcspec == null)
                    ReadContent();
                return Path.Combine(_file.DirectoryName, _srcspec);
            }
        }

        private FileInfo _sourceFile;
        public FileInfo SourceFile
        {
            get
            {
                if (_sourceFile == null)
                    _sourceFile = new FileInfo(SourceName);
                return _sourceFile;
            }
        } 

        public string UpdateTag
        {
            get
            {
                if (_updateTag == null)
                    ReadContent();
                return _updateTag;
            }
        }

        public DateTime? UpdateDate
        {
            get
            {
                if (_updateDate == null)
                    ReadContent();
                return _updateDate == DateTime.MinValue ? null : _updateDate;
            }
        }

        static Regex l1re = new Regex(@"^(?<hash>[a-f0-9]{32})\s*\*(?<src>.+)$", RegexOptions.IgnoreCase);
        static Regex l2re = new Regex(@"^;(?<tag>[a-f0-9]{16});(?<udate>[0-9/]+)$", RegexOptions.IgnoreCase);

        protected void ReadContent()
        {
            string[] lines = File.ReadAllLines(_file.FullName).Where(l => (l??"").Length > 0).ToArray();
            bool OK = 
                (lines.Length > 0 && l1re.IsMatch(lines[0])) &&
                (lines.Length < 2 || l2re.IsMatch(lines[1]));

            if (OK)
            {
                Match m = l1re.Match(lines[0]);
                _hash = m.Groups["hash"].Value;
                _srcspec = m.Groups["src"].Value;

                if (lines.Length > 1)
                {
                    m = l2re.Match(lines[1]);
                    _updateTag = m.Groups["tag"].Value;
                    DateTime dt;
                    if (DateTime.TryParse(m.Groups["udate"].Value, out dt))
                        _updateDate = dt;
                    else
                        _updateDate = DateTime.MinValue;
                }
            }
            else
            {
                _hash = string.Empty;
                _srcspec = string.Empty;
                _updateTag = string.Empty;
                _updateDate = DateTime.MinValue;
            }
        }
    }
}
