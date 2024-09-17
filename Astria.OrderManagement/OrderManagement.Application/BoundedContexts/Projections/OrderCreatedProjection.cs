using Astria.QueryRepository.Repository;
using MediatR;
using OrderManagement.Application.BoundedContexts.QueryObjects;
using OrderManagement.Domain.BoundedContexts.OrderManagment.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.BoundedContexts.Projections
{
	public class OrderCreatedProjection : INotificationHandler<OrderCreatedEvent>
	{
		private readonly IProjectionRepository<OrderInfo> _repository;

		public OrderCreatedProjection(IProjectionRepository<OrderInfo> repository)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		public async Task Handle(OrderCreatedEvent @event, CancellationToken cancellationToken)
		{
			var order = new OrderInfo
			{
				Id = @event.AggregateId,
				ProductId = @event.ProductId,
				CustomerId = @event.CustomerId,
				Status = @event.Status,
				OrderAmount = @event.OrderAmount,
				OrderDate = @event.OrderDate,
			};

			await _repository.InsertAsync(order);
		}
	}
}
