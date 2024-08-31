using AuthenticationManagement.Domain.BoundedContexts.UserAccountManagement.Events;
using AuthenticationManagement.Domain.BoundedContexts.UserAccountManagement.ValueObjects;
using AuthenticationManagement.Domain.Exceptions;
using Astria.SharedKernel;

namespace AuthenticationManagement.Domain.BoundedContexts.UserAccountManagement.Aggregates
{
	public class Admin : AggregateRoot
	{
		public UserFullName Name { get; private set; }
		public EmailAddress Email { get; private set; }
		public HashedPassword Password { get; private set; }
		public bool Verified { get; private set; }
		public Guid UserImageId { get; private set; }

		public Admin()
		{ }

		public Admin(Guid userId, string email, string hashedPassword, string firstname, string lastname, Guid userImageId)
		{
			if (userId == Guid.Empty)
				throw new ArgumentNullException(nameof(userId));

			AggregateId = userId;

			Email = (EmailAddress)CheckAndAssign(EmailAddress.Create(email));
			Password = (HashedPassword)CheckAndAssign(HashedPassword.Create(hashedPassword));
			Name = (UserFullName)CheckAndAssign(UserFullName.Create(firstname, lastname));
			UserImageId = userImageId;

			Verified = false;

			if (_businessLogicErrors?.Any() == true)
				throw new DomainBusinessLogicException(_businessLogicErrors);

			RaiseEvent(new AdminAccountCreatedEvent(
				userId: AggregateId,
				email: Email,
				password: Password,
				name: Name,
				userImageId: UserImageId
			));
		}

		#region Aggregate Methods
		public void ChangeEmail(string email)
		{
			Console.WriteLine($"Changing current email {Email} to {email} for UserId: {AggregateId}");
			Email = (EmailAddress)CheckAndAssign(EmailAddress.Create(email));
			Verified = false;

			if (_businessLogicErrors?.Any() == true)
				throw new DomainBusinessLogicException(_businessLogicErrors);

			RaiseEvent(new AdminAccountChangedEmailEvent(
				userId: AggregateId,
				email: Email,
				fullName: Name
			));
		}

		public void ChangePassword(string currentPassword, string newPassword)
		{
			var validatedNewPassword = (HashedPassword)CheckAndAssign(HashedPassword.Create(newPassword));
			var validatedCurrentPassword = (HashedPassword)CheckAndAssign(HashedPassword.Create(currentPassword));
			// Тут жесть была скорее всего дебажить нужно будет опять 

			if (Password.EqualsCurrentPassword(validatedCurrentPassword))
				Password = validatedNewPassword;

			if (_businessLogicErrors?.Any() == true)
				throw new DomainBusinessLogicException(_businessLogicErrors);

			RaiseEvent(new AdminAccountChangedPasswordEvent(
				userId: AggregateId,
				newPassword: validatedNewPassword
			));
		}

		public void VerifyAccount()
		{
			Verified = true;

			if (_businessLogicErrors?.Any() == true)
				throw new DomainBusinessLogicException(_businessLogicErrors);

			RaiseEvent(new AdminAccountVerifiedEvent(
				userId: AggregateId,
				email: Email,
				name: Name,
				verified: Verified
			));
		}
		#endregion

		#region Event Handling
		protected override void When(IDomainEvent @event)
		{
			switch (@event)
			{
				case AdminAccountCreatedEvent x: OnAccountCreatedChanged(x); break;
				case AdminAccountVerifiedEvent x: OnAccountVerificationChanged(x); break;
				case AdminAccountChangedEmailEvent x: OnEmailChanged(x); break;
				case AdminAccountChangedPasswordEvent x: OnPasswordChanged(x); break;
			}
		}

		private void OnAccountCreatedChanged(AdminAccountCreatedEvent @event)
		{
			AggregateId = @event.AggregateId;
			Email = @event.Email;
			Password = @event.Password;
			Name = @event.Name;
			UserImageId = @event.UserImageId;
		}

		private void OnAccountVerificationChanged(AdminAccountVerifiedEvent @event)
		{
			AggregateId = @event.AggregateId;
			Email = @event.Email;
			Verified = @event.Verified;
			Name = @event.Name;
		}

		private void OnEmailChanged(AdminAccountChangedEmailEvent @event)
		{
			Console.WriteLine("Succsessfully change email to " + @event.Email);
			AggregateId = @event.AggregateId;
			Email = @event.Email;
			Name = @event.Name;
		}

		private void OnPasswordChanged(AdminAccountChangedPasswordEvent @event)
		{
			AggregateId = @event.AggregateId;
			Password = @event.NewPassword;
		}
		#endregion
	}
}
