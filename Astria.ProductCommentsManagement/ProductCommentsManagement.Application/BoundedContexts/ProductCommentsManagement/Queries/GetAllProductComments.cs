using Astria.QueryRepository.Repository;
using MediatR;
using ProductCommentsManagement.Application.BoundedContexts.ProductCommentsManagement.QueryObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCommentsManagement.Application.BoundedContexts.ProductCommentsManagement.Queries
{
	public class GetAllProductComments : IRequest<IEnumerable<ProductCommentsInfo>>
	{
		public Guid ProductId;

		public GetAllProductComments(Guid productId)
		{
			ProductId = productId;
		}
	}

	public class GetAllProductCommentsInfosQueryHandler : IRequestHandler<GetAllProductComments, IEnumerable<ProductCommentsInfo>>
	{
		private readonly IQueryRepository<ProductCommentsInfo> _repository;

		public GetAllProductCommentsInfosQueryHandler(IQueryRepository<ProductCommentsInfo> repository)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		public async Task<IEnumerable<ProductCommentsInfo>> Handle(GetAllProductComments request, CancellationToken cancellationToken)
		{
			return await _repository.FindAllAsync(p => p.ProductId == request.ProductId);
		}
	}
}
