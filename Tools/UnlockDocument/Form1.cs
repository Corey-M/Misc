using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OMSUnlock
{
	public partial class Form1 : Form
	{
		DocumentInfo _info;

		public Form1()
		{
			InitializeComponent();
			this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);

			if (!UserChecks.IsAdmin)
			{
				LockButton.Visible = false;
				UnlockBtn.Visible = false;
				groupBox1.Height = (LockButton.Top + LockButton.Height - groupBox1.Top);
				Text = "Document CheckIn Status";
			}

#if testversion
			this.Text += " [Test Database]";
#endif
		}

		private void DocIDTB_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter && !(e.Alt || e.Control || e.Shift))
			{
				e.Handled = true;
				SearchBtn_Click(SearchBtn, EventArgs.Empty);
			}
		}

		private void SearchBtn_Click(object sender, EventArgs e)
		{
#if true
			long did = 0;
			if (!Int64.TryParse(DocIDTB.Text, out did))
				return;
			_info = DocumentInfo.Read(did);
#else
			_info = new DocumentInfo
			{
				DocumentID = 818283,
				ClientName = "Some Client",
				CaseName = "Legal case file name here",
				DocType = "SPREADSHEET",
				DocDescription = "Accounts.xls",
				CheckedOut = DateTime.Now,
				CheckoutUser = "Test User",
				CheckoutLocation = @"[PBN1XA4625]:[C:\Users\cbrehaut_DEG\AppData\Local\FWBS\OMS\0\PBN1SQL4604\OMSDATA\Documents\799908.1.pdf]",
				CheckoutUserID = 25
			};
#endif

			propertyGrid1.SelectedObject = _info;
			UpdateControls();
		}

		private void propertyGrid1_Resize(object sender, EventArgs e)
		{
			UpdateControls();
		}

		private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
		{
			UpdateControls();
        }

		private void UnlockBtn_Click(object sender, EventArgs e)
		{
			if (_info == null)
				return;
			if (_info.CheckInDocument())
				ReloadInfo();
			else
				MessageBox.Show("Failed to update database record.", "Unlock Failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}

		private void LockButton_Click(object sender, EventArgs e)
		{
			if (!(_info != null && _info.CheckedOut != null && _info.CheckoutUserID != null && !string.IsNullOrWhiteSpace(_info.CheckoutLocation)))
				return;
			if (_info.CheckOutDocument())
				ReloadInfo();
			else
				MessageBox.Show("Failed to update database record.", "Lock Failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}

		private void ReloadInfo()
		{
			if (_info == null || _info.DocumentID <= 0)
				return;
			_info = DocumentInfo.Read(_info.DocumentID);
			propertyGrid1.SelectedObject = _info;

			UpdateControls();
		}

		private void UpdateControls()
		{
			UnlockBtn.Enabled = _info != null && _info.CanCheckIn;
			LockButton.Enabled = _info != null && _info.CanCheckOut;
			SetLabelColumnWidth(propertyGrid1, 150);
		}

		public static void SetLabelColumnWidth(PropertyGrid grid, int width)
		{
			if (grid == null)
				throw new ArgumentNullException("grid");

			// get the grid view
			Control view = (Control)grid.GetType().GetField("gridView", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(grid);

			// set label width
			FieldInfo fi = view.GetType().GetField("labelWidth", BindingFlags.Instance | BindingFlags.NonPublic);
			fi.SetValue(view, width);

			// refresh
			view.Invalidate();
		}
	}
}
