using Astria.SharedKernel;

namespace ImagesManagment.Domain.BoundedContexts.ProductPreviewImageManagment.Events
{
	public class PreviewProductPictureCreatedEvent : DomainEvent
	{
		public Guid PreviewImageId { get; set; }
		public Guid ProductId { get; set; }
		public string ImageUrl { get; set; }

		public PreviewProductPictureCreatedEvent(Guid aggregateId, Guid modelId, string url)
			: base(aggregateId)
		{
			PreviewImageId = aggregateId;
			ProductId = modelId;
			ImageUrl = url;
		}
	}
}
