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
	public class IsOrderConfirmedToUserId : IRequest<bool>
	{
		public Guid ProductId { get; private set; }
		public Guid CustomerId { get; private set; }
		public IsOrderConfirmedToUserId(Guid orderId, Guid customerId)
		{
			ProductId = orderId;
			CustomerId = customerId;
		}
	}

	public class IsOrderConfirmedToUserIdQueryHandler : IRequestHandler<IsOrderConfirmedToUserId, bool>
	{
		private readonly IOrderRepository<OrderInfo> _repository;

		public IsOrderConfirmedToUserIdQueryHandler(IOrderRepository<OrderInfo> repository)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		public async Task<bool> Handle(IsOrderConfirmedToUserId request, CancellationToken cancellationToken)
		{
			return await _repository.IsOrderConfirmedByUserAsync(request.CustomerId, request.ProductId);
		}
	}
}
