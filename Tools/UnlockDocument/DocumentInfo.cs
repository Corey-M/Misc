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
	[TypeConverter(typeof(PropertySorter))]
	/// <summary>
	/// Contains information about a document to be displayed to the user
	/// </summary>
	public class DocumentInfo
	{
		#region Browsable properties - will show in PropertyGrid on main form
		/// <summary>dbDocument.docID value</summary>
		[ReadOnly(true), DisplayName("Document ID"), PropertyOrder(10)]
		public long DocumentID { get; set; }

		/// <summary>dbClient.clName value - Client Name</summary>
		[ReadOnly(true), DisplayName("Client Name"), PropertyOrder(20)]
		public string ClientName { get; set; }

		/// <summary>dbFile.fileDesc value - Case Description</summary>
		[ReadOnly(true), DisplayName("Case Description"), PropertyOrder(30)]
		public string CaseName { get; set; }

		/// <summary>dbDocument.docType value - Document Type</summary>
		[ReadOnly(true), DisplayName("Document Type"), PropertyOrder(40)]
		public string DocType { get; set; }

		/// <summary>dbDocument.docDesc value - Document Description</summary>
		[ReadOnly(true), DisplayName("Document Description"), PropertyOrder(50)]
		public string DocDescription { get; set; }

		/// <summary>dbDocument.docCheckedOut value = timestamp document was checked out</summary>
		[ReadOnly(true), DisplayName("Checked Out At"), PropertyOrder(60)]
		public virtual DateTime? CheckedOut { get; set; }

		/// <summary>dbUser.usrFullName value - Checked Out User name</summary>
		[ReadOnly(true), DisplayName("Checked Out By"), PropertyOrder(70)]
		public string CheckoutUser { get; set; }

		/// <summary>dbDocument.docCheckedOutlocation value</summary>
		[ReadOnly(true), DisplayName("Checkout Location"), PropertyOrder(80)]
		public virtual string CheckoutLocation { get; set; }

		/// <summary></summary>
		[ReadOnly(true), DisplayName("Checkout User ID"), PropertyOrder(90)]
		public virtual int? CheckoutUserID { get; set; }
		#endregion

		#region Non-browsable properties - will not be displayed in PropertyGrid
		[Browsable(false)]
		public virtual bool CanCheckIn { get { return false; } }

		[Browsable(false)]
		public virtual bool CanCheckOut { get { return false; } }
		#endregion

		#region Checkout operations
		/// <summary>
		/// Update the database to remove the document lock from this document
		/// </summary>
		/// <returns>True if database updated, false otherwise</returns>
		public virtual bool CheckInDocument()
		{
			return false;
		}

		/// <summary>Lock the document using the supplied data or using the current values</summary>
		/// <param name="checkoutTime">Optional lock timestamp</param>
		/// <param name="checkoutUser">Optional user ID</param>
		/// <param name="checkoutLocation">Optional location</param>
		/// <returns>True if lock applied, false otherwise</returns>
		public virtual bool CheckOutDocument(DateTime? checkoutTime = null, int checkoutUser = 0, string checkoutLocation = null)
		{
			return false;
		}
		#endregion

		#region Static database read methods
		protected static string ConnectionString()
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
					if (UserChecks.IsAdmin)
						res = new UpatableDocumentInfo();
					else
						res = new DocumentInfo();

					res.DocumentID = reader.LongValue(0) ?? 0;
					res.ClientName = reader.StringValue(1);
					res.CaseName = reader.StringValue(2);
					res.DocType = reader.StringValue(3);
					res.DocDescription = reader.StringValue(4);
					res.CheckedOut = reader.DateTimeValue(5);
					res.CheckoutUser = reader.StringValue(6);
					res.CheckoutLocation = reader.StringValue(7);
					res.CheckoutUserID = reader.IntValue(8);
				}
				catch
				{
				}
				if (res == null)
					yield break;

				yield return res;
			}
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
		protected const string sqlDocumentInfo =
			@"SELECT
	d.docID, c.clName, f.fileDesc, d.docType, d.docDesc, 
	d.docCheckedOut, u.usrFullName, d.docCheckedOutlocation,
	d.docCheckedOutBy
FROM dbDocument d WITH(NOLOCK)
LEFT JOIN dbUser u WITH(NOLOCK) ON d.docCheckedOutBy = u.usrID
LEFT JOIN dbClient c WITH(NOLOCK) ON d.clID = c.clID
LEFT JOIN dbFile f WITH(NOLOCK) ON d.fileID = f.fileID
WHERE d.docID = @ID";
#endregion
	}
}
