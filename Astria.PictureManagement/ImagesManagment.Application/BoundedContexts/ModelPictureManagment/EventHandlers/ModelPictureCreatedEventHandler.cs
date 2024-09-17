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
	public class ModelPictureCreateOrUpdateCommandHandler : IRequestHandler<ModelPictureCreateOrUpdateCommand, CommandResult>
	{
		private readonly IEventSourcingRepository<ModelPicture> _repository;
		private readonly IModelTextureRepository<ImagesManagment.Infrastructure.Entities.ModelPicture> _productsRepository;

		public ModelPictureCreateOrUpdateCommandHandler(IEventSourcingRepository<ModelPicture> repository, IModelTextureRepository<ImagesManagment.Infrastructure.Entities.ModelPicture> productsRepository)
		{
			_productsRepository = productsRepository ?? throw new ArgumentNullException(nameof(repository));
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		public async Task<CommandResult> Handle(ModelPictureCreateOrUpdateCommand command, CancellationToken cancellationToken)
		{
			try
			{
				// ModelId метод добавить 
				var entity = await _productsRepository.GetByModelId(command.ModelId);

				if (entity == null)
				{
					// Создать новую запись, если не существует
					entity = new Infrastructure.Entities.ModelPicture
					{
						TextureId = Guid.NewGuid(),
						ModelId = command.ModelId
					};

					await _productsRepository.Create(entity);
				}

				// Обновляем только нужное поле
				switch (command.TextureType)
				{
					case "baseColorFile":
						entity.BaseColorUrl = command.ImageUrl;
						break;
					case "normalMapFile":
						entity.NormalMapUrl = command.ImageUrl;
						break;
					case "displacementFile":
						entity.DisplacementUrl = command.ImageUrl;
						break;
					case "metallicFile":
						entity.MetallicUrl = command.ImageUrl;
						break;
					case "roughnessFile":
						entity.RoughnessUrl = command.ImageUrl;
						break;
					case "emissiveFile":
						entity.EmissiveUrl = command.ImageUrl;
						break;
					default:
						throw new ArgumentException("Unknown texture type");
				}

				await _productsRepository.Update(entity);

				var order = new ModelPicture(entity.TextureId, entity.ModelId, entity.BaseColorUrl,
					entity.NormalMapUrl, entity.DisplacementUrl, entity.MetallicUrl,
					entity.RoughnessUrl, entity.EmissiveUrl);

				await _repository.SaveAsync(order);

				return CommandResult.Success(entity.TextureId);
			}
			catch (DomainException ex)
			{
				return CommandResult.BusinessFail(ex.Message);
			}
		}
	}
}
