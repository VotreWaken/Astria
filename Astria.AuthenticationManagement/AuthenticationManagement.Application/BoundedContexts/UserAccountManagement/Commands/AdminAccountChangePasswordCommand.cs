using AuthenticationManagement.Application.BoundedContexts.UserAccountManagement.QueryObjects;
using AuthenticationManagement.Application.Results;
using AuthenticationManagement.Authentication.Repository;
using AuthenticationManagement.Domain.Exceptions;
using AuthenticationManagement.Domain.BoundedContexts.UserAccountManagement.Aggregates;
using Astria.EventSourcingRepository.Repository;
using Astria.QueryRepository.Repository;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AuthenticationManagement.Application.BoundedContexts.UserAccountManagement.Commands
{
	public class AdminAccountChangePasswordCommand : IRequest<CommandResult>
	{
		public long ExpectedVersion { get; set; }
		public Guid UserId { get; set; }
		public string CurrentPassword { get; set; }
		public string NewPassword { get; set; }
	}

	public class AdminAccountChangePasswordCommandHandler : IRequestHandler<AdminAccountChangePasswordCommand, CommandResult>
	{
		private readonly IProjectionRepository<AdminInfo> _repository;
		private readonly IAuthenticationRepository _authRepository;

		public AdminAccountChangePasswordCommandHandler(IProjectionRepository<AdminInfo> repository, IAuthenticationRepository authRepository)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
			_authRepository = authRepository ?? throw new ArgumentNullException(nameof(authRepository));
		}

		public async Task<CommandResult> Handle(AdminAccountChangePasswordCommand command, CancellationToken cancellationToken)
		{
			try
			{
				var admin = await _repository.FindByIdAsync(command.UserId);
				if (admin is null)
					return CommandResult.NotFound(command.UserId);

				var passwordHasher = new PasswordHasher<AdminInfo>();

				var verificationResult = passwordHasher.VerifyHashedPassword(admin, admin.HashedPassword, command.CurrentPassword);

				/*
                if (admin.Version != command.ExpectedVersion)
                    return CommandResult.NotFound(command.UserId);
                */
				Console.WriteLine("Admin Hashed Password: " + admin.HashedPassword);
				Console.WriteLine("Verification Result: " + verificationResult.ToString());
				if (verificationResult != PasswordVerificationResult.Success)
					return CommandResult.BusinessFail("Invalid Password.");

				var newHashedPassword = passwordHasher.HashPassword(admin, command.NewPassword);
				await _authRepository.ChangePassword(command.UserId, newHashedPassword);


				admin.HashedPassword = newHashedPassword;
				await _repository.UpdateAsync(admin);
				return CommandResult.Success();
			}
			catch (DomainException ex)
			{
				return CommandResult.BusinessFail(ex.Message);
			}
		}
	}
}
