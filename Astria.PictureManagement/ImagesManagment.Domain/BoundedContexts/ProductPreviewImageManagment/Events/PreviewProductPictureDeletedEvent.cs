using Astria.SharedKernel;

namespace ImagesManagment.Domain.BoundedContexts.ProductPreviewImageManagment.Events
{
	public class PreviewProductPictureDeletedEvent : DomainEvent
	{
		public Guid Id { get; private set; }

		public PreviewProductPictureDeletedEvent(Guid aggregateId, Guid id)
			: base(aggregateId)
		{
			Id = id;
		}
	}
}
