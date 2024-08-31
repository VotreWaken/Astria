using AuthenticationManagement.Application.BoundedContexts.UserAccountManagement.QueryObjects;
using AuthenticationManagement.Application.Results;
using AuthenticationManagement.Authentication.Repository;
using AuthenticationManagement.Domain.BoundedContexts.UserAccountManagement.Aggregates;
using AuthenticationManagement.Domain.Exceptions;
using Astria.EventSourcingRepository.Repository;
using Astria.QueryRepository.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationManagement.Application.BoundedContexts.UserAccountManagement.Commands
{
	public class AdminAccountUpdateImageCommand : IRequest<CommandResult>
	{
		public Guid UserId { get; set; }
		public Guid UserImageId { get; set; }
	}


	public class AdminAccountUpdateImageCommandHandler : IRequestHandler<AdminAccountUpdateImageCommand, CommandResult>
	{
		private readonly IEventSourcingRepository<Admin> _repository;
		private readonly IAuthenticationRepository _authRepository;
		private readonly IProjectionRepository<AdminInfo> _projectionRepository;
		public AdminAccountUpdateImageCommandHandler(IEventSourcingRepository<Admin> repository, IAuthenticationRepository authRepository, IProjectionRepository<AdminInfo> projectionRepository)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
			_authRepository = authRepository ?? throw new ArgumentNullException(nameof(authRepository));
			_projectionRepository = projectionRepository ?? throw new ArgumentNullException(nameof(projectionRepository));
		}

		public async Task<CommandResult> Handle(AdminAccountUpdateImageCommand command, CancellationToken cancellationToken)
		{
			try
			{
				var admin = await _projectionRepository.FindByIdAsync(command.UserId);
				if (admin is null)
					return CommandResult.NotFound(command.UserId);

				command.UserImageId = admin.AdminImageId;

				return CommandResult.Success(command.UserImageId);
			}
			catch (DomainException ex)
			{
				return CommandResult.BusinessFail(ex.Message);
			}
		}
	}
}
