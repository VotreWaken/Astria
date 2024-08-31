using AuthenticationManagement.Domain.BoundedContexts.UserAccountManagement.ValueObjects;
using Astria.SharedKernel;
using System;

namespace AuthenticationManagement.Domain.BoundedContexts.UserAccountManagement.Events
{
	public class AdminAccountChangedEmailEvent : DomainEvent
	{
		public EmailAddress Email { get; protected set; }
		public UserFullName Name { get; protected set; }

		private AdminAccountChangedEmailEvent()
		{ }

		public AdminAccountChangedEmailEvent(Guid userId, EmailAddress email, UserFullName fullName)
		{
			AggregateId = userId;
			Email = email;
			Name = fullName;
		}
	}
}
