namespace OMSUnlock
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			this.label1 = new System.Windows.Forms.Label();
			this.DocIDTB = new System.Windows.Forms.TextBox();
			this.SearchBtn = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
			this.UnlockBtn = new System.Windows.Forms.Button();
			this.LockButton = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 15);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(70, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Document ID";
			// 
			// DocIDTB
			// 
			this.DocIDTB.Location = new System.Drawing.Point(88, 12);
			this.DocIDTB.Name = "DocIDTB";
			this.DocIDTB.Size = new System.Drawing.Size(100, 20);
			this.DocIDTB.TabIndex = 1;
			this.DocIDTB.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DocIDTB_KeyDown);
			// 
			// SearchBtn
			// 
			this.SearchBtn.Image = ((System.Drawing.Image)(resources.GetObject("SearchBtn.Image")));
			this.SearchBtn.Location = new System.Drawing.Point(187, 10);
			this.SearchBtn.Name = "SearchBtn";
			this.SearchBtn.Size = new System.Drawing.Size(23, 23);
			this.SearchBtn.TabIndex = 2;
			this.SearchBtn.UseVisualStyleBackColor = true;
			this.SearchBtn.Click += new System.EventHandler(this.SearchBtn_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.propertyGrid1);
			this.groupBox1.Location = new System.Drawing.Point(12, 39);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(559, 175);
			this.groupBox1.TabIndex = 3;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Document Properties";
			// 
			// propertyGrid1
			// 
			this.propertyGrid1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.propertyGrid1.CategoryForeColor = System.Drawing.SystemColors.InactiveCaptionText;
			this.propertyGrid1.HelpVisible = false;
			this.propertyGrid1.Location = new System.Drawing.Point(6, 19);
			this.propertyGrid1.Name = "propertyGrid1";
			this.propertyGrid1.PropertySort = System.Windows.Forms.PropertySort.NoSort;
			this.propertyGrid1.Size = new System.Drawing.Size(547, 150);
			this.propertyGrid1.TabIndex = 0;
			this.propertyGrid1.ToolbarVisible = false;
			this.propertyGrid1.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid1_PropertyValueChanged);
			this.propertyGrid1.Resize += new System.EventHandler(this.propertyGrid1_Resize);
			// 
			// UnlockBtn
			// 
			this.UnlockBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.UnlockBtn.Enabled = false;
			this.UnlockBtn.Location = new System.Drawing.Point(496, 220);
			this.UnlockBtn.Name = "UnlockBtn";
			this.UnlockBtn.Size = new System.Drawing.Size(75, 23);
			this.UnlockBtn.TabIndex = 4;
			this.UnlockBtn.Text = "Check &In";
			this.UnlockBtn.UseVisualStyleBackColor = true;
			this.UnlockBtn.Click += new System.EventHandler(this.UnlockBtn_Click);
			// 
			// LockButton
			// 
			this.LockButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.LockButton.Enabled = false;
			this.LockButton.Location = new System.Drawing.Point(415, 220);
			this.LockButton.Name = "LockButton";
			this.LockButton.Size = new System.Drawing.Size(75, 23);
			this.LockButton.TabIndex = 5;
			this.LockButton.Text = "Check &Out";
			this.LockButton.UseVisualStyleBackColor = true;
			this.LockButton.Click += new System.EventHandler(this.LockButton_Click);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(583, 255);
			this.Controls.Add(this.LockButton);
			this.Controls.Add(this.UnlockBtn);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.SearchBtn);
			this.Controls.Add(this.DocIDTB);
			this.Controls.Add(this.label1);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Form1";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.Text = "Unlock OMS Document";
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox DocIDTB;
		private System.Windows.Forms.Button SearchBtn;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.PropertyGrid propertyGrid1;
		private System.Windows.Forms.Button UnlockBtn;
		private System.Windows.Forms.Button LockButton;
	}
}

