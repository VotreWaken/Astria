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
    public class ImageDeletedEventHandler : IRequestHandler<ImageDeleteCommand, CommandResult>
    {
        private readonly IEventSourcingRepository<Picture> _repository;
        private readonly IPictureRepository<ImagesManagment.Infrastructure.Entities.Picture> _productsRepository;

        public ImageDeletedEventHandler(IEventSourcingRepository<Picture> repository, IPictureRepository<ImagesManagment.Infrastructure.Entities.Picture> productsRepository)
        {
            _productsRepository = productsRepository ?? throw new ArgumentNullException(nameof(repository));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<CommandResult> Handle(ImageDeleteCommand command, CancellationToken cancellationToken)
        {
            try
            {
                // Тут вызывать Event у Order 

                // admin.ChangeEmail(command.NewEmail);
                // В Celestial.Application -> Commands -> AdminAccountChangeEmailCommand
                var product = await _repository.FindByIdAsync(command.Id);
                Console.WriteLine("Deleted: " + command.Id);
                product.DeleteOrder(command.Id);

                await _productsRepository.Delete(command.Id);

                await _repository.SaveAsync(product);

                return CommandResult.Success();
            }
            catch (DomainException ex)
            {
                return CommandResult.BusinessFail(ex.Message);
            }
        }
    }
}
