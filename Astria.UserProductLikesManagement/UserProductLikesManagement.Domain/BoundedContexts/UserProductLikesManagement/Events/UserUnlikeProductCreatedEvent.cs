using Astria.SharedKernel;

namespace UserProductLikesManagement.Domain.BoundedContexts.UserProductLikesManagement.Events
{
	public class UserUnlikeProductCreatedEvent : DomainEvent
	{
		public Guid UserId { get; private set; }
		public Guid ProductId { get; private set; }

		public UserUnlikeProductCreatedEvent(Guid aggregateId, Guid id)
			: base(aggregateId)
		{
			ProductId = id;
		}
	}
}
