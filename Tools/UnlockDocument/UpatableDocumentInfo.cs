using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace OMSUnlock
{
	[TypeConverter(typeof(PropertySorter))]
	public class UpatableDocumentInfo : DocumentInfo
	{
		/// <summary>dbDocument.docCheckedOut value = timestamp document was checked out</summary>
		[ReadOnly(false), DisplayName("Checked Out At"), PropertyOrder(60)]
		public override DateTime? CheckedOut { get; set; }

		/// <summary>dbDocument.docCheckedOutlocation value</summary>
		[ReadOnly(false), DisplayName("Checkout Location"), PropertyOrder(80)]
		public override string CheckoutLocation { get; set; }

		/// <summary></summary>
		[ReadOnly(false), DisplayName("Checkout User ID"), PropertyOrder(90)]
		public override int? CheckoutUserID { get; set; }


		#region Non-browsable properties - will not be displayed in PropertyGrid
		[Browsable(false)]
		public override bool CanCheckIn
		{
			get
			{
				return CheckedOut != null || CheckoutUserID != null || !string.IsNullOrWhiteSpace(CheckoutLocation);
			}
		}

		[Browsable(false)]
		public override bool CanCheckOut
		{
			get
			{
				return CheckedOut != null && CheckoutUserID != null && !string.IsNullOrWhiteSpace(CheckoutLocation);
			}
		}
		#endregion

		#region Checkout operations
		/// <summary>
		/// Update the database to remove the document lock from this document
		/// </summary>
		/// <returns>True if database updated, false otherwise</returns>
		public override bool CheckInDocument()
		{
			if (!CanCheckIn)
				return false;

			using (var conn = new SqlConnection(ConnectionString()))
			{
				conn.Open();
				using (var cmd = conn.CreateCommand())
				{
					cmd.CommandType = CommandType.Text;
					cmd.CommandText = sqlClearDocLock;
					cmd.Parameters.AddWithValue("@ID", DocumentID);

					int rowcount = cmd.ExecuteNonQuery();
					//MessageBox.Show(string.Format("Affected {0} record{1}", rowcount, rowcount == 1 ? "" : "s"));
					return rowcount != 0;
				}
			}
		}

		/// <summary>Lock the document using the supplied data or using the current values</summary>
		/// <param name="checkoutTime">Optional lock timestamp</param>
		/// <param name="checkoutUser">Optional user ID</param>
		/// <param name="checkoutLocation">Optional location</param>
		/// <returns>True if lock applied, false otherwise</returns>
		public override bool CheckOutDocument(DateTime? checkoutTime = null, int checkoutUser = 0, string checkoutLocation = null)
		{
			if (checkoutTime == null && checkoutUser == 0 && CheckoutLocation == null && !CanCheckOut)
				return false;

			checkoutTime = checkoutTime ?? CheckedOut;
			checkoutUser = checkoutUser <= 0 ? (CheckoutUserID ?? 0) : checkoutUser;
			checkoutLocation = checkoutLocation ?? CheckoutLocation;

			if (checkoutTime == DateTime.MinValue || checkoutUser == -1 || string.IsNullOrWhiteSpace(checkoutLocation))
				return false;

			using (var conn = new SqlConnection(ConnectionString()))
			{
				conn.Open();
				using (var cmd = conn.CreateCommand())
				{
					cmd.CommandType = CommandType.Text;
					cmd.CommandText = sqlSetDocLock;
					cmd.Parameters.AddWithValue("@TS", checkoutTime.Value);
					cmd.Parameters.AddWithValue("@uID", CheckoutUserID);
					cmd.Parameters.AddWithValue("@loc", CheckoutLocation);
					cmd.Parameters.AddWithValue("@ID", DocumentID);

					int rowcount = cmd.ExecuteNonQuery();
					//MessageBox.Show(string.Format("Affected {0} record{1}", rowcount, rowcount == 1 ? "" : "s"));
					return rowcount != 0;
				}
			}
		}
		#endregion

		#region SQL Query strings
		protected const string sqlClearDocLock =
			@"UPDATE dbDocument
SET docCheckedOut = NULL, docCheckedOutBy = NULL, docCheckedOutLocation = NULL
WHERE docID = @ID";

		protected const string sqlSetDocLock =
			@"UPDATE dbDocument
SET docCheckedOut = @TS, docCheckedOutBy = @uID, docCheckedOutLocation = @loc
WHERE docID = @ID";

		#endregion
	}
}
