using Astria.QueryRepository.Repository;
using ImagesManagment.Application.BoundedContexts.UserImageManagment.QueryObjects;
using ImagesManagment.Domain.BoundedContexts.ImageManagment.Events;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImagesManagment.Application.BoundedContexts.UserImageManagment.Projections
{
    public class ImageCreatedProjection : INotificationHandler<ImageCreatedEvent>
    {
        private readonly IProjectionRepository<ImageInfo> _repository;

        public ImageCreatedProjection(IProjectionRepository<ImageInfo> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task Handle(ImageCreatedEvent @event, CancellationToken cancellationToken)
        {
			var existingImage = await _repository.FindByIdAsync(@event.AggregateId);
			if (existingImage != null)
			{
				// Обновление существующей записи
				existingImage.Url = @event.Url;
				await _repository.UpdateAsync(existingImage);
			}
			else
			{
				// Создание новой записи
				var order = new ImageInfo
				{
					Id = @event.AggregateId,
					Url = @event.Url,
				};

				await _repository.InsertAsync(order);
			}
		}
    }
}
