namespace ADPhotoPrep
{
	partial class EditADPrefs
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
            this.label1 = new System.Windows.Forms.Label();
            this.LoginDomainTB = new System.Windows.Forms.TextBox();
            this.UsernameTB = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.PasswordTB = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.AutoCredentialsChk = new System.Windows.Forms.CheckBox();
            this.DomainTB = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 79);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Login Domain";
            // 
            // LoginDomainTB
            // 
            this.LoginDomainTB.Enabled = false;
            this.LoginDomainTB.Location = new System.Drawing.Point(98, 76);
            this.LoginDomainTB.Name = "LoginDomainTB";
            this.LoginDomainTB.Size = new System.Drawing.Size(175, 20);
            this.LoginDomainTB.TabIndex = 4;
            // 
            // UsernameTB
            // 
            this.UsernameTB.Enabled = false;
            this.UsernameTB.Location = new System.Drawing.Point(98, 102);
            this.UsernameTB.Name = "UsernameTB";
            this.UsernameTB.Size = new System.Drawing.Size(175, 20);
            this.UsernameTB.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 105);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Username";
            // 
            // PasswordTB
            // 
            this.PasswordTB.Enabled = false;
            this.PasswordTB.Location = new System.Drawing.Point(98, 128);
            this.PasswordTB.Name = "PasswordTB";
            this.PasswordTB.PasswordChar = '*';
            this.PasswordTB.Size = new System.Drawing.Size(175, 20);
            this.PasswordTB.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 131);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Password";
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(117, 164);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 9;
            this.button1.Text = "&OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(198, 164);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 10;
            this.button2.Text = "&Cancel";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // AutoCredentialsChk
            // 
            this.AutoCredentialsChk.AutoSize = true;
            this.AutoCredentialsChk.Checked = true;
            this.AutoCredentialsChk.CheckState = System.Windows.Forms.CheckState.Checked;
            this.AutoCredentialsChk.Location = new System.Drawing.Point(12, 49);
            this.AutoCredentialsChk.Name = "AutoCredentialsChk";
            this.AutoCredentialsChk.Size = new System.Drawing.Size(147, 17);
            this.AutoCredentialsChk.TabIndex = 2;
            this.AutoCredentialsChk.Text = "Use Windows Credentials";
            this.AutoCredentialsChk.UseVisualStyleBackColor = true;
            this.AutoCredentialsChk.CheckedChanged += new System.EventHandler(this.AutoCredentialsChk_CheckedChanged);
            // 
            // DomainTB
            // 
            this.DomainTB.Location = new System.Drawing.Point(98, 12);
            this.DomainTB.Name = "DomainTB";
            this.DomainTB.Size = new System.Drawing.Size(175, 20);
            this.DomainTB.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "A/D Domain";
            // 
            // EditADPrefs
            // 
            this.AcceptButton = this.button1;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button2;
            this.ClientSize = new System.Drawing.Size(285, 199);
            this.Controls.Add(this.DomainTB);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.AutoCredentialsChk);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.PasswordTB);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.UsernameTB);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.LoginDomainTB);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EditADPrefs";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit A/D Details";
            this.Shown += new System.EventHandler(this.EditADProps_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox LoginDomainTB;
		private System.Windows.Forms.TextBox UsernameTB;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox PasswordTB;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
        private System.Windows.Forms.CheckBox AutoCredentialsChk;
        private System.Windows.Forms.TextBox DomainTB;
        private System.Windows.Forms.Label label4;
    }
}