using AuthenticationManagement.Domain.Exceptions;

namespace AuthenticationManagement.Domain.BoundedContexts.UserAccountManagement.Exceptions
{
	public class UserAccountNotFoundException : DomainException
	{
		public UserAccountNotFoundException() : base("User Account not found.") { }
	}
}
