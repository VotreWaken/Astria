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
	public class UserFollowCreatedProjection : INotificationHandler<UserFollowsCreatedEvent>
	{
		private readonly IProjectionRepository<UserFollowInfo> _repository;

		public UserFollowCreatedProjection(IProjectionRepository<UserFollowInfo> repository)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		public async Task Handle(UserFollowsCreatedEvent @event, CancellationToken cancellationToken)
		{
			var existingLikes = await _repository.FindAllAsync(
				x => x.FollowerId == @event.FollowerId && x.FollowedId == @event.FollowedId
			);

			var existingLike = existingLikes.FirstOrDefault();

			if (existingLike != null)
			{
				await _repository.DeleteAsync(
					existingLike.Id
				);
				Console.WriteLine("Follow removed for FollowerId: " + @event.FollowerId + " and FollowedId: " + @event.FollowedId);
			}
			else
			{
				var likeInfo = new UserFollowInfo
				{
					FollowerId = @event.FollowerId,
					FollowedId = @event.FollowedId,
				};
				await _repository.InsertAsync(likeInfo);
				Console.WriteLine("Follow added for FollowerId: " + @event.FollowerId + " and FollowedId: " + @event.FollowedId);
			}
		}
	}
}
