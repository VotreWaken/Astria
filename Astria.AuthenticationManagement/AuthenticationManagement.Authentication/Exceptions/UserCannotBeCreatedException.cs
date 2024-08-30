using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationManagement.Authentication.Exceptions
{
	class UserCannotBeCreatedException : AuthenticationException
	{
		public UserCannotBeCreatedException(string email) : base($"Idenity User cannot be created: {email}") { }
	}
}
