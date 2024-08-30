namespace Astria.SharedKernel
{
	public interface IDomainEvent
	{
		Guid EventId { get; }

		Guid AggregateId { get; }

		long AggregateVersion { get; set; }
	}
}
