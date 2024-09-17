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
	public class GetAllUserFollowers : IRequest<IEnumerable<UserFollowInfo>>
	{
		public Guid UserId;

		public GetAllUserFollowers(Guid userId)
		{
			UserId = userId;
		}
	}

	public class GetAllUserFollowersInfosQueryHandler : IRequestHandler<GetAllUserFollowers, IEnumerable<UserFollowInfo>>
	{
		private readonly IQueryRepository<UserFollowInfo> _repository;

		public GetAllUserFollowersInfosQueryHandler(IQueryRepository<UserFollowInfo> repository)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		public async Task<IEnumerable<UserFollowInfo>> Handle(GetAllUserFollowers request, CancellationToken cancellationToken)
		{
			return await _repository.FindAllAsync(p => p.FollowedId == request.UserId);
		}
	}
}
