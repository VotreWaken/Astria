using AuthenticationManagement.Domain.BoundedContexts.UserAccountManagement.ValueObjects;
using Astria.SharedKernel;
using System;

namespace AuthenticationManagement.Domain.BoundedContexts.UserAccountManagement.Events
{
	public class AdminAccountChangedPasswordEvent : DomainEvent
	{
		public HashedPassword NewPassword { get; protected set; }

		private AdminAccountChangedPasswordEvent()
		{ }

		public AdminAccountChangedPasswordEvent(Guid userId, HashedPassword newPassword)
		{
			AggregateId = userId;
			NewPassword = newPassword;
		}
	}
}
