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
	public class GetUserProductLikeCountByProductId : IRequest<int>
	{
		public Guid ProductId;

		public GetUserProductLikeCountByProductId(Guid productId)
		{
			ProductId = productId;
		}
	}

	public class GetUserProductLikeCountByProductIdInfosQueryHandler : IRequestHandler<GetUserProductLikeCountByProductId, int>
	{
		private readonly IUserProductLikeRepository<UserProductLikesInfo> _repository;

		public GetUserProductLikeCountByProductIdInfosQueryHandler(IUserProductLikeRepository<UserProductLikesInfo> repository)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		public async Task<int> Handle(GetUserProductLikeCountByProductId request, CancellationToken cancellationToken)
		{
			return await _repository.CountAsync(p => p.ProductId == request.ProductId);
		}
	}
}
