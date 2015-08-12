using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OMSUnlock
{
	/// <summary>
	/// Contains information about a document to be displayed to the user
	/// </summary>
	public class DocumentInfo
	{
		#region Browsable properties - will show in PropertyGrid on main form
		/// <summary>dbDocument.docID value</summary>
		[ReadOnly(true), DisplayName("Document ID")]
		public long DocumentID { get; set; }

		/// <summary>dbClient.clName value - Client Name</summary>
		[ReadOnly(true), DisplayName("Client Name")]
		public string ClientName { get; set; }

		/// <summary>dbFile.fileDesc value - Case Description</summary>
		[ReadOnly(true), DisplayName("Case Description")]
		public string CaseName { get; set; }

		/// <summary>dbDocument.docType value - Document Type</summary>
		[ReadOnly(true), DisplayName("Document Type")]
		public string DocType { get; set; }

		/// <summary>dbDocument.docDesc value - Document Description</summary>
		[ReadOnly(true), DisplayName("Document Description")]
		public string DocDescription { get; set; }

		/// <summary>dbDocument.docCheckedOut value = timestamp document was checked out</summary>
		//[ReadOnly(true)]
		[DisplayName("Checked Out At")]
		public DateTime? CheckedOut { get; set; }

		/// <summary>dbUser.usrFullName value - Checked Out User name</summary>
		[ReadOnly(true), DisplayName("Checked Out By")]
		public string CheckoutUser { get; set; }

		/// <summary>dbDocument.docCheckedOutlocation value</summary>
		//[ReadOnly(true)]
		[DisplayName("Checkout Location")]
		public string CheckoutLocation { get; set; }

		/// <summary></summary>
		//[ReadOnly(true)]
		[DisplayName("Checkout User ID")]
		public int? CheckoutUserID { get; set; }
		#endregion

		#region Non-browsable properties - will not be displayed in PropertyGrid
		[Browsable(false)]
		public bool CanCheckIn
		{
			get
			{
				return CheckedOut != null || CheckoutUserID != null || !string.IsNullOrWhiteSpace(CheckoutLocation);
			}
		}

		[Browsable(false)]
		public bool CanCheckOut
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
		public bool CheckInDocument()
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
		public bool CheckOutDocument(DateTime? checkoutTime = null, int checkoutUser = 0, string checkoutLocation = null)
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

		#region Static database read methods
		/// <summary>
		/// Convert an IDataReader instance's contents into a stream of DocumentInfo objects
		/// </summary>
		/// <param name="reader">IDataReader instance to convert</param>
		/// <returns>Enumeration of DocumentInfo</returns>
		public static IEnumerable<DocumentInfo> FromDataReader(IDataReader reader)
		{
			while (reader.Read())
			{
				DocumentInfo res = null;
				try
				{
					res = new DocumentInfo
					{
						DocumentID = reader.LongValue(0) ?? 0,
						ClientName = reader.StringValue(1),
						CaseName = reader.StringValue(2),
						DocType = reader.StringValue(3),
						DocDescription = reader.StringValue(4),
						CheckedOut = reader.DateTimeValue(5),
						CheckoutUser = reader.StringValue(6),
						CheckoutLocation = reader.StringValue(7),
						CheckoutUserID = reader.IntValue(8)
					};
				}
				catch
				{
				}
				if (res == null)
					yield break;

				yield return res;
			}
		}

		static string ConnectionString()
		{
			var csb = new SqlConnectionStringBuilder();
			csb.DataSource = "pbn1sql4604";
#if testversion
			csb.InitialCatalog = "omsdata_test";
#else
			csb.InitialCatalog = "omsdata";
#endif
			csb.IntegratedSecurity = true;

			return csb.ConnectionString;
		}

		/// <summary>
		/// Load a particular DocumentInfo record base on the supplied Document ID
		/// </summary>
		/// <param name="docID">Document ID to load</param>
		/// <returns>DocumentInfo instance if found, null otherwise</returns>
		public static DocumentInfo Read(long docID)
		{
			using (var conn = new SqlConnection(ConnectionString()))
			{
				conn.Open();
				using (var cmd = conn.CreateCommand())
				{
					cmd.CommandType = CommandType.Text;
					cmd.CommandText = sqlDocumentInfo;
					cmd.Parameters.AddWithValue("@ID", docID);

					using (var rdr = cmd.ExecuteReader())
					{
						return FromDataReader(rdr).FirstOrDefault();
					}
				}
			}
		}
#endregion

#region SQL Query strings
		const string sqlDocumentInfo =
			@"SELECT
	d.docID, c.clName, f.fileDesc, d.docType, d.docDesc, 
	d.docCheckedOut, u.usrFullName, d.docCheckedOutlocation,
	d.docCheckedOutBy
FROM dbDocument d WITH(NOLOCK)
LEFT JOIN dbUser u WITH(NOLOCK) ON d.docCheckedOutBy = u.usrID
LEFT JOIN dbClient c WITH(NOLOCK) ON d.clID = c.clID
LEFT JOIN dbFile f WITH(NOLOCK) ON d.fileID = f.fileID
WHERE d.docID = @ID";

		const string sqlClearDocLock =
			@"UPDATE dbDocument
SET docCheckedOut = NULL, docCheckedOutBy = NULL, docCheckedOutLocation = NULL
WHERE docID = @ID";

		const string sqlSetDocLock =
			@"UPDATE dbDocument
SET docCheckedOut = @TS, docCheckedOutBy = @uID, docCheckedOutLocation = @loc
WHERE docID = @ID";

#endregion
	}
}
