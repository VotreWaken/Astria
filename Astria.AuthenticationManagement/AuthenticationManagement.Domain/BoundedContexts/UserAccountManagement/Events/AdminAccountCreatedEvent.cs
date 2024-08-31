using AuthenticationManagement.Domain.BoundedContexts.UserAccountManagement.ValueObjects;
using Astria.SharedKernel;
using System;

namespace AuthenticationManagement.Domain.BoundedContexts.UserAccountManagement.Events
{
	public class AdminAccountCreatedEvent : DomainEvent
	{
		public EmailAddress Email { get; private set; }
		public HashedPassword Password { get; private set; }
		public UserFullName Name { get; private set; }
		public Guid UserImageId { get; private set; }

		private AdminAccountCreatedEvent()
		{ }

		public AdminAccountCreatedEvent(Guid userId, EmailAddress email, HashedPassword password, UserFullName name, Guid userImageId)
		{
			AggregateId = userId;
			Email = email;
			Password = password;
			Name = name;
			UserImageId = userImageId;
		}
	}
}
