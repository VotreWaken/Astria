using Astria.SharedKernel;
using OrderManagement.Domain.BoundedContexts.OrderManagment.Events;
using OrderManagement.Domain.BoundedContexts.OrderManagment.ValueObjects;


namespace OrderManagement.Domain.BoundedContexts.OrderManagment.Aggregates
{
	public class Order : AggregateRoot
	{
		public Guid CustomerId { get; private set; }
		public Guid ProductId { get; private set; }
		public DateTime OrderDate { get; private set; }
		public decimal OrderAmount { get; private set; }
		public OrderStatus Status { get; private set; }

		// Конструктор по умолчанию
		public Order() { }

		// Конструктор для создания нового заказа
		public Order(Guid orderId, Guid customerId, Guid productId, DateTime orderDate, decimal orderAmount, OrderStatus status)
		{
			if (orderId == Guid.Empty)
				throw new ArgumentNullException(nameof(orderId));

			AggregateId = orderId;
			CustomerId = customerId;
			ProductId = productId;
			OrderDate = orderDate;
			OrderAmount = orderAmount;
			Status = status;

			RaiseEvent(new OrderCreatedEvent(orderId, customerId, productId, orderDate, orderAmount, status));
		}

		#region Aggregate Methods

		public void ChangeOrderStatus(Guid id, OrderStatus status)
		{
			// if (Status != OrderStatus.Pending)
			// throw new InvalidOperationException("Order can only be confirmed if it is in Pending status.");
			AggregateId = id;
			Status = status;
			RaiseEvent(new OrderChangedStatusEvent(AggregateId, status));
		}

		public void CancelOrder()
		{
			if (Status != OrderStatus.Pending)
				throw new InvalidOperationException("Order can only be cancelled if it is in Pending status.");

			Status = OrderStatus.Cancelled;
			RaiseEvent(new OrderCancelledEvent(AggregateId));
		}

		// Добавьте другие методы бизнес-логики, если необходимо

		#endregion

		#region Event Handling

		// Обработка событий для восстановления состояния агрегата
		protected override void When(IDomainEvent @event)
		{
			switch (@event)
			{
				case OrderCreatedEvent e: OnOrderCreatedEvent(e); break;
				case OrderChangedStatusEvent e: OnOrderChangedStatusEvent(e); break;
				case OrderCancelledEvent e: OnOrderCancelledEvent(e); break;
			}
		}

		private void OnOrderCreatedEvent(OrderCreatedEvent @event)
		{
			AggregateId = @event.AggregateId;
			CustomerId = @event.CustomerId;
			ProductId = @event.ProductId;
			OrderDate = @event.OrderDate;
			OrderAmount = @event.OrderAmount;
			Status = @event.Status;
		}

		private void OnOrderChangedStatusEvent(OrderChangedStatusEvent @event)
		{
			AggregateId = @event.AggregateId;
			Status = @event.Status;
		}

		private void OnOrderCancelledEvent(OrderCancelledEvent @event)
		{
			Status = OrderStatus.Cancelled;
		}

		#endregion
	}
}
