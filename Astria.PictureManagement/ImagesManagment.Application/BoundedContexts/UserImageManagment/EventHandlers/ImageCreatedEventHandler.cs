using Astria.EventSourcingRepository.Repository;
using ImagesManagment.Application.BoundedContexts.UserImageManagment.Commands;
using ImagesManagment.Application.Results;
using ImagesManagment.Domain.BoundedContexts.ImageManagment.Aggregates;
using ImagesManagment.Domain.Exceptions;
using ImagesManagment.Infrastructure.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImagesManagment.Application.BoundedContexts.UserImageManagment.EventHandlers
{
    public class ImageCreatedEventHandler : IRequestHandler<ImageCreateCommand, CommandResult>
    {
        private readonly IEventSourcingRepository<Picture> _repository;
        private readonly IPictureRepository<ImagesManagment.Infrastructure.Entities.Picture> _productsRepository;

        public ImageCreatedEventHandler(IEventSourcingRepository<Picture> repository, IPictureRepository<ImagesManagment.Infrastructure.Entities.Picture> productsRepository)
        {
            _productsRepository = productsRepository ?? throw new ArgumentNullException(nameof(repository));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<CommandResult> Handle(ImageCreateCommand command, CancellationToken cancellationToken)
        {
            try
            {
				var orderId = command.ImageId;
				var order = new Picture(orderId, command.UserImageUrl);

				var existingImage = await _productsRepository.GetById(orderId);

				Console.WriteLine("Existing Image: " + existingImage);
				if (existingImage != null)
				{
					Console.WriteLine("Create New Image");
					// Обновление существующего изображения
					existingImage.ImageUrl = command.UserImageUrl;
					await _productsRepository.Update(existingImage);
				}
				else
				{
					Console.WriteLine("Existing Image");
					// Создание нового изображения
					var entity = new Infrastructure.Entities.Picture
					{
						ImageId = orderId,
						ImageUrl = command.UserImageUrl,
					};
					await _productsRepository.Create(entity);
				}

				await _repository.SaveAsync(order);
				return CommandResult.Success();
			}
            catch (DomainException ex)
            {
                return CommandResult.BusinessFail(ex.Message);
            }
        }
    }
}
