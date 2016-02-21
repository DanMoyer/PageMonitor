using System;
using System.Security.Principal;
using Microsoft.AspNet.Identity;

namespace PageHitter
{
	public static class UserIdentity
	{
		public static string GetShortName(IPrincipal user)
		{
			var userName = user.Identity.GetUserName();

			if (string.IsNullOrEmpty(userName)) return string.Empty;

			var index     = userName.IndexOf("@", 1, StringComparison.Ordinal);
			var shortName = userName.Substring(0, index);

			return shortName;
		}
	}
}
