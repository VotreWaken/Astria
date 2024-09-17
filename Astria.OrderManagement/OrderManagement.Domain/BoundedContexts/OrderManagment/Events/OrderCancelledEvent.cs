using Astria.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Domain.BoundedContexts.OrderManagment.Events
{
	public class OrderCancelledEvent : DomainEvent
	{
		public Guid AggregateId { get; }

		public OrderCancelledEvent(Guid aggregateId)
		{
			AggregateId = aggregateId;
		}
	}
}
