using Astria.QueryRepository.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserProductLikesManagement.Application.BoundedContexts.UserProductLikesManagement.QueryObjects;
using UserProductLikesManagement.Domain.BoundedContexts.UserProductLikesManagement.Events;

namespace UserProductLikesManagement.Application.BoundedContexts.UserProductLikesManagement.Projections
{
	public class UserProductLikesCreatedProjection : INotificationHandler<UserLikeProductCreatedEvent>
	{
		private readonly IProjectionRepository<UserProductLikesInfo> _repository;

		public UserProductLikesCreatedProjection(IProjectionRepository<UserProductLikesInfo> repository)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		public async Task Handle(UserLikeProductCreatedEvent @event, CancellationToken cancellationToken)
		{
			var existingLikes = await _repository.FindAllAsync(
				x => x.UserId == @event.UserId && x.ProductId == @event.ProductId
			);

			var existingLike = existingLikes.FirstOrDefault();

			if (existingLike != null)
			{
				await _repository.DeleteAsync(
					existingLike.Id
				);
				Console.WriteLine("Like removed for ProductId: " + @event.ProductId + " and UserId: " + @event.UserId);
			}
			else
			{
				var likeInfo = new UserProductLikesInfo
				{
					UserId = @event.UserId,
					ProductId = @event.ProductId,
				};
				await _repository.InsertAsync(likeInfo);
				Console.WriteLine("Like added for ProductId: " + @event.ProductId + " and UserId: " + @event.UserId);
			}
		}
	}
}
