using AuthenticationManagement.Authentication.Repository;
using AuthenticationManagement.Domain.BoundedContexts.UserAccountManagement.Events;
using Celestial.EmailService.Services;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AuthenticationManagement.Application.BoundedContexts.UserAccountManagement.EventHandlers
{
	public class UserAccountSendEmailVerificationToken :
		INotificationHandler<AdminAccountCreatedEvent>,
		INotificationHandler<CustomerAccountCreatedEvent>,
		INotificationHandler<AdminAccountChangedEmailEvent>,
		INotificationHandler<CustomerAccountChangedEmailEvent>
	{
		private readonly IAuthenticationRepository _authRepository;
		private readonly IEmailService _emailService;

		public UserAccountSendEmailVerificationToken(IAuthenticationRepository authRepository, IEmailService emailService)
		{
			_authRepository = authRepository ?? throw new ArgumentNullException(nameof(authRepository));
			_emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
		}

		public async Task Handle(AdminAccountCreatedEvent @event, CancellationToken cancellationToken)
		{
			await _emailService.SendEmailAddressConfirmationLink(
				userId: @event.AggregateId,
				userEmail: @event.Email,
				userName: @event.Name.ToString(),
				token: await _authRepository.GenerateEmailValidationToken(@event.AggregateId)
			);
		}

		public async Task Handle(CustomerAccountCreatedEvent @event, CancellationToken cancellationToken)
		{
			await _emailService.SendEmailAddressConfirmationLink(
				userId: @event.AggregateId,
				userEmail: @event.Email,
				userName: @event.Name.ToString(),
				token: await _authRepository.GenerateEmailValidationToken(@event.AggregateId)
			);
		}

		public async Task Handle(AdminAccountChangedEmailEvent @event, CancellationToken cancellationToken)
		{
			await _emailService.SendEmailAddressConfirmationLink(
				userId: @event.AggregateId,
				userEmail: @event.Email,
				userName: @event.Name.ToString(),
				token: await _authRepository.GenerateEmailValidationToken(@event.AggregateId)
			);
		}

		public async Task Handle(CustomerAccountChangedEmailEvent @event, CancellationToken cancellationToken)
		{
			await _emailService.SendEmailAddressConfirmationLink(
				userId: @event.AggregateId,
				userEmail: @event.Email,
				userName: @event.Name.ToString(),
				token: await _authRepository.GenerateEmailValidationToken(@event.AggregateId)
			);
		}
	}
}
