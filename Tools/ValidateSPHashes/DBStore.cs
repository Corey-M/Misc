using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite.Linq;
using System.Data.Common;
using System.Data.Linq.Mapping;
using System.Data.Linq;
using System.Data.SQLite;
using System.IO;

namespace ValidateSPHashes
{
    [Table(Name = "Hashes")]
    public class Hash
    {
        [Column(IsPrimaryKey = true)]
        public string Path { get; set; }
        [Column]
        public DateTime LastWriteTime { get; set; }
        [Column]
        public long FileLength { get; set; }
        [Column]
        public string SPHash { get; set; }
        [Column]
        public string FileHash { get; set; }
        [Column]
        public DateTime Updated { get; set; }
    }

    public class HashDataContext : DataContext
    {
        //private SQLiteConnection _connection = null;
        private DbConnection _connection = null;

        public HashDataContext(string fname = null)
            //: this(new SQLiteConnection(@"Data Source=C:\Temp\SPHash.sqlite").OpenAndReturn())
            : this(DBStore.OpenConnection(fname))
        { }

        public HashDataContext(DbConnection conn)
            : base(conn)
        {
            _connection = conn;
            //this.Log = Console.Out;
        }

        public Table<Hash> Hashes => GetTable<Hash>();
    }

    public static class DBStore
    {
        static string defaultFile = @"C:\Temp\SPHash.sqlite";
        static string ConnectionString(string fname = null)
        {
            var csb = new SQLiteConnectionStringBuilder();
            csb.DataSource = fname ?? defaultFile;
            return csb.ConnectionString;
        }

        public static DbConnection GetConnection(string fname = null)
        {
            var conn = System.Data.SQLite.Linq.SQLiteProviderFactory.Instance.CreateConnection();
            conn.ConnectionString = ConnectionString(fname);
            conn.Open();
            return conn;
        }

        public static DbConnection OpenConnection(string fname = null)
        {
            if (!CheckDBFile(fname ?? defaultFile, true))
                return null;
            return GetConnection(fname);
        }

        public static bool CheckDBFile(string fname, bool create = false)
        {
            if (!File.Exists(fname))
            {
                if (!create)
                    return false;

                try
                {
                    SQLiteConnection.CreateFile(fname);

                    string cmd1str = "CREATE TABLE Hashes(Path TEXT, LastWriteTime DATETIME, FileLength BIGINT, SPHash TEXT, FileHash TEXT, Updated DATETIME, PRIMARY KEY(Path))";
                    string cmd2str = "CREATE VIEW vHashes AS SELECT *, CAST((SPHash = FileHash) AS BIT) OK FROM Hashes";
                    using (var conn = new SQLiteConnection(ConnectionString(fname)).OpenAndReturn())
                    using (var cmd1 = new SQLiteCommand(cmd1str, conn))
                    using (var cmd2 = new SQLiteCommand(cmd2str, conn))
                    {
                        cmd1.ExecuteNonQuery();
                        cmd2.ExecuteNonQuery();
                    }
                }
                catch { return false; }
            }
            return true;
        }
    }
}
