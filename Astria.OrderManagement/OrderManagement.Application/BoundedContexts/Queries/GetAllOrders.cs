using Astria.QueryRepository.Repository;
using MediatR;
using OrderManagement.Application.BoundedContexts.QueryObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.BoundedContexts.Queries
{
	public class GetAllOrders : IRequest<List<OrderInfo>>
	{ }

	public class GetAllOrdersInfosQueryHandler : IRequestHandler<GetAllOrders, List<OrderInfo>>
	{
		private readonly IQueryRepository<OrderInfo> _repository;

		public GetAllOrdersInfosQueryHandler(IQueryRepository<OrderInfo> repository)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		public async Task<List<OrderInfo>> Handle(GetAllOrders request, CancellationToken cancellationToken)
		{
			var customers = await _repository.FindAllAsync();
			return customers?.ToList();
		}
	}
}
