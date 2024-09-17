using Astria.QueryRepository.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserFollowsManagement.Application.BoundedContexts.UserFollowsManagement.QueryObjects;

namespace UserFollowsManagement.Application.BoundedContexts.UserFollowsManagement.Queries
{
	public class GetUserAlreadyFollows : IRequest<bool>
	{
		public Guid ProductId;
		public Guid UserId;

		public GetUserAlreadyFollows(Guid productId, Guid userId)
		{
			ProductId = productId;
			UserId = userId;
		}
	}

	public class GetUserAlreadyFollowsInfosQueryHandler : IRequestHandler<GetUserAlreadyFollows, bool>
	{
		private readonly IUserFollowRepository<UserFollowInfo> _repository;

		public GetUserAlreadyFollowsInfosQueryHandler(IUserFollowRepository<UserFollowInfo> repository)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		public async Task<bool> Handle(GetUserAlreadyFollows request, CancellationToken cancellationToken)
		{
			return await _repository.IsUserAlreadyFollowsAsync(request.UserId, request.ProductId);
		}
	}
}
