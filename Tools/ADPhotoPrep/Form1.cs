using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ADPhotoPrep
{
    public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
			// adapted from StackOverflow answer at: http://stackoverflow.com/questions/189031/set-same-icon-for-all-my-forms
			//Icon = System.Drawing.Icon.ExtractAssociatedIcon(Application.ExecutablePath);
			Icon = OS.ExtractAssociatedIcon(Application.ExecutablePath);

			label1.Location = tabControl1.Bounds.Middle().Subtract(new Point(label1.Width / 2, label1.Height / 2));
			//(tabControl1 as Control).Paint += TabControl1_Paint;
		}

		#region Properties
		/// <summary>
		/// ImageEditor control from selected tab, or null if no tab selected
		/// </summary>
		private ImageEditor CurrentEditor
		{
			get
			{
				if (tabControl1.SelectedTab == null)
					return null;
				return tabControl1.SelectedTab.Controls.OfType<ImageEditor>().FirstOrDefault();
			}
		}
		#endregion

		#region Operations
		/// <summary>Create new ImageEditor for specified filename, or select existing tab if image already loaded</summary>
		/// <param name="filename">Image file to load</param>
		/// <returns>Newly create or selected tab</returns>
		private TabPage LoadImage(string filename)
		{
			// try loading image
			try
			{
				Image.FromFile(filename);
			}
			catch
			{
				MessageBox.Show(string.Format("Failed to open image file:\n\n{0}", filename), "Load Failed");
				return null;
			}

			if (tabControl1.TabPages.ContainsKey(filename))
				return tabControl1.TabPages[filename] ;

			tabControl1.TabPages.Add(filename, Path.GetFileNameWithoutExtension(filename));
			var tab = tabControl1.TabPages[filename];

			var editor = new ImageEditor();
			tab.Controls.Add(editor);
			editor.Dock = DockStyle.Fill;
			editor.Close += Editor_Close;
			editor.LoadImage(filename);

			return tab;
		}
		#endregion

		#region Editor events
		private void Editor_Close(object sender, CancelEventArgs e)
		{
			// get index of tab control
			var editor = sender as ImageEditor;
			if (editor == null)
				return;

			var tp = editor.Parent as TabPage;
			if (tp == null)
				return;

			int idx = tabControl1.TabPages.IndexOf(tp);
			if (idx >= 0)
				tabControl1.TabPages.RemoveAt(idx);
		}
		#endregion

		#region Tab Control events
		private void tabControl1_DragEnter(object sender, DragEventArgs e)
		{
			e.Effect = DragDropEffects.Move;
		}

		private void tabControl1_DragDrop(object sender, DragEventArgs e)
		{
			string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
			if (files == null || files.Length < 1)
				return;

			int lastindex = -1;

			foreach (var file in files.OrderBy(f => Path.GetFileNameWithoutExtension(f)))
			{
				var tab = LoadImage(file);
				if (tab != null)
					lastindex = tabControl1.TabPages.IndexOf(tab);
			}
			if (lastindex >= 0)
				tabControl1.SelectedIndex = lastindex;
		}
		#endregion

		#region Tab Context Menu
		private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
		{
			if (tabControl1.TabCount < 1)
				e.Cancel = true;
		}

		private void tabctxSaveAll_Click(object sender, EventArgs e)
		{
			foreach (TabPage tab in tabControl1.TabPages)
			{
				var editor = tab.Controls.OfType<ImageEditor>().FirstOrDefault();
				if (editor != null)
					editor.SaveImage();
			}
		}
		#endregion

		#region File Menu
		private void menuFile_DropDownOpening(object sender, EventArgs e)
		{
			bool en = tabControl1.TabCount > 0;
			miFileSave.Enabled = en;
			miFileSaveAs.Enabled = en;
			miFileClose.Enabled = en;
			miFileSaveAll.Enabled = en;
			miFileCloseAll.Enabled = en;
		}

		private void miFileOpen_Click(object sender, EventArgs e)
		{
			dlgOpen.FileName = null;
			if (dlgOpen.ShowDialog() != DialogResult.OK)
				return;

			var tab = LoadImage(dlgOpen.FileName);
			if (tab != null)
				tabControl1.SelectedIndex = tabControl1.TabPages.IndexOf(tab);
		}

		private void miFileSave_Click(object sender, EventArgs e)
		{
			var ed = CurrentEditor;
			if (ed != null)
				ed.SaveImage();
		}

		private void miFileSaveAs_Click(object sender, EventArgs e)
		{
			var ed = CurrentEditor;
			if (ed != null)
			{
				dlgSaveAs.FileName = ed.Filename;
				if (dlgSaveAs.ShowDialog() != DialogResult.OK)
					return;
				ed.SaveImage(dlgSaveAs.FileName);
			}
		}

		private void miFileClose_Click(object sender, EventArgs e)
		{
			var ed = CurrentEditor;
			if (ed == null)
				return;

			Editor_Close(ed, new CancelEventArgs());
		}

		private void miFileSaveAll_Click(object sender, EventArgs e)
		{
			if (folderBrowserDialog1.ShowDialog() != DialogResult.OK)
				return;

			foreach (var editor in tabControl1.TabPages.OfType<TabPage>().Select(p => p.Controls.OfType<ImageEditor>().FirstOrDefault()).Where(ed => ed != null))
			{
				string filename = Path.Combine(folderBrowserDialog1.SelectedPath, Path.GetFileNameWithoutExtension(editor.Filename) + ".jpg");
				editor.SaveImage(filename);
			}
		}

		private void miFileCloseAll_Click(object sender, EventArgs e)
		{
			while (tabControl1.TabCount > 0)
				tabControl1.TabPages.RemoveAt(0);
		}

		private void miFileExit_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}
		#endregion

		#region Edit menu
		private void menuEdit_DropDownOpened(object sender, EventArgs e)
		{
			bool en = tabControl1.TabCount > 0;
			miEditCopy.Enabled = en;
            miEditPaste.Enabled = Clipboard.ContainsImage() || Clipboard.ContainsFileDropList();
		}

		private void miEditCopy_Click(object sender, EventArgs e)
		{
			var ed = CurrentEditor;
			if (ed == null)
				return;

			var bmp = ed.GetSelectedImage();
			Clipboard.SetImage(bmp);
		}

        private void miEditPaste_Click(object sender, EventArgs e)
        {
            var ed = CurrentEditor;

            if (Clipboard.ContainsFileDropList())
            {
                var files = Clipboard.GetFileDropList();
                if (files.Count < 1)
                    return;
                else if (files.Count == 1)
                {
                    string filename = files[0];
                    if (ed == null)
                        LoadImage(filename);
                    else
                        ed.LoadImage(filename);
                }
                else
                {
                    TabPage tab = null;
                    foreach (string filename in files)
                        tab = LoadImage(filename) ?? tab;
                    
                    if (tab != null)
                        tabControl1.SelectedIndex = tabControl1.TabPages.IndexOf(tab);
                }
            }
            else if (Clipboard.ContainsImage())
            {
                var img = Clipboard.GetImage();
                if (ed == null)
                {
                    string key = string.Format("CB-{0:X}", DateTime.Now.ToBinary());
                    tabControl1.TabPages.Add(key, "[Clipboard]");
                    var tab = tabControl1.TabPages[key];

                    ed = new ImageEditor();
                    tab.Controls.Add(ed);
                    ed.Dock = DockStyle.Fill;
                    ed.Close += Editor_Close;
                }
                ed.SetImage(img);
            }
        }

        private void miEditADPrefs_Click(object sender, EventArgs e)
		{
			EditADPrefs.Execute();
		}
		#endregion

		#region Help menu
		private void miHelpAboout_Click(object sender, EventArgs e)
		{
			AboutBox1.ShowAbout();
		}
		#endregion

		private void tabControl1_Resize(object sender, EventArgs e)
		{
			label1.Location = tabControl1.Bounds.Middle().Subtract(new Point(label1.Width / 2, label1.Height / 2));
		}

		private void tabControl1_ControlAdded(object sender, ControlEventArgs e)
		{
			label1.Visible = tabControl1.TabCount == 0;
		}

		private void tabControl1_ControlRemoved(object sender, ControlEventArgs e)
		{
			int c = tabControl1.TabPages.OfType<Control>().Count(p => p != e.Control);
			label1.Visible = c == 0;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            var config = Properties.Settings.Default;
            if (config.Dirty)
                config.Save();
        }
    }
}
