using Astria.SharedKernel;

namespace ImagesManagment.Domain.BoundedContexts.ModelTextureManagment.Events
{
	public class ModelPictureDeletedEvent : DomainEvent
	{
		public Guid Id { get; private set; }

		public ModelPictureDeletedEvent(Guid aggregateId, Guid id)
			: base(aggregateId)
		{
			Id = id;
		}
	}
}
