using Astria.QueryRepository.Repository;
using MediatR;
using ProductManagement.Application.BoundedContexts.QueryObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Application.BoundedContexts.Queries
{
	public class GetAllProductsInfosQuery : IRequest<List<ProductInfo>>
	{ }

	public class GetAllCustomerInfosQueryHandler : IRequestHandler<GetAllProductsInfosQuery, List<ProductInfo>>
	{
		private readonly IQueryRepository<ProductInfo> _repository;

		public GetAllCustomerInfosQueryHandler(IQueryRepository<ProductInfo> repository)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		public async Task<List<ProductInfo>> Handle(GetAllProductsInfosQuery request, CancellationToken cancellationToken)
		{
			var customers = await _repository.FindAllAsync();
			return customers?.ToList();
		}
	}
}
