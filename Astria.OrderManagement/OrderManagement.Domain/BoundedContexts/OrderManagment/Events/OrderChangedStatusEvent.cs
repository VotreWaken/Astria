using Astria.SharedKernel;
using OrderManagement.Domain.BoundedContexts.OrderManagment.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Domain.BoundedContexts.OrderManagment.Events
{
	public class OrderChangedStatusEvent : DomainEvent
	{
		public Guid AggregateId { get; }

		public OrderStatus Status { get; }

		public OrderChangedStatusEvent(Guid aggregateId, OrderStatus status)
		{
			AggregateId = aggregateId;
			Status = status;
		}
	}

}
