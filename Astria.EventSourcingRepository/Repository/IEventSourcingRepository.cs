using Astria.SharedKernel;

namespace Astria.EventSourcingRepository.Repository
{
	public interface IEventSourcingRepository<TAggregate> where TAggregate : IAggregateRoot
	{
		Task<TAggregate> FindByIdAsync(Guid id);
		Task SaveAsync(TAggregate aggregate);
	}
}
