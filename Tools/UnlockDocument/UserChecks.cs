using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace OMSUnlock
{
	internal static class UserChecks
	{
		static bool? _isAdmin;
		internal static bool IsAdmin
		{
			get
			{
				if (_isAdmin == null)
#if true
					_isAdmin = CheckUsername();
#else
					_isAdmin = CheckAD();
#endif
				return _isAdmin ?? false;
			}
		}

		internal static bool CheckUsername()
		{
			return Environment.UserName.EndsWith("_CSP", StringComparison.OrdinalIgnoreCase);
		}

		internal static bool CheckAD()
		{
			var user = UserPrincipal.Current;
			var groups = user.GetGroups().ToArray();

			if (!groups.Any(g => g.SamAccountName == "Domain Admins"))
				return false;

			return true;
		}
	}
}
