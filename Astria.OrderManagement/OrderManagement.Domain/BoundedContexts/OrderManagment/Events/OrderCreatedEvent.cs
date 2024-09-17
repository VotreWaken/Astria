using Astria.SharedKernel;
using OrderManagement.Domain.BoundedContexts.OrderManagment.ValueObjects;

namespace OrderManagement.Domain.BoundedContexts.OrderManagment.Events
{
	public class OrderCreatedEvent : DomainEvent
	{
		public Guid AggregateId { get; }
		public Guid CustomerId { get; }
		public Guid ProductId { get; }
		public DateTime OrderDate { get; }
		public decimal OrderAmount { get; }
		public OrderStatus Status { get; }

		public OrderCreatedEvent(Guid aggregateId, Guid customerId, Guid productId, DateTime orderDate, decimal orderAmount, OrderStatus status)
		{
			AggregateId = aggregateId;
			CustomerId = customerId;
			ProductId = productId;
			OrderDate = orderDate;
			OrderAmount = orderAmount;
			Status = status;
		}
	}
}
