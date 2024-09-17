using Astria.QueryRepository.Repository;
using MediatR;
using ModelsManagment.Application.BoundedContexts.QueryObjects;
using ModelsManagment.Domain.BoundedContexts.ModelsManagment.Events;

namespace ModelsManagment.Application.BoundedContexts.Projections
{
	public class ModelCreatedProjection : INotificationHandler<ModelCreatedEvent>
	{
		private readonly IProjectionRepository<ModelInfo> _repository;

		public ModelCreatedProjection(IProjectionRepository<ModelInfo> repository)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		public async Task Handle(ModelCreatedEvent @event, CancellationToken cancellationToken)
		{
			Console.WriteLine("Created Model in MongoDB with Id" + @event.AggregateId);
			Console.WriteLine("Created Model in MongoDB with Model Data Length" + @event.ModelDataUrl);
			var order = new ModelInfo
			{
				Id = @event.AggregateId,
				ProductId = @event.ProductId,
				ModelDataUrl = @event.ModelDataUrl,
				TextureId = @event.TextureId,
				ModelType = @event.ModelType
			};

			await _repository.InsertAsync(order);
		}
	}
}
