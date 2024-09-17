using Astria.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Domain.BoundedContexts.ProductManagment.Events
{
	public class ProductUpdatedEvent : DomainEvent
	{
		public string Name { get; private set; }
		public string Description { get; private set; }
		public decimal Price { get; private set; }
		public bool IsAvailable { get; private set; }
		public Guid ModelId { get; private set; }
		public DateTime Date { get; private set; }
		public Guid PreviewImageId { get; private set; }
		public Guid UserId { get; private set; }

		public int Views {  get; private set; }


		public ProductUpdatedEvent(Guid aggregateId, string productName, string productDescription, decimal productPrice, bool productIsAvailable, DateTime orderDate, Guid modelId, Guid previewImageId, Guid userId, int views)
			: base(aggregateId)
		{
			Name = productName;
			Description = productDescription;
			Price = productPrice;
			IsAvailable = productIsAvailable;
			Date = orderDate;
			ModelId = modelId;
			PreviewImageId = previewImageId;
			UserId = userId;
			Views = views;
		}
	}
}
