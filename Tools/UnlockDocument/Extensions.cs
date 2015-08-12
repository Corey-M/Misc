using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMSUnlock
{
	public static class Extensions
	{
		public static int? IntValue(this IDataReader reader, int index)
		{
			return reader.IsDBNull(index) ? (int?)null : reader.GetInt32(index);
		}

		public static DateTime? DateTimeValue(this IDataReader reader, int index)
		{
			return reader.IsDBNull(index) ? (DateTime?)null : reader.GetDateTime(index);
		}

		public static string StringValue(this IDataReader reader, int index)
		{
			return reader.IsDBNull(index) ? null : reader.GetString(index);
		}

		public static long? LongValue(this IDataReader reader, int index)
		{
			return reader.IsDBNull(index) ? (long?)null : reader.GetInt64(index);
		}
	}
}
