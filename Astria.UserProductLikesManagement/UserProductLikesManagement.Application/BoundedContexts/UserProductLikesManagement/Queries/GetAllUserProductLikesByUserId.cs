using Astria.QueryRepository.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserProductLikesManagement.Application.BoundedContexts.UserProductLikesManagement.QueryObjects;

namespace UserProductLikesManagement.Application.BoundedContexts.UserProductLikesManagement.Queries
{
	public class GetAllUserProductLikesByUserId : IRequest<IEnumerable<UserProductLikesInfo>>
	{
		public Guid UserId;

		public GetAllUserProductLikesByUserId(Guid userId)
		{
			UserId = userId;
		}
	}

	public class GetMostLikedProductInfosQueryHandler : IRequestHandler<GetAllUserProductLikesByUserId, IEnumerable<UserProductLikesInfo>>
	{
		private readonly IQueryRepository<UserProductLikesInfo> _repository;

		public GetMostLikedProductInfosQueryHandler(IQueryRepository<UserProductLikesInfo> repository)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		public async Task<IEnumerable<UserProductLikesInfo>> Handle(GetAllUserProductLikesByUserId request, CancellationToken cancellationToken)
		{
			return await _repository.FindAllAsync(p => p.UserId == request.UserId);
		}
	}
}
