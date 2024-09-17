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
	public class GetFollowerCountByUserId : IRequest<int>
	{
		public Guid FollowerId;

		public GetFollowerCountByUserId(Guid followerId)
		{
			FollowerId = followerId;
		}
	}

	public class GetFollowerCountByUserIdInfosQueryHandler : IRequestHandler<GetFollowerCountByUserId, int>
	{
		private readonly IUserFollowRepository<UserFollowInfo> _repository;

		public GetFollowerCountByUserIdInfosQueryHandler(IUserFollowRepository<UserFollowInfo> repository)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		public async Task<int> Handle(GetFollowerCountByUserId request, CancellationToken cancellationToken)
		{
			return await _repository.CountAsync(p => p.FollowerId == request.FollowerId);
		}
	}
}
