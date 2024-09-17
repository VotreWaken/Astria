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
	public class ImageDeletedProjection : INotificationHandler<ModelPictureDeletedEvent>
	{
		private readonly IProjectionRepository<ModelPictureInfo> _repository;

		public ImageDeletedProjection(IProjectionRepository<ModelPictureInfo> repository)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
			Console.WriteLine("Start");
		}

		public async Task Handle(ModelPictureDeletedEvent @event, CancellationToken cancellationToken)
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
