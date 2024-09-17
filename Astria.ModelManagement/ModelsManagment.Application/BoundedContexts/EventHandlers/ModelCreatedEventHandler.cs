using Astria.EventSourcingRepository.Repository;
using Astria.QueryRepository.Repository;
using MediatR;
using ModelsManagment.Application.BoundedContexts.Commands;
using ModelsManagment.Application.BoundedContexts.QueryObjects;
using ModelsManagment.Application.Results;
using ModelsManagment.Domain.BoundedContexts.ModelsManagment.Aggregates;
using ModelsManagment.Domain.Exceptions;
using ModelsManagment.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EventStore.Client.StreamMessage;

namespace ModelsManagment.Application.BoundedContexts.EventHandlers
{
	public class ModelCreatedEventHandler : IRequestHandler<ModelCreateCommand, CommandResult>
	{
		private readonly IEventSourcingRepository<Model> _repository;
		private readonly IModelRepository _productsRepository;
		private readonly IProjectionRepository<ModelInfo> _Projectionrepository;
		public ModelCreatedEventHandler(IEventSourcingRepository<Model> repository, IModelRepository productsRepository, IProjectionRepository<ModelInfo> projectionrepository)
		{
			_productsRepository = productsRepository ?? throw new ArgumentNullException(nameof(repository));
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
			_Projectionrepository = projectionrepository ?? throw new ArgumentNullException(nameof(projectionrepository));
		}

		public async Task<CommandResult> Handle(ModelCreateCommand command, CancellationToken cancellationToken)
		{
			try
			{
				var model = new Model(command.ModelId, command.ProductId, command.ModelDataUrl, command.TextureId, command.ModelType);
				Console.WriteLine("Model Created with Id: " + command.ModelId);
				var entity = new Infrastructure.Entities.Model
				{
					ModelId = command.ModelId,
					ProductId = command.ProductId,
					ModelDataUrl = command.ModelDataUrl,
					TextureId = command.TextureId,
					BinFileDataUrl = command.BinFileDataUrl,
					ModelType = command.ModelType,
				};

				await _productsRepository.CreateModel(entity);

				/*
				var order = new ModelInfo
				{
					Id = command.ModelId,
					ProductId = command.ProductId,
					ModelDataUrl = command.ModelDataUrl,
					TextureId = command.TextureId,
					ModelType = command.ModelType
				};

				await _Projectionrepository.InsertAsync(order);
				*/
				await _repository.SaveAsync(new Model(command.ModelId, model.ProductId, command.ModelDataUrl, command.TextureId, command.ModelType));
				return CommandResult.Success();
			}
			catch (DomainException ex)
			{
				return CommandResult.BusinessFail(ex.Message);
			}
		}
	}
}
