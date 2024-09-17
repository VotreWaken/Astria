using Astria.EventSourcingRepository.Repository;
using MediatR;
using ModelsManagment.Application.BoundedContexts.Commands;
using ModelsManagment.Application.Results;
using ModelsManagment.Domain.BoundedContexts.ModelsManagment.Aggregates;
using ModelsManagment.Domain.Exceptions;
using ModelsManagment.Infrastructure.Repositories;

namespace ModelsManagment.Application.BoundedContexts.EventHandlers
{
	public class ModelDeletedEventHandler : IRequestHandler<ModelDeleteCommand, CommandResult>
	{
		private readonly IEventSourcingRepository<Model> _repository;
		private readonly IModelRepository _productsRepository;

		public ModelDeletedEventHandler(IEventSourcingRepository<Model> repository, IModelRepository productsRepository)
		{
			_productsRepository = productsRepository ?? throw new ArgumentNullException(nameof(repository));
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		public async Task<CommandResult> Handle(ModelDeleteCommand command, CancellationToken cancellationToken)
		{
			try
			{
				var model = await _repository.FindByIdAsync(command.Id);
				Console.WriteLine("Deleted: " + command.Id);
				model.DeleteOrder(command.Id);

				await _productsRepository.DeleteModel(command.Id);

				await _repository.SaveAsync(model);

				return CommandResult.Success();
			}
			catch (DomainException ex)
			{
				return CommandResult.BusinessFail(ex.Message);
			}
		}
	}
}
