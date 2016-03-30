using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ADPhotoPrep
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

			AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            Properties.Settings.AutoUpgrade();

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new Form1());
		}

		private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			MessageBox.Show("Exception: \n\n" + e.ExceptionObject.ToString());
		}

		private static System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
		{
			var asm = args.RequestingAssembly ?? Assembly.GetExecutingAssembly();
			string asmname = asm.GetName().Name.Split('.').FirstOrDefault();
			var tgt = new AssemblyName(args.Name);
			var tgtname = asmname + "." + tgt.Name + ".dll";
			var resname = asm.GetManifestResourceNames().FirstOrDefault(n => string.Compare(n, tgtname, true) == 0);
			if (!string.IsNullOrWhiteSpace(resname))
			{
				using (var strm = asm.GetManifestResourceStream(resname))
				{
					byte[] data = new byte[strm.Length];
					strm.Read(data, 0, (int)strm.Length);
					return Assembly.Load(data);
				}
			}

			resname = asm.GetManifestResourceNames().FirstOrDefault(n => string.Compare(n, tgtname + ".gz", true) == 0);
			if (!string.IsNullOrWhiteSpace(resname))
			{
				using (var strm = asm.GetManifestResourceStream(resname))
				using (var decom = new GZipStream(strm, CompressionMode.Decompress, true))
				{
					byte[] data = null;
					using (var ms = new MemoryStream())
					{
						decom.CopyTo(ms);
						data = ms.ToArray();
					}
					return Assembly.Load(data);
				}
			}

			return null;
		}
	}
}
