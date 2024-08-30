using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationManagement.Authentication.Exceptions
{
	class UserCannotBeFoundException : AuthenticationException
	{
		public UserCannotBeFoundException(string email) : base($"Idenity User cannot be found: {email}") { }
		public UserCannotBeFoundException(Guid userId) : base($"Idenity User cannot be found: {userId}") { }
	}
}
