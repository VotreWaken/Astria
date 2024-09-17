using Astria.QueryRepository.Repository;
using MediatR;
using ProductManagement.Application.BoundedContexts.QueryObjects;

namespace ProductManagement.Application.BoundedContexts.Queries
{
	public class GetProductsByPriceQuery : IRequest<IEnumerable<ProductInfo>>
	{
		public decimal Price { get; private set; }

		public GetProductsByPriceQuery(decimal price)
		{
			Price = price;
		}
	}

	public class GetProductsByPriceQueryHandler : IRequestHandler<GetProductsByPriceQuery, IEnumerable<ProductInfo>>
	{
		private readonly IQueryRepository<ProductInfo> _repository;

		public GetProductsByPriceQueryHandler(IQueryRepository<ProductInfo> repository)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		public async Task<IEnumerable<ProductInfo>> Handle(GetProductsByPriceQuery request, CancellationToken cancellationToken)
		{
			return await _repository.FindAllAsync(p => p.Price > request.Price);
		}
	}
}
