namespace Astria.SharedKernel
{
	public interface IAggregateRoot
	{
		public Guid AggregateId { get; }
		public long Version { get; }

		public void LoadFromHistory(long version, IEnumerable<IDomainEvent> history);

		public IEnumerable<IDomainEvent> GetUncommittedChanges();

		public void MarkChangesAsCommitted();
	}
}
