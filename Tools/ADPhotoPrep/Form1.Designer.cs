namespace ADPhotoPrep
{
	partial class Form1
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tabctxSaveAll = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.miFileOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.miFileSave = new System.Windows.Forms.ToolStripMenuItem();
            this.miFileSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.miFileClose = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.miFileSaveAll = new System.Windows.Forms.ToolStripMenuItem();
            this.miFileCloseAll = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.miFileExit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.miEditCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.miEditADPrefs = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.miHelpAboout = new System.Windows.Forms.ToolStripMenuItem();
            this.dlgOpen = new System.Windows.Forms.OpenFileDialog();
            this.dlgSaveAs = new System.Windows.Forms.SaveFileDialog();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.miEditPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.AllowDrop = true;
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.ContextMenuStrip = this.contextMenuStrip1;
            this.tabControl1.Location = new System.Drawing.Point(0, 24);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(691, 456);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.ControlAdded += new System.Windows.Forms.ControlEventHandler(this.tabControl1_ControlAdded);
            this.tabControl1.ControlRemoved += new System.Windows.Forms.ControlEventHandler(this.tabControl1_ControlRemoved);
            this.tabControl1.DragDrop += new System.Windows.Forms.DragEventHandler(this.tabControl1_DragDrop);
            this.tabControl1.DragEnter += new System.Windows.Forms.DragEventHandler(this.tabControl1_DragEnter);
            this.tabControl1.Resize += new System.EventHandler(this.tabControl1_Resize);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tabctxSaveAll});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(116, 26);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // tabctxSaveAll
            // 
            this.tabctxSaveAll.Name = "tabctxSaveAll";
            this.tabctxSaveAll.Size = new System.Drawing.Size(115, 22);
            this.tabctxSaveAll.Text = "Save All";
            this.tabctxSaveAll.Click += new System.EventHandler(this.tabctxSaveAll_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFile,
            this.menuEdit,
            this.menuHelp});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(691, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // menuFile
            // 
            this.menuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miFileOpen,
            this.miFileSave,
            this.miFileSaveAs,
            this.miFileClose,
            this.toolStripSeparator2,
            this.miFileSaveAll,
            this.miFileCloseAll,
            this.toolStripSeparator1,
            this.miFileExit});
            this.menuFile.Name = "menuFile";
            this.menuFile.Size = new System.Drawing.Size(37, 20);
            this.menuFile.Text = "&File";
            this.menuFile.DropDownOpening += new System.EventHandler(this.menuFile_DropDownOpening);
            // 
            // miFileOpen
            // 
            this.miFileOpen.Name = "miFileOpen";
            this.miFileOpen.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.miFileOpen.Size = new System.Drawing.Size(198, 22);
            this.miFileOpen.Text = "&Open";
            this.miFileOpen.Click += new System.EventHandler(this.miFileOpen_Click);
            // 
            // miFileSave
            // 
            this.miFileSave.Name = "miFileSave";
            this.miFileSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.miFileSave.Size = new System.Drawing.Size(198, 22);
            this.miFileSave.Text = "&Save";
            this.miFileSave.Click += new System.EventHandler(this.miFileSave_Click);
            // 
            // miFileSaveAs
            // 
            this.miFileSaveAs.Name = "miFileSaveAs";
            this.miFileSaveAs.Size = new System.Drawing.Size(198, 22);
            this.miFileSaveAs.Text = "Save &As";
            this.miFileSaveAs.Click += new System.EventHandler(this.miFileSaveAs_Click);
            // 
            // miFileClose
            // 
            this.miFileClose.Name = "miFileClose";
            this.miFileClose.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F4)));
            this.miFileClose.Size = new System.Drawing.Size(198, 22);
            this.miFileClose.Text = "&Close";
            this.miFileClose.Click += new System.EventHandler(this.miFileClose_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(195, 6);
            // 
            // miFileSaveAll
            // 
            this.miFileSaveAll.Name = "miFileSaveAll";
            this.miFileSaveAll.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
            this.miFileSaveAll.Size = new System.Drawing.Size(198, 22);
            this.miFileSaveAll.Text = "Save A&ll";
            this.miFileSaveAll.Click += new System.EventHandler(this.miFileSaveAll_Click);
            // 
            // miFileCloseAll
            // 
            this.miFileCloseAll.Name = "miFileCloseAll";
            this.miFileCloseAll.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.F4)));
            this.miFileCloseAll.Size = new System.Drawing.Size(198, 22);
            this.miFileCloseAll.Text = "Close All";
            this.miFileCloseAll.Click += new System.EventHandler(this.miFileCloseAll_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(195, 6);
            // 
            // miFileExit
            // 
            this.miFileExit.Name = "miFileExit";
            this.miFileExit.ShortcutKeyDisplayString = "Alt+F4";
            this.miFileExit.Size = new System.Drawing.Size(198, 22);
            this.miFileExit.Text = "E&xit";
            this.miFileExit.Click += new System.EventHandler(this.miFileExit_Click);
            // 
            // menuEdit
            // 
            this.menuEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miEditCopy,
            this.miEditPaste,
            this.toolStripSeparator3,
            this.miEditADPrefs});
            this.menuEdit.Name = "menuEdit";
            this.menuEdit.Size = new System.Drawing.Size(39, 20);
            this.menuEdit.Text = "&Edit";
            this.menuEdit.DropDownOpened += new System.EventHandler(this.menuEdit_DropDownOpened);
            // 
            // miEditCopy
            // 
            this.miEditCopy.Name = "miEditCopy";
            this.miEditCopy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.miEditCopy.Size = new System.Drawing.Size(152, 22);
            this.miEditCopy.Text = "&Copy";
            this.miEditCopy.Click += new System.EventHandler(this.miEditCopy_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(149, 6);
            // 
            // miEditADPrefs
            // 
            this.miEditADPrefs.Name = "miEditADPrefs";
            this.miEditADPrefs.Size = new System.Drawing.Size(152, 22);
            this.miEditADPrefs.Text = "A/D Prefs";
            this.miEditADPrefs.Click += new System.EventHandler(this.miEditADPrefs_Click);
            // 
            // menuHelp
            // 
            this.menuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miHelpAboout});
            this.menuHelp.Name = "menuHelp";
            this.menuHelp.Size = new System.Drawing.Size(44, 20);
            this.menuHelp.Text = "&Help";
            // 
            // miHelpAboout
            // 
            this.miHelpAboout.Name = "miHelpAboout";
            this.miHelpAboout.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.miHelpAboout.Size = new System.Drawing.Size(126, 22);
            this.miHelpAboout.Text = "&About";
            this.miHelpAboout.Click += new System.EventHandler(this.miHelpAboout_Click);
            // 
            // dlgOpen
            // 
            this.dlgOpen.DefaultExt = "jpg";
            this.dlgOpen.Filter = "JPEG Image|*.jpg|PNG Image|*.png|Bitmap Image|*.bmp|All Files|*.*";
            this.dlgOpen.ShowReadOnly = true;
            this.dlgOpen.Title = "Open Image";
            // 
            // dlgSaveAs
            // 
            this.dlgSaveAs.DefaultExt = "jpg";
            this.dlgSaveAs.Filter = "JPEG Image|*.jpg|All Files|*.*";
            this.dlgSaveAs.Title = "Save As";
            // 
            // label1
            // 
            this.label1.AllowDrop = true;
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(328, 234);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(125, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Drag && Drop images here";
            this.label1.DragDrop += new System.Windows.Forms.DragEventHandler(this.tabControl1_DragDrop);
            this.label1.DragEnter += new System.Windows.Forms.DragEventHandler(this.tabControl1_DragEnter);
            // 
            // miEditPaste
            // 
            this.miEditPaste.Name = "miEditPaste";
            this.miEditPaste.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.miEditPaste.Size = new System.Drawing.Size(152, 22);
            this.miEditPaste.Text = "&Paste";
            this.miEditPaste.Click += new System.EventHandler(this.miEditPaste_Click);
            // 
            // Form1
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(691, 480);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.tabControl1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Photo Crop";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.contextMenuStrip1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
		private System.Windows.Forms.ToolStripMenuItem tabctxSaveAll;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem menuFile;
		private System.Windows.Forms.ToolStripMenuItem miFileOpen;
		private System.Windows.Forms.ToolStripMenuItem miFileSave;
		private System.Windows.Forms.ToolStripMenuItem miFileSaveAs;
		private System.Windows.Forms.ToolStripMenuItem miFileClose;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem miFileSaveAll;
		private System.Windows.Forms.ToolStripMenuItem miFileCloseAll;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem miFileExit;
		private System.Windows.Forms.ToolStripMenuItem menuEdit;
		private System.Windows.Forms.ToolStripMenuItem miEditCopy;
		private System.Windows.Forms.ToolStripMenuItem menuHelp;
		private System.Windows.Forms.ToolStripMenuItem miHelpAboout;
		private System.Windows.Forms.OpenFileDialog dlgOpen;
		private System.Windows.Forms.SaveFileDialog dlgSaveAs;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripMenuItem miEditADPrefs;
        private System.Windows.Forms.ToolStripMenuItem miEditPaste;
    }
}

