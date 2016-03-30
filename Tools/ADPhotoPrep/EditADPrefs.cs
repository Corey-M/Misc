using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ADPhotoPrep
{
	public partial class EditADPrefs : Form
	{
		static string defpwd = "**UNCHANGED**";

		public EditADPrefs()
		{
			InitializeComponent();
		}

		private void EditADProps_Shown(object sender, EventArgs e)
		{
            DomainTB.Text = string.IsNullOrEmpty(AD.DomainName) ? Environment.GetEnvironmentVariable("USERDOMAIN") : AD.DomainName;
            AutoCredentialsChk.Checked = AD.UseWindowsCreds;
            LoginDomainTB.Text = string.IsNullOrEmpty(AD.LoginDomain) ? Environment.GetEnvironmentVariable("USERDOMAIN") : AD.LoginDomain;
            UsernameTB.Text = string.IsNullOrEmpty(AD.Username) ? Environment.GetEnvironmentVariable("USERNAME") : AD.Username;
			PasswordTB.Text = defpwd;

            AutoCredentialsChk_CheckedChanged(this, EventArgs.Empty);
		}

        private void AutoCredentialsChk_CheckedChanged(object sender, EventArgs e)
        {
            LoginDomainTB.Enabled = UsernameTB.Enabled = PasswordTB.Enabled = !AutoCredentialsChk.Checked;
        }

        private void button1_Click(object sender, EventArgs e)
		{
            AD.DomainName = DomainTB.Text;
            AD.UseWindowsCreds = AutoCredentialsChk.Checked;
			AD.LoginDomain = LoginDomainTB.Text;
			AD.Username = UsernameTB.Text;
			if (PasswordTB.Text != defpwd)
				AD.Password = PasswordTB.Text;
            
		}

		public static DialogResult Execute()
		{
			using (EditADPrefs frm = new EditADPrefs())
			{
				return frm.ShowDialog();
			}
		}
    }
}
