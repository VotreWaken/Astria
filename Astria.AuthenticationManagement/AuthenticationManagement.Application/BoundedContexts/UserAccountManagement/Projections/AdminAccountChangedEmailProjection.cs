using AuthenticationManagement.Application.BoundedContexts.UserAccountManagement.QueryObjects;
using AuthenticationManagement.Domain.BoundedContexts.UserAccountManagement.Events;
using Astria.QueryRepository.Repository;
using MediatR;

namespace AuthenticationManagement.Application.BoundedContexts.UserAccountManagement.Projections
{
	public class AdminAccountChangedEmailProjection : INotificationHandler<AdminAccountChangedEmailEvent>
	{
		private readonly IProjectionRepository<AdminInfo> _repository;

		public AdminAccountChangedEmailProjection(IProjectionRepository<AdminInfo> repository)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		public async Task Handle(AdminAccountChangedEmailEvent @event, CancellationToken cancellationToken)
		{
			var admin = await _repository.FindByIdAsync(@event.AggregateId);
			if (admin is not null)
			{
				admin.Version = @event.AggregateVersion;
				// admin.Email = @event.Email;
				await _repository.UpdateAsync(admin);
			}
			else
			{

			}
		}
	}
}
