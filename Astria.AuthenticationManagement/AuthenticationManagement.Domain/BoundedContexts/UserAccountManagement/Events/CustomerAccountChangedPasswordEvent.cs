using AuthenticationManagement.Domain.BoundedContexts.UserAccountManagement.ValueObjects;
using Astria.SharedKernel;
using System;

namespace AuthenticationManagement.Domain.BoundedContexts.UserAccountManagement.Events
{
	public class CustomerAccountChangedPasswordEvent : DomainEvent
	{
		public HashedPassword NewPassword { get; protected set; }

		private CustomerAccountChangedPasswordEvent()
		{ }

		public CustomerAccountChangedPasswordEvent(Guid userId, HashedPassword newPassword)
		{
			AggregateId = userId;
			NewPassword = newPassword;
		}
	}
}
