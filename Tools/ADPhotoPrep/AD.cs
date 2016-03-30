using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ADPhotoPrep
{
	static class AD
	{
        public static bool UseWindowsCreds { get { return Properties.Settings.Default.UseWindowsCredentials; } set { Properties.Settings.Default.UseWindowsCredentials = value; } }

		public static string DomainName
		{
			get { return Properties.Settings.Default.ADDomain; }
			set { Properties.Settings.Default.ADDomain = value; }
		}

        public static string LoginDomain
        {
            get { return Properties.Settings.Default.UserDomain; }
            set { Properties.Settings.Default.UserDomain = value; }
        }

		public static string Username
		{
			internal get { return Properties.Settings.Default.Username; }
			set { Properties.Settings.Default.Username = value; }
		}

		public static string Password
		{
			internal get { return Properties.Settings.Default.SecurePassword; }
			set { Properties.Settings.Default.SecurePassword = value; }
		}

		public static DirectoryEntry FindUser(string userName)
		{
			if (string.IsNullOrEmpty(LoginDomain) || !UseWindowsCreds && (string.IsNullOrEmpty(DomainName) || string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password)))
			{
				if (EditADPrefs.Execute() != System.Windows.Forms.DialogResult.OK)
					return null;
			}

			try
			{
                DirectoryEntry dom = null;
                if (UseWindowsCreds)
                    dom = new DirectoryEntry("LDAP://" + DomainName);
                else
                    dom = new DirectoryEntry("LDAP://" + DomainName, LoginDomain + @"\" + Username, Password, AuthenticationTypes.None);
				using (DirectorySearcher dsSearcher = new DirectorySearcher(dom))
				{
					dsSearcher.Filter = string.Format("(&(objectClass=user)(|(cn={0})(samaccountname={0})))", userName);
					dsSearcher.PropertiesToLoad.Add("ThumbnailPhoto");
					SearchResult result = dsSearcher.FindOne();

					if (result == null || string.IsNullOrEmpty(result.Path))
						return null;

                    if (UseWindowsCreds)
                        return new DirectoryEntry(result.Path);
					return new DirectoryEntry(result.Path, LoginDomain + @"\" + Username, Password, AuthenticationTypes.None);
				}
			}
			catch (Exception e)
			{
				MessageBox.Show(string.Format("Failed to search for user.\r\n\r\nError was:\r\n{0}", e.Message), "A/D Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
			return null;
		}

		public static DialogResult InputBox(string title, string promptText, ref string value)
		{
			Form form = new Form();
			Label label = new Label();
			TextBox textBox = new TextBox();
			Button buttonOk = new Button();
			Button buttonCancel = new Button();

			form.Text = title;
			label.Text = promptText;
			textBox.Text = value;

			buttonOk.Text = "OK";
			buttonCancel.Text = "Cancel";
			buttonOk.DialogResult = DialogResult.OK;
			buttonCancel.DialogResult = DialogResult.Cancel;

			label.SetBounds(9, 20, 372, 13);
			textBox.SetBounds(12, 36, 372, 20);
			buttonOk.SetBounds(228, 72, 75, 23);
			buttonCancel.SetBounds(309, 72, 75, 23);

			label.AutoSize = true;
			textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
			buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

			form.ClientSize = new Size(396, 107);
			form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
			form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
			form.FormBorderStyle = FormBorderStyle.FixedDialog;
			form.StartPosition = FormStartPosition.CenterScreen;
			form.MinimizeBox = false;
			form.MaximizeBox = false;
			form.AcceptButton = buttonOk;
			form.CancelButton = buttonCancel;

			DialogResult dialogResult = form.ShowDialog();
			value = textBox.Text;
			return dialogResult;
		}
	}
}
