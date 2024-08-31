using AuthenticationManagement.Application.Results;
using AuthenticationManagement.Authentication.Repository;
using AuthenticationManagement.Domain.BoundedContexts.UserAccountManagement.Aggregates;
using AuthenticationManagement.Domain.Exceptions;
using Astria.EventSourcingRepository.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationManagement.Application.BoundedContexts.UserAccountManagement.Commands
{
	public class CustomerAccountDeleteCommand : IRequest<CommandResult>
	{
		public Guid UserId { get; set; }
		public string CurrentPassword { get; set; }
	}


	public class CustomerAccountDeleteCommandHandler : IRequestHandler<CustomerAccountDeleteCommand, CommandResult>
	{
		private readonly IEventSourcingRepository<Customer> _repository;
		private readonly IAuthenticationRepository _authRepository;

		public CustomerAccountDeleteCommandHandler(IEventSourcingRepository<Customer> repository, IAuthenticationRepository authRepository)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
			_authRepository = authRepository ?? throw new ArgumentNullException(nameof(authRepository));
		}

		public async Task<CommandResult> Handle(CustomerAccountDeleteCommand command, CancellationToken cancellationToken)
		{
			try
			{
				var admin = await _repository.FindByIdAsync(command.UserId);
				if (admin is null)
					return CommandResult.NotFound(command.UserId);

				Console.WriteLine("Current Password: " + command.CurrentPassword);
				Console.WriteLine("User Id: " + command.UserId);
				Console.WriteLine("Current Admin Password" + admin.Password);
				if (admin.Password != _authRepository.GenerateHashedPassword(command.UserId, command.CurrentPassword).Result)
					return CommandResult.BusinessFail("Invalid Password.");

				await _authRepository.DeleteUser(command.UserId);

				await _repository.SaveAsync(admin);
				return CommandResult.Success();
			}
			catch (DomainException ex)
			{
				return CommandResult.BusinessFail(ex.Message);
			}
		}
	}
}
