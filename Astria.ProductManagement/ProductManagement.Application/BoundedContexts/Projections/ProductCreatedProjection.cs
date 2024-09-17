using Astria.QueryRepository.Repository;
using MediatR;
using ProductManagement.Application.BoundedContexts.QueryObjects;
using ProductManagement.Domain.BoundedContexts.ProductManagement.Events;

namespace ProductManagement.Application.BoundedContexts.Projections
{
	internal class ProductCreatedProjection : INotificationHandler<ProductCreatedEvent>
	{
		private readonly IProjectionRepository<ProductInfo> _repository;

		public ProductCreatedProjection(IProjectionRepository<ProductInfo> repository)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		public async Task Handle(ProductCreatedEvent @event, CancellationToken cancellationToken)
		{
			var order = new ProductInfo
			{
				Id = @event.AggregateId,
				Name = @event.Name,
				Date = @event.Date,
				Description = @event.Description,
				IsAvailable = @event.IsAvailable,
				Price = @event.Price,
				Version = @event.AggregateVersion,
				PreviewImageId = @event.PreviewImageId,
				ModelId = @event.ModelId,
				UserId = @event.UserId,
			};

			await _repository.InsertAsync(order);
		}
	}
}
