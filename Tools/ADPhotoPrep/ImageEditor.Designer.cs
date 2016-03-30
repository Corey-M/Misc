namespace ADPhotoPrep
{
	partial class ImageEditor
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.pbctxLoad = new System.Windows.Forms.ToolStripMenuItem();
            this.pbctxSave = new System.Windows.Forms.ToolStripMenuItem();
            this.pbctxUpdateAD = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.pbctxReset = new System.Windows.Forms.ToolStripMenuItem();
            this.pbctxClose = new System.Windows.Forms.ToolStripMenuItem();
            this.panel2 = new System.Windows.Forms.Panel();
            this.AspectGrp = new System.Windows.Forms.GroupBox();
            this.aspect5_4 = new System.Windows.Forms.RadioButton();
            this.aspect3_2 = new System.Windows.Forms.RadioButton();
            this.aspect4_3 = new System.Windows.Forms.RadioButton();
            this.aspect8_7 = new System.Windows.Forms.RadioButton();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.numericUpDown3 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.previewL = new System.Windows.Forms.Label();
            this.previewPB = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.previewTimer = new System.Windows.Forms.Timer(this.components);
            this.panel1 = new ADPhotoPrep.SelectablePanel();
            this.picturebox = new System.Windows.Forms.PictureBox();
            this.btnUpdateAD = new System.Windows.Forms.Button();
            this.contextMenuStrip1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.AspectGrp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.previewPB)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picturebox)).BeginInit();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pbctxLoad,
            this.pbctxSave,
            this.pbctxUpdateAD,
            this.toolStripSeparator1,
            this.pbctxReset,
            this.pbctxClose});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(137, 120);
            // 
            // pbctxLoad
            // 
            this.pbctxLoad.Name = "pbctxLoad";
            this.pbctxLoad.Size = new System.Drawing.Size(136, 22);
            this.pbctxLoad.Text = "Load";
            this.pbctxLoad.Visible = false;
            // 
            // pbctxSave
            // 
            this.pbctxSave.Name = "pbctxSave";
            this.pbctxSave.Size = new System.Drawing.Size(136, 22);
            this.pbctxSave.Text = "Save";
            this.pbctxSave.Click += new System.EventHandler(this.pbctxSave_Click);
            // 
            // pbctxUpdateAD
            // 
            this.pbctxUpdateAD.Name = "pbctxUpdateAD";
            this.pbctxUpdateAD.Size = new System.Drawing.Size(136, 22);
            this.pbctxUpdateAD.Text = "Update A/D";
            this.pbctxUpdateAD.Click += new System.EventHandler(this.pbctxUpdateAD_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(133, 6);
            // 
            // pbctxReset
            // 
            this.pbctxReset.Name = "pbctxReset";
            this.pbctxReset.Size = new System.Drawing.Size(136, 22);
            this.pbctxReset.Text = "Reset";
            this.pbctxReset.Click += new System.EventHandler(this.pbctxReset_Click);
            // 
            // pbctxClose
            // 
            this.pbctxClose.Name = "pbctxClose";
            this.pbctxClose.Size = new System.Drawing.Size(136, 22);
            this.pbctxClose.Text = "Close";
            this.pbctxClose.Click += new System.EventHandler(this.pbctxClose_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnUpdateAD);
            this.panel2.Controls.Add(this.AspectGrp);
            this.panel2.Controls.Add(this.trackBar1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 302);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(529, 42);
            this.panel2.TabIndex = 2;
            // 
            // AspectGrp
            // 
            this.AspectGrp.Controls.Add(this.aspect5_4);
            this.AspectGrp.Controls.Add(this.aspect3_2);
            this.AspectGrp.Controls.Add(this.aspect4_3);
            this.AspectGrp.Controls.Add(this.aspect8_7);
            this.AspectGrp.Location = new System.Drawing.Point(140, 3);
            this.AspectGrp.Name = "AspectGrp";
            this.AspectGrp.Size = new System.Drawing.Size(229, 34);
            this.AspectGrp.TabIndex = 1;
            this.AspectGrp.TabStop = false;
            this.AspectGrp.Text = "Aspect Ratio";
            // 
            // aspect5_4
            // 
            this.aspect5_4.AutoSize = true;
            this.aspect5_4.Checked = true;
            this.aspect5_4.Location = new System.Drawing.Point(64, 11);
            this.aspect5_4.Name = "aspect5_4";
            this.aspect5_4.Size = new System.Drawing.Size(40, 17);
            this.aspect5_4.TabIndex = 1;
            this.aspect5_4.TabStop = true;
            this.aspect5_4.Text = "5:4";
            this.aspect5_4.UseVisualStyleBackColor = true;
            this.aspect5_4.CheckedChanged += new System.EventHandler(this.AspectRatio_CheckChanged);
            // 
            // aspect3_2
            // 
            this.aspect3_2.AutoSize = true;
            this.aspect3_2.Location = new System.Drawing.Point(179, 11);
            this.aspect3_2.Name = "aspect3_2";
            this.aspect3_2.Size = new System.Drawing.Size(40, 17);
            this.aspect3_2.TabIndex = 3;
            this.aspect3_2.Text = "3:2";
            this.aspect3_2.UseVisualStyleBackColor = true;
            this.aspect3_2.CheckedChanged += new System.EventHandler(this.AspectRatio_CheckChanged);
            // 
            // aspect4_3
            // 
            this.aspect4_3.AutoSize = true;
            this.aspect4_3.Location = new System.Drawing.Point(123, 11);
            this.aspect4_3.Name = "aspect4_3";
            this.aspect4_3.Size = new System.Drawing.Size(40, 17);
            this.aspect4_3.TabIndex = 2;
            this.aspect4_3.Text = "4:3";
            this.aspect4_3.UseVisualStyleBackColor = true;
            this.aspect4_3.CheckedChanged += new System.EventHandler(this.AspectRatio_CheckChanged);
            // 
            // aspect8_7
            // 
            this.aspect8_7.AutoSize = true;
            this.aspect8_7.Location = new System.Drawing.Point(11, 11);
            this.aspect8_7.Name = "aspect8_7";
            this.aspect8_7.Size = new System.Drawing.Size(40, 17);
            this.aspect8_7.TabIndex = 0;
            this.aspect8_7.Text = "8:7";
            this.aspect8_7.UseVisualStyleBackColor = true;
            this.aspect8_7.CheckedChanged += new System.EventHandler(this.AspectRatio_CheckChanged);
            // 
            // trackBar1
            // 
            this.trackBar1.Location = new System.Drawing.Point(3, 3);
            this.trackBar1.Margin = new System.Windows.Forms.Padding(0);
            this.trackBar1.Maximum = 100;
            this.trackBar1.Minimum = 10;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(134, 45);
            this.trackBar1.TabIndex = 0;
            this.trackBar1.TickFrequency = 10;
            this.trackBar1.Value = 50;
            this.trackBar1.ValueChanged += new System.EventHandler(this.trackBar1_ValueChanged);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.panel4);
            this.panel3.Controls.Add(this.previewL);
            this.panel3.Controls.Add(this.previewPB);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel3.Location = new System.Drawing.Point(404, 0);
            this.panel3.MinimumSize = new System.Drawing.Size(120, 0);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new System.Windows.Forms.Padding(3);
            this.panel3.Size = new System.Drawing.Size(125, 302);
            this.panel3.TabIndex = 1;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.label4);
            this.panel4.Controls.Add(this.label3);
            this.panel4.Controls.Add(this.label2);
            this.panel4.Controls.Add(this.numericUpDown3);
            this.panel4.Controls.Add(this.numericUpDown2);
            this.panel4.Controls.Add(this.numericUpDown1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(3, 221);
            this.panel4.Margin = new System.Windows.Forms.Padding(0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(119, 78);
            this.panel4.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(0, 57);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Gamma";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(0, 31);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Contrast";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(0, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Brightness";
            // 
            // numericUpDown3
            // 
            this.numericUpDown3.DecimalPlaces = 2;
            this.numericUpDown3.Increment = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            this.numericUpDown3.Location = new System.Drawing.Point(60, 55);
            this.numericUpDown3.Maximum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.numericUpDown3.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            65536});
            this.numericUpDown3.Name = "numericUpDown3";
            this.numericUpDown3.Size = new System.Drawing.Size(52, 20);
            this.numericUpDown3.TabIndex = 5;
            this.numericUpDown3.Value = new decimal(new int[] {
            2,
            0,
            0,
            65536});
            this.numericUpDown3.ValueChanged += new System.EventHandler(this.numericUpDown3_ValueChanged);
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.DecimalPlaces = 2;
            this.numericUpDown2.Increment = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            this.numericUpDown2.Location = new System.Drawing.Point(60, 29);
            this.numericUpDown2.Maximum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.numericUpDown2.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            65536});
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(52, 20);
            this.numericUpDown2.TabIndex = 3;
            this.numericUpDown2.Value = new decimal(new int[] {
            2,
            0,
            0,
            65536});
            this.numericUpDown2.ValueChanged += new System.EventHandler(this.numericUpDown2_ValueChanged);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.DecimalPlaces = 2;
            this.numericUpDown1.Increment = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            this.numericUpDown1.Location = new System.Drawing.Point(60, 3);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            65536});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(52, 20);
            this.numericUpDown1.TabIndex = 1;
            this.numericUpDown1.Value = new decimal(new int[] {
            2,
            0,
            0,
            65536});
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // previewL
            // 
            this.previewL.Dock = System.Windows.Forms.DockStyle.Top;
            this.previewL.Location = new System.Drawing.Point(3, 73);
            this.previewL.Name = "previewL";
            this.previewL.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.previewL.Size = new System.Drawing.Size(119, 86);
            this.previewL.TabIndex = 3;
            // 
            // previewPB
            // 
            this.previewPB.Dock = System.Windows.Forms.DockStyle.Top;
            this.previewPB.Location = new System.Drawing.Point(3, 23);
            this.previewPB.Name = "previewPB";
            this.previewPB.Size = new System.Drawing.Size(119, 50);
            this.previewPB.TabIndex = 2;
            this.previewPB.TabStop = false;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(119, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Preview";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // previewTimer
            // 
            this.previewTimer.Interval = 50;
            this.previewTimer.Tick += new System.EventHandler(this.previewTimer_Tick);
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.ContextMenuStrip = this.contextMenuStrip1;
            this.panel1.Controls.Add(this.picturebox);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(276, 244);
            this.panel1.TabIndex = 0;
            this.panel1.TabStop = true;
            this.panel1.ClientSizeChanged += new System.EventHandler(this.panel1_ClientSizeChanged);
            // 
            // picturebox
            // 
            this.picturebox.ContextMenuStrip = this.contextMenuStrip1;
            this.picturebox.Location = new System.Drawing.Point(1, 1);
            this.picturebox.Margin = new System.Windows.Forms.Padding(2);
            this.picturebox.MinimumSize = new System.Drawing.Size(100, 100);
            this.picturebox.Name = "picturebox";
            this.picturebox.Size = new System.Drawing.Size(100, 100);
            this.picturebox.TabIndex = 1;
            this.picturebox.TabStop = false;
            this.picturebox.Paint += new System.Windows.Forms.PaintEventHandler(this.picturebox_Paint);
            this.picturebox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picturebox_MouseDown);
            this.picturebox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picturebox_MouseMove);
            this.picturebox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picturebox_MouseUp);
            // 
            // btnUpdateAD
            // 
            this.btnUpdateAD.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpdateAD.Location = new System.Drawing.Point(444, 8);
            this.btnUpdateAD.Name = "btnUpdateAD";
            this.btnUpdateAD.Size = new System.Drawing.Size(75, 23);
            this.btnUpdateAD.TabIndex = 2;
            this.btnUpdateAD.Text = "Update A/D";
            this.btnUpdateAD.UseVisualStyleBackColor = true;
            this.btnUpdateAD.Click += new System.EventHandler(this.btnUpdateAD_Click);
            // 
            // ImageEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Name = "ImageEditor";
            this.Size = new System.Drawing.Size(529, 344);
            this.Resize += new System.EventHandler(this.ImageEditor_Resize);
            this.contextMenuStrip1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.AspectGrp.ResumeLayout(false);
            this.AspectGrp.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.previewPB)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picturebox)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.PictureBox picturebox;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.TrackBar trackBar1;
		private System.Windows.Forms.GroupBox AspectGrp;
		private System.Windows.Forms.RadioButton aspect3_2;
		private System.Windows.Forms.RadioButton aspect4_3;
		private System.Windows.Forms.RadioButton aspect8_7;
		private System.Windows.Forms.RadioButton aspect5_4;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
		private System.Windows.Forms.ToolStripMenuItem pbctxLoad;
		private System.Windows.Forms.ToolStripMenuItem pbctxSave;
		private System.Windows.Forms.ToolStripMenuItem pbctxReset;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Label previewL;
		private System.Windows.Forms.PictureBox previewPB;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ToolStripMenuItem pbctxClose;
		private System.Windows.Forms.Panel panel4;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.NumericUpDown numericUpDown3;
		private System.Windows.Forms.NumericUpDown numericUpDown2;
		private System.Windows.Forms.NumericUpDown numericUpDown1;
		private System.Windows.Forms.Timer previewTimer;
		private SelectablePanel panel1;
		private System.Windows.Forms.ToolStripMenuItem pbctxUpdateAD;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.Button btnUpdateAD;
    }
}
