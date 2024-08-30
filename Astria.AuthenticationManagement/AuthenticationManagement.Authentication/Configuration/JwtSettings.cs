using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationManagement.Authentication.Configuration
{
	public class JwtSettings
	{
		public string Issuer { get; set; }
		public string Secret { get; set; }
		public int ExpirationInMinutes { get; set; }
	}
}
