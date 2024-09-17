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
    public class ImageDeletedProjection : INotificationHandler<ImageDeletedEvent>
    {
        private readonly IProjectionRepository<ImageInfo> _repository;

        public ImageDeletedProjection(IProjectionRepository<ImageInfo> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            Console.WriteLine("Start");
        }

        public async Task Handle(ImageDeletedEvent @event, CancellationToken cancellationToken)
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
