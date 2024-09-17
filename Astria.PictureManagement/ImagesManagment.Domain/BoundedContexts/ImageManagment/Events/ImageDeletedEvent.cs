using Astria.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImagesManagment.Domain.BoundedContexts.ImageManagment.Events
{
	public class ImageDeletedEvent : DomainEvent
	{
		public Guid Id { get; private set; }

		public ImageDeletedEvent(Guid aggregateId, Guid id)
			: base(aggregateId)
		{
			Id = id;
		}
	}
}
