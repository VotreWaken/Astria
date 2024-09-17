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
	public class GetUserProductLikeByProductId : IRequest<IEnumerable<UserProductLikesInfo>>
	{
		public Guid ProductId;

		public GetUserProductLikeByProductId(Guid productId)
		{
			ProductId = productId;
		}
	}

	public class GetUserProductLikeByProductIdInfosQueryHandler : IRequestHandler<GetUserProductLikeByProductId, IEnumerable<UserProductLikesInfo>>
	{
		private readonly IQueryRepository<UserProductLikesInfo> _repository;

		public GetUserProductLikeByProductIdInfosQueryHandler(IQueryRepository<UserProductLikesInfo> repository)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		public async Task<IEnumerable<UserProductLikesInfo>> Handle(GetUserProductLikeByProductId request, CancellationToken cancellationToken)
		{
			return await _repository.FindAllAsync(p => p.ProductId == request.ProductId);
		}
	}
}
