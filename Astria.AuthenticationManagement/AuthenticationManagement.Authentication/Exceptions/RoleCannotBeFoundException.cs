using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationManagement.Authentication.Exceptions
{
	class RoleCannotBeFoundException : AuthenticationException
	{
		public RoleCannotBeFoundException(string role) : base($"Idenity Role cannot be found: {role}") { }
	}
}
