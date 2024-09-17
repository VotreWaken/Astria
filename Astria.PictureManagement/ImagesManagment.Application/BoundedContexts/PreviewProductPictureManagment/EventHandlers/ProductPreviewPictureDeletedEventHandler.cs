using Astria.EventSourcingRepository.Repository;
using ImagesManagment.Application.BoundedContexts.PreviewProductPictureManagment.Commands;
using ImagesManagment.Application.Results;
using ImagesManagment.Domain.BoundedContexts.ProductPreviewImageManagment.Aggregates;
using ImagesManagment.Domain.Exceptions;
using ImagesManagment.Infrastructure.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImagesManagment.Application.BoundedContexts.PreviewProductPictureManagment.EventHandlers
{
	public class ProductPreviewPictureDeletedEventHandler : IRequestHandler<ProductPreviewPictureDeleteCommand, CommandResult>
	{
		private readonly IEventSourcingRepository<PreviewProductPicture> _repository;
		private readonly IPictureRepository<ImagesManagment.Infrastructure.Entities.PreviewPicture> _productsRepository;

		public ProductPreviewPictureDeletedEventHandler(IEventSourcingRepository<PreviewProductPicture> repository, IPictureRepository<ImagesManagment.Infrastructure.Entities.PreviewPicture> productsRepository)
		{
			_productsRepository = productsRepository ?? throw new ArgumentNullException(nameof(repository));
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		public async Task<CommandResult> Handle(ProductPreviewPictureDeleteCommand command, CancellationToken cancellationToken)
		{
			try
			{
				// Тут вызывать Event у Order 

				// admin.ChangeEmail(command.NewEmail);
				// В Celestial.Application -> Commands -> AdminAccountChangeEmailCommand
				var product = await _repository.FindByIdAsync(command.ProductPreviewImageId);
				Console.WriteLine("Deleted: " + command.ProductPreviewImageId);
				product.DeleteOrder(command.ProductPreviewImageId);

				await _productsRepository.Delete(command.ProductPreviewImageId);

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
