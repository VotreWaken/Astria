using Astria.QueryRepository.Repository;
using MediatR;
using ProductManagement.Application.BoundedContexts.QueryObjects;

namespace ProductManagement.Application.BoundedContexts.Queries
{
	public class GetMostLikedProductInfosQuery : IRequest<List<ProductInfo>>
	{ }

	public class GetMostLikedProductInfosQueryHandler : IRequestHandler<GetMostLikedProductInfosQuery, List<ProductInfo>>
	{
		private readonly IQueryRepository<ProductInfo> _repository;

		public GetMostLikedProductInfosQueryHandler(IQueryRepository<ProductInfo> repository)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		public async Task<List<ProductInfo>> Handle(GetMostLikedProductInfosQuery request, CancellationToken cancellationToken)
		{
			var customers = await _repository.FindAllAsync();
			return customers?.ToList();
		}
	}
}
