using AuthenticationManagement.Application.Results;
using AuthenticationManagement.Domain.BoundedContexts.UserAccountManagement.Aggregates;
using AuthenticationManagement.Domain.Exceptions;
using Astria.EventSourcingRepository.Repository;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AuthenticationManagement.Application.BoundedContexts.UserAccountManagement.Commands
{
	public class AdminAccountVerifyCommand : IRequest<CommandResult>
	{
		public Guid UserId { get; set; }
	}

	public class AdminAccountVerifyCommandHandler : IRequestHandler<AdminAccountVerifyCommand, CommandResult>
	{
		private readonly IEventSourcingRepository<Admin> _repository;
		public AdminAccountVerifyCommandHandler(IEventSourcingRepository<Admin> repository)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		public async Task<CommandResult> Handle(AdminAccountVerifyCommand command, CancellationToken cancellationToken)
		{
			try
			{
				var admin = await _repository.FindByIdAsync(command.UserId);
				if (admin is null)
					return CommandResult.NotFound(command.UserId);

				admin.VerifyAccount();

				await _repository.SaveAsync(admin);
				return CommandResult.Success(command.UserId);
			}
			catch (DomainException ex)
			{
				return CommandResult.BusinessFail(ex.Message);
			}
		}
	}
}
