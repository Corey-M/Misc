using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace EnumerateAccessRights
{
	public class EffectiveRights
	{
		public bool Any { get { return !string.IsNullOrWhiteSpace(string.Format("{0}{1}{2}", Full, Read, Write)); } }

		public string Account { get; set; }
		public string Full { get; set; }
		public string Read { get; set; }
		public string Write { get; set; }
		public string Special { get; set; }

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("\"" + Account + "\"=[");
			string[] rights = new string[] 
				{
					string.IsNullOrWhiteSpace(Full) ? "" : (Full+"Full"),
					string.IsNullOrWhiteSpace(Read) ? "" : (Read+"Read"),
					string.IsNullOrWhiteSpace(Write) ? "" : (Write+"Write"),
					string.IsNullOrWhiteSpace(Special) ? "" : (Special+"Special"),
				};
			sb.Append(string.Join(",", rights.Where(r => !string.IsNullOrWhiteSpace(r))));
			sb.Append("]");
			return sb.ToString();
		}

		public string ToCSV()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendFormat("\"{0}\",", Account);
			sb.AppendFormat("{0},", Full);
			sb.AppendFormat("{0},", Read);
			sb.AppendFormat("{0},", Write);
			//sb.AppendFormat("{0}", Special);
			return sb.ToString();
		}
	}

	public class RightsFlags
	{
		public string Account { get; set; }
		public bool AllowRead { get; set; }
		public bool AllowWrite { get; set; }
		public bool AllowFull { get; set; }
		public bool AllowSpecial { get; set; }
		public bool DenyRead { get; set; }
		public bool DenyWrite { get; set; }
		public bool DenyFull { get; set; }
		public bool DenySpecial { get; set; }

		public static implicit operator EffectiveRights(RightsFlags flags)
		{
			return flags.Effective();
		}

		public EffectiveRights Effective()
		{
			return new EffectiveRights
			{
				Account = Account,
				Full = DenyFull ? "Deny" : AllowFull ? "Allow" : "",
				Read = DenyRead ? "Deny" : AllowRead ? "Allow" : "",
				Write = DenyWrite ? "Deny" : AllowWrite ? "Allow" : "",
				Special = DenySpecial ? "Deny" : AllowSpecial ? "Allow" : ""
			};
		}

		public void Add(FileSystemAccessRule rule)
		{
			if (rule.AccessControlType == AccessControlType.Deny)
			{
				if ((rule.FileSystemRights & FileSystemRights.FullControl) == FileSystemRights.FullControl)
					DenyFull = true;
				if ((rule.FileSystemRights & FileSystemRights.Read) == FileSystemRights.Read)
					DenyRead = true;
				if ((rule.FileSystemRights & FileSystemRights.Write) == FileSystemRights.Write)
					DenyWrite = true;
			}
			else
			{
				if ((rule.FileSystemRights & FileSystemRights.FullControl) == FileSystemRights.FullControl)
					AllowFull = true;
				if ((rule.FileSystemRights & FileSystemRights.Read) == FileSystemRights.Read)
					AllowRead = true;
				if ((rule.FileSystemRights & FileSystemRights.Write) == FileSystemRights.Write)
					AllowWrite = true;
			}
		}
	}

	public class autodict<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
		where TValue : new() 
	{
		private Dictionary<TKey, TValue> _dict = new Dictionary<TKey, TValue>();

		public TValue this[TKey key]
		{
			get
			{
				if (!_dict.ContainsKey(key))
					_dict[key] = new TValue();
				return _dict[key];
			}
			set
			{
				_dict[key] = value;
			}
		}

		public bool ContainsKey(TKey key)
		{
			return _dict.ContainsKey(key);
		}

		public Dictionary<TKey, TValue>.KeyCollection Keys { get { return _dict.Keys; } }
		public Dictionary<TKey, TValue>.ValueCollection Values { get { return _dict.Values; } }

		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			return _dict.AsEnumerable().GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
	}

	class Program
	{
		//static Dictionary<string, Dictionary<string, EffectiveRights>> _folderRights = new Dictionary<string, Dictionary<string, EffectiveRights>>();
		static autodict<string, autodict<string, EffectiveRights>> _folderRights = new autodict<string, autodict<string, EffectiveRights>>();

		static void Main(string[] args)
		{
			string basePath = @"C:\Windows";
			
			foreach (var dir in EnumerateDirectories(basePath))
			{
				try
				{
					var acls = dir
						.GetAccessControl(AccessControlSections.Access)
						.GetAccessRules(true, true, typeof(System.Security.Principal.NTAccount))
						.OfType<FileSystemAccessRule>()
						.Where(r => r.IdentityReference.Value != "NT AUTHORITY\\SYSTEM")
						.ToArray();

					Dictionary<string, RightsFlags> rights = new Dictionary<string, RightsFlags>();

					foreach (var acl in acls)
					{
						RightsFlags flags;
						if (rights.ContainsKey(acl.IdentityReference.Value))
							flags = rights[acl.IdentityReference.Value];
						else
						{
							flags = new RightsFlags { Account = acl.IdentityReference.Value };
							rights[acl.IdentityReference.Value] = flags;
						}

						flags.Add(acl);
					}

					foreach (var key in rights.Keys.OrderBy(k => k))
					{
						EffectiveRights eff = rights[key];
						if (!eff.Any)
							continue;
						_folderRights[dir.FullName][key] = eff;

						string parent = dir.Parent.FullName;
						if (!_folderRights.ContainsKey(parent) || !_folderRights[parent].ContainsKey(key) || eff.ToString() != _folderRights[parent][key].ToString())
							Console.WriteLine(@"""{0}"",{1}", dir.FullName, eff.ToCSV());
					}
				}
				catch { }
			}

		}

		public static IEnumerable<DirectoryInfo> EnumerateDirectories(string root)
		{
			return EnumerateDirectories(new DirectoryInfo(root));
		}

		public static IEnumerable<DirectoryInfo> EnumerateDirectories(DirectoryInfo root)
		{
			if (root == null)
				throw new ArgumentNullException("root");
			if (!root.Exists)
				throw new DirectoryNotFoundException(root.FullName);

			Stack<DirectoryInfo> stack = new Stack<DirectoryInfo>();
			stack.Push(root);

			while (stack.Any())
			{
				var curr = stack.Pop();
				
				try
				{
					var subs = curr.GetDirectories();
					foreach (var nxt in subs.OrderByDescending(d => d.Name))
						stack.Push(nxt);
				}
				catch
				{
				}

				yield return curr;
			}
		}
	}
}
