using Astria.QueryRepository.Repository;
using ProductManagement.Application.BoundedContexts.QueryObjects;
using MediatR;

namespace ProductManagement.Application.BoundedContexts.Queries
{
	public class GetProductById : IRequest<ProductInfo>
	{
		public Guid Id { get; private set; }
		public GetProductById(Guid id)
		{
			Id = id;
		}
	}

	public class GetProductByIdQueryHandler : IRequestHandler<GetProductById, ProductInfo>
	{
		private readonly IProductRepository<ProductInfo> _repository;

		public GetProductByIdQueryHandler(IProductRepository<ProductInfo> repository)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		public async Task<ProductInfo> Handle(GetProductById request, CancellationToken cancellationToken)
		{
			await _repository.IncrementFieldAsync(request.Id, "Views");

			return await _repository.FindByIdAsync(request.Id);
		}
	}
}
