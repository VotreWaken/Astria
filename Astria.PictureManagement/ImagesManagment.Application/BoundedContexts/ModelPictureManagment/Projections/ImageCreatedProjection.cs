using Astria.QueryRepository.Repository;
using ImagesManagment.Application.BoundedContexts.ModelPictureManagment.QueryObjects;
using ImagesManagment.Application.BoundedContexts.UserImageManagment.QueryObjects;
using ImagesManagment.Domain.BoundedContexts.ImageManagment.Events;
using ImagesManagment.Domain.BoundedContexts.ModelTextureManagment.Events;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImagesManagment.Application.BoundedContexts.ModelPictureManagment.Projections
{
	public class ImageCreatedProjection : INotificationHandler<ModelPictureCreatedEvent>
	{
		private readonly IProjectionRepository<ModelPictureInfo> _repository;

		public ImageCreatedProjection(IProjectionRepository<ModelPictureInfo> repository)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		public async Task Handle(ModelPictureCreatedEvent @event, CancellationToken cancellationToken)
		{
			var existingDocument = await _repository.FindByIdAsync(@event.TextureId);

			if (existingDocument == null)
			{
				// Если документа нет, вставляем новый
				var newDocument = new ModelPictureInfo
				{
					Id = @event.TextureId,
					ModelId = @event.ModelId,
					BaseColorUrl = @event.BaseColorUrl,
					DisplacementUrl = @event.DisplacementUrl,
					EmissiveUrl = @event.EmissiveUrl,
					MetallicUrl = @event.MetallicUrl,
					NormalMapUrl = @event.NormalMapUrl,
					RoughnessUrl = @event.RoughnessUrl,
				};

				await _repository.InsertAsync(newDocument);
			}
			else
			{
				// Если документ существует, обновляем его поля
				existingDocument.BaseColorUrl = @event.BaseColorUrl;
				existingDocument.DisplacementUrl = @event.DisplacementUrl;
				existingDocument.EmissiveUrl = @event.EmissiveUrl;
				existingDocument.MetallicUrl = @event.MetallicUrl;
				existingDocument.NormalMapUrl = @event.NormalMapUrl;
				existingDocument.RoughnessUrl = @event.RoughnessUrl;

				await _repository.UpdateAsync(existingDocument);
			}
		}
	}
}
