using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationManagement.Authentication.Exceptions
{
	class RoleCannotBeAssignedException : AuthenticationException
	{
		public RoleCannotBeAssignedException(string email) : base($"Idenity Role cannot be assigned to: {email}") { }
		public RoleCannotBeAssignedException(Guid userId) : base($"Idenity Role cannot be assigned to: {userId}") { }
	}
}
