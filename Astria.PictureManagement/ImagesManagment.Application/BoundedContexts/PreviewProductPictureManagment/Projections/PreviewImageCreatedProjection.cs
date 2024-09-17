using Astria.QueryRepository.Repository;
using ImagesManagment.Application.BoundedContexts.PreviewProductPictureManagment.QueryObjects;
using ImagesManagment.Domain.BoundedContexts.ProductPreviewImageManagment.Events;
using MediatR;

namespace ImagesManagment.Application.BoundedContexts.PreviewProductPictureManagment.Projections
{
	public class PreviewImageCreatedProjection : INotificationHandler<PreviewProductPictureCreatedEvent>
	{
		private readonly IProjectionRepository<ProductPreviewImageInfo> _repository;

		public PreviewImageCreatedProjection(IProjectionRepository<ProductPreviewImageInfo> repository)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		public async Task Handle(PreviewProductPictureCreatedEvent @event, CancellationToken cancellationToken)
		{
			var order = new ProductPreviewImageInfo
			{
				Id = @event.AggregateId,
				ProductId = @event.ProductId,
				Url = @event.ImageUrl,
			};

			await _repository.InsertAsync(order);
		}
	}
}
