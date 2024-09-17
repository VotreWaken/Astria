using Astria.EventSourcingRepository.Repository;
using ImagesManagment.Application.BoundedContexts.ModelPictureManagment.Commands;
using ImagesManagment.Application.Results;
using ImagesManagment.Domain.BoundedContexts.ModelTextureManagment.Aggregates;
using ImagesManagment.Domain.Exceptions;
using ImagesManagment.Infrastructure.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImagesManagment.Application.BoundedContexts.ModelPictureManagment.EventHandlers
{
	public class ModelPictureDeletedEventHandler : IRequestHandler<ModelPictureDeleteCommand, CommandResult>
	{
		private readonly IEventSourcingRepository<ModelPicture> _repository;
		private readonly IModelTextureRepository<ImagesManagment.Infrastructure.Entities.ModelPicture> _productsRepository;

		public ModelPictureDeletedEventHandler(IEventSourcingRepository<ModelPicture> repository, IModelTextureRepository<ImagesManagment.Infrastructure.Entities.ModelPicture> productsRepository)
		{
			_productsRepository = productsRepository ?? throw new ArgumentNullException(nameof(repository));
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		public async Task<CommandResult> Handle(ModelPictureDeleteCommand command, CancellationToken cancellationToken)
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
