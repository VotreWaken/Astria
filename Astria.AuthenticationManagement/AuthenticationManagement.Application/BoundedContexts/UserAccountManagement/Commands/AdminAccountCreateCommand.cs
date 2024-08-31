using Astria.EventSourcingRepository.Repository;
using MediatR;
using Astria.QueryRepository.Repository;
using AuthenticationManagement.Domain.Exceptions;
using AuthenticationManagement.Domain.BoundedContexts.UserAccountManagement.Aggregates;
using AuthenticationManagement.Application.BoundedContexts.UserAccountManagement.QueryObjects;
using AuthenticationManagement.Application.Results;
using AuthenticationManagement.Authentication.Repository;

namespace AuthenticationManagement.Application.BoundedContexts.UserAccountManagement.Commands
{
	public class AdminAccountCreateCommand : IRequest<CommandResult>
	{
		public string Email { get; set; }
		public string PlainPassword { get; set; }
		public string Firstname { get; set; }
		public string Lastname { get; set; }
		public string SecretProductKey { get; set; }
		public Guid UserImageId { get; set; }
	}

	public class AdminAccountCreateCommandHandler : IRequestHandler<AdminAccountCreateCommand, CommandResult>
	{
		private readonly IEventSourcingRepository<Admin> _repository;
		private readonly IAuthenticationRepository _authRepository;
		private readonly IQueryRepository<AdminInfo> _queryRepository;
		public AdminAccountCreateCommandHandler(IEventSourcingRepository<Admin> repository, IAuthenticationRepository authRepository, IQueryRepository<AdminInfo> queryRepository)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
			_authRepository = authRepository ?? throw new ArgumentNullException(nameof(authRepository));
			_queryRepository = queryRepository ?? throw new ArgumentNullException(nameof(queryRepository));
		}

		public async Task<CommandResult> Handle(AdminAccountCreateCommand command, CancellationToken cancellationToken)
		{
			if (await _authRepository.EmailAddressInUse(command.Email))
				return CommandResult.EmailInUse(command.Email);

			var hashedPassword = await _authRepository.GenerateHashedPassword(Guid.Empty, command.PlainPassword);

			try
			{
				var admin = new Admin(
					userId: Guid.NewGuid(),
					email: command.Email,
					hashedPassword: hashedPassword,
					firstname: command.Firstname,
					lastname: command.Lastname,
					userImageId: command.UserImageId
				);

				await _authRepository.CreateAdmin(admin.AggregateId, admin.Email, hashedPassword, admin.UserImageId);
				await _repository.SaveAsync(admin);


				return CommandResult.Success(admin.AggregateId);
			}
			catch (DomainException ex)
			{
				return CommandResult.BusinessFail(ex.Message);
			}
		}
	}
}
