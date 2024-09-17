using Astria.QueryRepository.Repository;
using MediatR;
using OrderManagement.Application.BoundedContexts.QueryObjects;
using OrderManagement.Domain.BoundedContexts.OrderManagment.Events;
using OrderManagement.Domain.BoundedContexts.OrderManagment.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.BoundedContexts.Projections
{
	internal class OrderConfirmedProjection : INotificationHandler<OrderChangedStatusEvent>
	{
		private readonly IProjectionRepository<OrderInfo> _repository;

		public OrderConfirmedProjection(IProjectionRepository<OrderInfo> repository)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		public async Task Handle(OrderChangedStatusEvent @event, CancellationToken cancellationToken)
		{
			Console.WriteLine("Projection For Update");
			var order = await _repository.FindByIdAsync(@event.AggregateId);

			if (order != null)
			{
				order.Status = @event.Status;
				await _repository.UpdateAsync(order);
			}

			await _repository.UpdateAsync(order);
		}
	}
}
