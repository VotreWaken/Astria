using Astria.SharedKernel;

namespace ProductManagement.Domain.BoundedContexts.ProductManagement.Events
{
	public class ProductCreatedEvent : DomainEvent
	{
		public string Name { get; private set; }
		public string Description { get; private set; }
		public decimal Price { get; private set; }
		public bool IsAvailable { get; private set; }
		public Guid ModelId { get; private set; }
		public DateTime Date { get; private set; }
		public Guid PreviewImageId { get; private set; }
		public Guid UserId { get; private set; }


		public ProductCreatedEvent(Guid aggregateId, string productName, string productDescription, decimal productPrice, bool productIsAvailable, DateTime orderDate, Guid modelId, Guid previewImageId, Guid userId)
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
		}
	}
}
