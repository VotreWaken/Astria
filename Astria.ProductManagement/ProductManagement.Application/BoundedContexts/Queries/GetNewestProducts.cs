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
	public class GetNewestProductInfosQuery : IRequest<List<ProductInfo>>
	{ }

	public class GetNewestProductInfosQueryHandler : IRequestHandler<GetNewestProductInfosQuery, List<ProductInfo>>
	{
		private readonly IQueryRepository<ProductInfo> _repository;

		public GetNewestProductInfosQueryHandler(IQueryRepository<ProductInfo> repository)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		public async Task<List<ProductInfo>> Handle(GetNewestProductInfosQuery request, CancellationToken cancellationToken)
		{
			var customers = await _repository.FindAllAsync();
			return customers?.ToList();
		}
	}
}
