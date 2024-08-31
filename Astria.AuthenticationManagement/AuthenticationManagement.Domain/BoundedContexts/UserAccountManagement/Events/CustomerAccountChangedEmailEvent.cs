using AuthenticationManagement.Domain.BoundedContexts.UserAccountManagement.ValueObjects;
using Astria.SharedKernel;
using System;

namespace AuthenticationManagement.Domain.BoundedContexts.UserAccountManagement.Events
{
	public class CustomerAccountChangedEmailEvent : DomainEvent
	{
		public EmailAddress Email { get; protected set; }
		public UserFullName Name { get; protected set; }

		private CustomerAccountChangedEmailEvent()
		{ }

		public CustomerAccountChangedEmailEvent(Guid userId, EmailAddress email, UserFullName name)
		{
			AggregateId = userId;
			Email = email;
			Name = name;
		}
	}
}
