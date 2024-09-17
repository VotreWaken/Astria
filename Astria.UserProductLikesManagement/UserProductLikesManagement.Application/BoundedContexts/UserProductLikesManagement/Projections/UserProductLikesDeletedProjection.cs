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
	public class UserProductLikesDeletedProjection : INotificationHandler<UserUnlikeProductCreatedEvent>
	{
		private readonly IProjectionRepository<UserProductLikesInfo> _repository;

		public UserProductLikesDeletedProjection(IProjectionRepository<UserProductLikesInfo> repository)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		public async Task Handle(UserUnlikeProductCreatedEvent @event, CancellationToken cancellationToken)
		{
			try
			{
				Console.WriteLine("Delte Projection");
				await _repository.DeleteAsync(@event.UserId);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				throw;
			}

		}
	}
}
