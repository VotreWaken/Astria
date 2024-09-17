using Astria.EventSourcingRepository.Repository;
using Astria.QueryRepository.Repository;
using AuthenticationManagement.Application.BoundedContexts.UserAccountManagement.QueryObjects;
using AuthenticationManagement.Application.Results;
using AuthenticationManagement.Authentication.Repository;
using AuthenticationManagement.Domain.BoundedContexts.UserAccountManagement.Aggregates;
using AuthenticationManagement.Domain.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationManagement.Application.BoundedContexts.UserAccountManagement.Commands
{
	public class AdminAccountChangeNameAndSurnameCommand : IRequest<CommandResult>
	{
		public long ExpectedVersion { get; set; }
		public Guid UserId { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
	}

	public class AdminAccountChangeNameAndSurnameCommandHandler : IRequestHandler<AdminAccountChangeNameAndSurnameCommand, CommandResult>
	{
		private readonly IProjectionRepository<AdminInfo> _repository;
		private readonly IAuthenticationRepository _authRepository;
		private readonly IEventSourcingRepository<Admin> _eventRepository;

		public AdminAccountChangeNameAndSurnameCommandHandler(IProjectionRepository<AdminInfo> repository, IAuthenticationRepository authRepository, IEventSourcingRepository<Admin> eventRepository)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
			_authRepository = authRepository ?? throw new ArgumentNullException(nameof(authRepository));
			_eventRepository = eventRepository ?? throw new ArgumentNullException(nameof(eventRepository));
		}

		public async Task<CommandResult> Handle(AdminAccountChangeNameAndSurnameCommand command, CancellationToken cancellationToken)
		{
			// if (await _authRepository.EmailAddressInUse(command.NewEmail))
				// return CommandResult.EmailInUse(command.NewEmail);

			try
			{
				Console.WriteLine("CommandId: " + command.UserId);
				Console.WriteLine("UserFirstName: " + command.FirstName);
				Console.WriteLine("UserLastName: " + command.LastName);

				// var admin = await _repository.FindByIdAsync(command.UserId);
				// if (admin is null)
					// return CommandResult.NotFound(command.UserId);

				var admin = new Admin();

				/*
                if (admin.Version != command.ExpectedVersion)
                    return CommandResult.NotFound(command.UserId);
                */
				// admin.ChangeEmail(command.NewEmail);

				admin.ChangeName(command.UserId, command.FirstName, command.LastName);

				await _authRepository.ChangeUserName(command.UserId, command.FirstName, command.LastName);

				await _eventRepository.SaveAsync(admin);

				return CommandResult.Success();
			}
			catch (DomainException ex)
			{
				return CommandResult.BusinessFail(ex.Message);
			}
		}
	}
}
