using Astria.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsManagment.Domain.BoundedContexts.ModelsManagment.Events
{
	public class ModelDeletedEvent : DomainEvent
	{
		public Guid Id { get; private set; }

		public ModelDeletedEvent(Guid aggregateId, Guid id)
			: base(aggregateId)
		{
			Id = id;
		}
	}
}
