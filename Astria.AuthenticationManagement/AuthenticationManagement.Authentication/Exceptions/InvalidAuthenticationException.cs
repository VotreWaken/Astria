using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationManagement.Authentication.Exceptions
{
	class InvalidAuthenticationException : AuthenticationException
	{
		public InvalidAuthenticationException() : base("Wrong Username or Password.") { }
	}
}
