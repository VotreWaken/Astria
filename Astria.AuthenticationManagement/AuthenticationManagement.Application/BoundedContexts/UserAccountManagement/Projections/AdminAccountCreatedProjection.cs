using AuthenticationManagement.Application.BoundedContexts.UserAccountManagement.QueryObjects;
using AuthenticationManagement.Domain.BoundedContexts.UserAccountManagement.Events;
using Astria.QueryRepository.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationManagement.Application.BoundedContexts.UserAccountManagement.Projections
{
	public class AdminAccountCreatedProjection : INotificationHandler<AdminAccountCreatedEvent>
	{
		private readonly IProjectionRepository<AdminInfo> _repository;

		public AdminAccountCreatedProjection(IProjectionRepository<AdminInfo> repository)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		public async Task Handle(AdminAccountCreatedEvent @event, CancellationToken cancellationToken)
		{
			var admin = new AdminInfo
			{
				HashedPassword = @event.Password,
				AdminImageId = @event.UserImageId,
				Id = @event.AggregateId,
				Email = @event.Email,
				FirstName = @event.Name.First,
				LastName = @event.Name.Last,
			};

			await _repository.InsertAsync(admin);
		}
	}
}
