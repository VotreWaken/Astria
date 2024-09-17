using Astria.QueryRepository.Repository;
using MediatR;
using ModelsManagment.Application.BoundedContexts.QueryObjects;
using ModelsManagment.Domain.BoundedContexts.ModelsManagment.Events;

namespace ModelsManagment.Application.BoundedContexts.Projections
{
	internal class ModelDeletedProjection : INotificationHandler<ModelDeletedEvent>
	{
		private readonly IProjectionRepository<ModelInfo> _repository;

		public ModelDeletedProjection(IProjectionRepository<ModelInfo> repository)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
			Console.WriteLine("Start");
		}

		public async Task Handle(ModelDeletedEvent @event, CancellationToken cancellationToken)
		{
			try
			{
				Console.WriteLine("Projection Id: " + @event.AggregateId);
				await _repository.DeleteAsync(@event.Id);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				throw;
			}

		}
	}
}
