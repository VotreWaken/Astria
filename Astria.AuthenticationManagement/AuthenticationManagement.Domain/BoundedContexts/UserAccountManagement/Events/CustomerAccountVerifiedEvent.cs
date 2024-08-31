using AuthenticationManagement.Domain.BoundedContexts.UserAccountManagement.ValueObjects;
using Astria.SharedKernel;
using System;

namespace AuthenticationManagement.Domain.BoundedContexts.UserAccountManagement.Events
{
	public class CustomerAccountVerifiedEvent : DomainEvent
	{
		public UserFullName Name { get; private set; }
		public EmailAddress Email { get; private set; }
		public bool Verified { get; private set; }

		private CustomerAccountVerifiedEvent()
		{ }

		internal CustomerAccountVerifiedEvent(Guid userId, EmailAddress email, UserFullName name, bool verified)
		{
			AggregateId = userId;
			Name = name;
			Email = email;
			Verified = verified;
		}
	}
}
