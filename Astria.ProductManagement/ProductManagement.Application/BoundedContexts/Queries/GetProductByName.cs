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
	public class GetProductsByNameQuery : IRequest<IEnumerable<ProductInfo>>
	{
		public string Name { get; private set; }

		public GetProductsByNameQuery(string name)
		{
			Name = name;
		}
	}

	public class GetProductsByNameQueryHandler : IRequestHandler<GetProductsByNameQuery, IEnumerable<ProductInfo>>
	{
		private readonly IQueryRepository<ProductInfo> _repository;

		public GetProductsByNameQueryHandler(IQueryRepository<ProductInfo> repository)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		public async Task<IEnumerable<ProductInfo>> Handle(GetProductsByNameQuery request, CancellationToken cancellationToken)
		{
			return await _repository.FindAllAsync(p => p.Name.Contains(request.Name));
		}
	}
}
