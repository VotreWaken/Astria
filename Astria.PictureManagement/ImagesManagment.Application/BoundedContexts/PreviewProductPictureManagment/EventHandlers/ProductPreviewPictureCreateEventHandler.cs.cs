using Astria.EventSourcingRepository.Repository;
using ImagesManagment.Application.BoundedContexts.PreviewProductPictureManagment.Commands;
using ImagesManagment.Application.Results;
using ImagesManagment.Domain.BoundedContexts.ProductPreviewImageManagment.Aggregates;
using ImagesManagment.Domain.Exceptions;
using ImagesManagment.Infrastructure.Entities;
using ImagesManagment.Infrastructure.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImagesManagment.Application.BoundedContexts.PreviewProductPictureManagment.EventHandlers
{
	public class ProductPreviewPictureCreateEventHandler : IRequestHandler<ProductPreviewPictureCreateCommand, CommandResult>
	{
		private readonly IEventSourcingRepository<PreviewProductPicture> _repository;
		private readonly IPictureRepository<ImagesManagment.Infrastructure.Entities.PreviewPicture> _productsRepository;

		public ProductPreviewPictureCreateEventHandler(IEventSourcingRepository<PreviewProductPicture> repository, IPictureRepository<ImagesManagment.Infrastructure.Entities.PreviewPicture> productsRepository)
		{
			_productsRepository = productsRepository ?? throw new ArgumentNullException(nameof(repository));
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		public async Task<CommandResult> Handle(ProductPreviewPictureCreateCommand command, CancellationToken cancellationToken)
		{
			try
			{
				Console.WriteLine("Product Id: " + command.ProductId);
				Console.WriteLine("Image Id:" + command.ProductPreviewImageId);
				Console.WriteLine("Image Url:" + command.Url);

				var order = new PreviewProductPicture(command.ProductPreviewImageId, command.ProductId, command.Url);

				var entity = new Infrastructure.Entities.PreviewPicture
				{
					PreviewImageId = command.ProductPreviewImageId,
					ProductId = command.ProductId,
					ImageUrl = command.Url,
				};

				await _productsRepository.Create(entity);

				await _repository.SaveAsync(order);
				return CommandResult.Success(entity.PreviewImageId);
			}
			catch (DomainException ex)
			{
				return CommandResult.BusinessFail(ex.Message);
			}
		}
	}
}
