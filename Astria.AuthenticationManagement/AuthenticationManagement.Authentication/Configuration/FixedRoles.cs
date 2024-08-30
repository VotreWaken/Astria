using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationManagement.Authentication.Configuration
{
	public static class FixedRoles
	{
		public static string AdminRole { get; } = "Admin";
		public static string CustomerRole { get; } = "CUSTOMER";
	}
}
