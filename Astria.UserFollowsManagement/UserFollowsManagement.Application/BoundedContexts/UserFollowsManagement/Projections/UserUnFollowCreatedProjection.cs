using Astria.QueryRepository.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserFollowsManagement.Application.BoundedContexts.UserFollowsManagement.QueryObjects;
using UserFollowsManagement.Domain.BoundedContexts.UserFollowsManagement.Events;

namespace UserFollowsManagement.Application.BoundedContexts.UserFollowsManagement.Projections
{
	public class UserUnFollowCreatedProjection : INotificationHandler<UserUnFollowsCreatedEvent>
	{
		private readonly IProjectionRepository<UserFollowInfo> _repository;

		public UserUnFollowCreatedProjection(IProjectionRepository<UserFollowInfo> repository)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		public async Task Handle(UserUnFollowsCreatedEvent @event, CancellationToken cancellationToken)
		{
			var order = new UserFollowInfo
			{
				FollowerId = @event.FollowerId,
				FollowedId = @event.FollowedId,
			};
			
			Console.WriteLine("Projection for ProductId And UserId: " + order.FollowerId + order.FollowedId);
			var followRecords = await _repository.FindAllAsync(f => f.FollowerId == @event.FollowerId && f.FollowedId == @event.FollowedId);
			foreach (var record in followRecords)
			{
				await _repository.DeleteAsync(record.Id);
			}
		}
	}
}
