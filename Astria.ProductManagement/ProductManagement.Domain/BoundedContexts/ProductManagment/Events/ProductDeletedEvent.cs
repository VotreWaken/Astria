using Astria.SharedKernel;

namespace ProductManagement.Domain.BoundedContexts.ProductManagement.Events
{
	public class ProductDeletedEvent : DomainEvent
	{
		public Guid Id { get; private set; }

		public ProductDeletedEvent(Guid aggregateId, Guid id)
			: base(aggregateId)
		{
			Id = id;
		}
	}
}
