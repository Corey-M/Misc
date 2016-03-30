using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace ADPhotoPrep
{
	partial class AboutBox1 : Form
	{
		#region Description Text
		const string desctext = @"

Uses the following:

* BitMiracle's .NET LibJpeg implementation, v1.4.255.0
      http://bitmiracle.com/libjpeg/
      BSD License

* Code snippets from Stack Overflow as noted in source

Licensed under Creative Common Attribution Share-Alike license (CC BY-SA)
";
		#endregion


		public AboutBox1()
		{
            string cb = "";
            IDataObject cbdat = null;
            if (Clipboard.ContainsImage())
                cb = "[CB: Image]\r\n\r\n";
            else if (Clipboard.ContainsFileDropList())
            {
                var files = Clipboard.GetFileDropList();
                cb = string.Format("[CB: Files({0})]\r\n\r\n", files.Count);
            }
            else if (Clipboard.ContainsText())
            {
                var txt = Clipboard.GetText();
                cb = string.Format("[CB: Text {{{0}}}]\r\n\r\n", txt);
            }
            else if ((cbdat = Clipboard.GetDataObject()) != null)
            {
                cb = string.Format("[CB: Other {{\r\n\t{0}}}]\r\n", string.Join("\r\n\t", cbdat.GetFormats()));
            }



			InitializeComponent();
			this.Text = String.Format("About {0}", AssemblyTitle);
			this.labelProductName.Text = AssemblyProduct;
			this.labelVersion.Text = String.Format("Version {0}", AssemblyVersion);
			this.labelCopyright.Text = AssemblyCopyright;
			this.labelCompanyName.Text = AssemblyCompany;
			this.textBoxDescription.Text = cb + AssemblyDescription + desctext;

			byte[] icoData;
			using (var icoStrm = Assembly.GetExecutingAssembly().GetManifestResourceStream("ADPhotoPrep.Iconsmind-Outline-Portrait.ico"))
			using (var mstrm = new MemoryStream())
			{
				icoStrm.CopyTo(mstrm);
				icoData = mstrm.ToArray();
			}

			var bmp = ExtractImage(icoData);
			logoPictureBox.Image = bmp;
		}

		/// <summary>Extract the best image we can find from the supplied icon file</summary>
		/// <param name="srcBuf">Bytes to scan for images</param>
		/// <returns>Best found image</returns>
		static Bitmap ExtractImage(byte[] srcBuf)
		{
			const int sizeICONDIR = 6;
			const int sizeICONDIRENTRY = 16;

			Bitmap bmpIcon = null;
			int maxSize = 0;
			try
			{
				int iCount = BitConverter.ToInt16(srcBuf, 4);
				for (int index = 0; index < iCount; index++)
				{
					// get image properties
					int width = srcBuf[sizeICONDIR + sizeICONDIRENTRY * index];
					int height = srcBuf[sizeICONDIR + sizeICONDIRENTRY * index + 1];
					int nBits = BitConverter.ToInt16(srcBuf, sizeICONDIR + sizeICONDIRENTRY * index + 6);

					// if larger size than the one we already have (if we have one):
					if (width > maxSize || (width == 0 && height == 0 && nBits == 32))
					{
						int imgSize = BitConverter.ToInt32(srcBuf, sizeICONDIR + sizeICONDIRENTRY * index + 8);
						int imgOffset = BitConverter.ToInt32(srcBuf, sizeICONDIR + sizeICONDIRENTRY * index + 12);
						try
						{
							using (MemoryStream destStream = new MemoryStream())
							using (BinaryWriter writer = new BinaryWriter(destStream))
							{
								// extract image into stream
								writer.Write(srcBuf, imgOffset, imgSize);

								// load image stream into bitmap 
								destStream.Seek(0, System.IO.SeekOrigin.Begin);
								bmpIcon = new Bitmap(Image.FromStream(destStream));
							}
							maxSize = width == 0 ? 65535 : width;

							// early exit if we found the best image already
							if (maxSize >= 65535)
								break;
						}
						catch { }
					}
				}
			}
			catch { return null; }
			return bmpIcon;
		}

		#region Assembly Attribute Accessors

		public string AssemblyTitle
		{
			get
			{
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
				if (attributes.Length > 0)
				{
					AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
					if (titleAttribute.Title != "")
					{
						return titleAttribute.Title;
					}
				}
				return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
			}
		}

		public string AssemblyVersion
		{
			get
			{
				return Assembly.GetExecutingAssembly().GetName().Version.ToString();
			}
		}

		public string AssemblyDescription
		{
			get
			{
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
				if (attributes.Length == 0)
				{
					return "";
				}
				return ((AssemblyDescriptionAttribute)attributes[0]).Description;
			}
		}

		public string AssemblyProduct
		{
			get
			{
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
				if (attributes.Length == 0)
				{
					return "";
				}
				return ((AssemblyProductAttribute)attributes[0]).Product;
			}
		}

		public string AssemblyCopyright
		{
			get
			{
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
				if (attributes.Length == 0)
				{
					return "";
				}
				return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
			}
		}

		public string AssemblyCompany
		{
			get
			{
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
				if (attributes.Length == 0)
				{
					return "";
				}
				return ((AssemblyCompanyAttribute)attributes[0]).Company;
			}
		}
		#endregion

		public static void ShowAbout()
		{
			using (var frm = new AboutBox1())
			{
				frm.ShowDialog();
			}
		}

		private void SourceCodeBtn_Click(object sender, EventArgs e)
		{
			// locate source in assembly's resources
			var asm = Assembly.GetExecutingAssembly();
			var resname = asm.GetManifestResourceNames().Where(n => n.EndsWith("Source.7z", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
			if (string.IsNullOrEmpty(resname))
			{
				MessageBox.Show("Unable to locate source code resource.\n\nContact author.", "Source Missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}

			saveFileDialog1.FileName = resname;
			if (saveFileDialog1.ShowDialog() != DialogResult.OK)
				return;

			using (var instrm = asm.GetManifestResourceStream(resname))
			using (var outstrm = File.Create(saveFileDialog1.FileName))
			{
				instrm.CopyTo(outstrm);
			}
			try
			{
				System.Diagnostics.Process.Start("explorer", "/select,\"" + saveFileDialog1.FileName + "\"");
			}
			catch { }
		}
	}
}
