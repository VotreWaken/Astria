using AuthenticationManagement.Domain.Exceptions;

namespace AuthenticationManagement.Domain.BoundedContexts.UserAccountManagement.Exceptions
{
	public class ConfirmationEmailNotEqualException : DomainException
	{
		public ConfirmationEmailNotEqualException() : base("The Accounts existing email address does not match the provided confirmation email address.") { }
	}
}
