using Astria.SharedKernel;

namespace Astria.EventSourcingRepository.Client
{
	public interface IEventSourcingClient
	{
		Task<(long Version, IEnumerable<IDomainEvent> Events)> ReadEventsAsync(Guid aggregateId);
		Task AppendEventAsync(IDomainEvent @event);
	}
}
