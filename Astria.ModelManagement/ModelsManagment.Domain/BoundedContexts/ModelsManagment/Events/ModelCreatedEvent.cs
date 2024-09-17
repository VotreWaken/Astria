using Astria.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsManagment.Domain.BoundedContexts.ModelsManagment.Events
{
	public class ModelCreatedEvent : DomainEvent
	{
		public Guid ProductId { get; private set; }
		public Guid ModelId { get; private set; }
		public string ModelDataUrl { get; private set; }
		public string ModelType { get; private set; }
		public Guid TextureId { get; private set; }

		public ModelCreatedEvent(Guid aggregateId, Guid productId, string modelDataUrl, Guid textureId, string modelType)
			: base(aggregateId)
		{
			ModelId = aggregateId;
			ProductId = productId;
			ModelDataUrl = modelDataUrl;
			TextureId = textureId;
			ModelType = modelType;
		}
	}
}
