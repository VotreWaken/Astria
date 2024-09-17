using Astria.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImagesManagment.Domain.BoundedContexts.ImageManagment.Events
{
	public class ImageCreatedEvent : DomainEvent
	{
		public Guid ImageId { get; set; }
		public string Url { get; set; }

		public ImageCreatedEvent(Guid aggregateId, string url)
			: base(aggregateId)
		{
			ImageId = aggregateId;
			Url = url;
		}
	}
}
