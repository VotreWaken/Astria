using Astria.EventSourcingRepository.Repository;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using AuthenticationManagement.Domain.Exceptions;
using AuthenticationManagement.Domain.BoundedContexts.UserAccountManagement.Aggregates;
using AuthenticationManagement.Application.Results;
using AuthenticationManagement.Authentication.Repository;

namespace AuthenticationManagement.Application.BoundedContexts.UserAccountManagement.Commands
{
	public class CustomerAccountCreateCommand : IRequest<CommandResult>
	{
		public string Email { get; set; }
		public string PlainPassword { get; set; }
		public string Firstname { get; set; }
		public string Lastname { get; set; }
		public Guid UserImageId { get; set; }
	}

	public class CustomerAccountCreateCommandHandler : IRequestHandler<CustomerAccountCreateCommand, CommandResult>
	{
		private readonly IEventSourcingRepository<Customer> _repository;
		private readonly IAuthenticationRepository _authRepository;
		public CustomerAccountCreateCommandHandler(IEventSourcingRepository<Customer> repository, IAuthenticationRepository authRepository)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
			_authRepository = authRepository ?? throw new ArgumentNullException(nameof(authRepository));
		}

		public async Task<CommandResult> Handle(CustomerAccountCreateCommand command, CancellationToken cancellationToken)
		{
			if (await _authRepository.EmailAddressInUse(command.Email))
				return CommandResult.EmailInUse(command.Email);

			var hashedPassword = await _authRepository.GenerateHashedPassword(Guid.Empty, command.PlainPassword);


			try
			{
				var customer = new Customer(
					userId: Guid.NewGuid(),
					email: command.Email,
					hashedPassword: hashedPassword,
					firstname: command.Firstname,
					lastname: command.Lastname,
					userImageId: command.UserImageId
				);
				await _authRepository.CreateCustomer(customer.AggregateId, customer.Email, customer.Password, customer.UserImageId);
				await _repository.SaveAsync(customer);

				return CommandResult.Success(customer.AggregateId);
			}
			catch (DomainException ex)
			{
				return CommandResult.BusinessFail(ex.Message);
			}
		}
	}
}
