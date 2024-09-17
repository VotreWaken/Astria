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
	public class GetFollowedCountByUserId : IRequest<int>
	{
		public Guid FollowedId;

		public GetFollowedCountByUserId(Guid followerId)
		{
			FollowedId = followerId;
		}
	}

	public class GetFollowedCountByUserIdInfosQueryHandler : IRequestHandler<GetFollowedCountByUserId, int>
	{
		private readonly IUserFollowRepository<UserFollowInfo> _repository;

		public GetFollowedCountByUserIdInfosQueryHandler(IUserFollowRepository<UserFollowInfo> repository)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		public async Task<int> Handle(GetFollowedCountByUserId request, CancellationToken cancellationToken)
		{
			return await _repository.CountAsync(p => p.FollowedId == request.FollowedId);
		}
	}
}
