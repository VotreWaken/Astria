using Astria.QueryRepository.Repository;
using MediatR;
using ProductCommentsManagement.Application.BoundedContexts.ProductCommentsManagement.QueryObjects;

namespace ProductCommentsManagement.Application.BoundedContexts.ProductCommentsManagement.Queries
{
	public class GetCommentsCountByProductId : IRequest<int>
	{
		public Guid ProductId;

		public GetCommentsCountByProductId(Guid productId)
		{
			ProductId = productId;
		}
	}

	public class GetCommentsCountByProductIdInfosQueryHandler : IRequestHandler<GetCommentsCountByProductId, int>
	{
		private readonly IProductCommentsRepository<ProductCommentsInfo> _repository;

		public GetCommentsCountByProductIdInfosQueryHandler(IProductCommentsRepository<ProductCommentsInfo> repository)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		public async Task<int> Handle(GetCommentsCountByProductId request, CancellationToken cancellationToken)
		{
			return await _repository.CountAsync(p => p.ProductId == request.ProductId);
		}
	}
}
