using Astria.EventSourcingRepository.Repository;
using MediatR;
using ProductManagement.Application.BoundedContexts.Commands;
using ProductManagement.Application.Results;
using ProductManagement.Infrastructure.Entities;
using ProductManagement.Infrastructure.Repositories;
using ProductManagement.Domain.BoundedContexts.ProductManagement.Aggregates;
using ProductManagement.Domain.Exceptions;

namespace ProductManagement.Application.BoundedContexts.EventHandlers
{
	public class ProductCreatedEventHandler : IRequestHandler<ProductCreateCommand, CommandResult>
	{
		private readonly IEventSourcingRepository<ProductManagement.Domain.BoundedContexts.ProductManagement.Aggregates.Product> _repository;
		private readonly IProductRepository _productsRepository;

		public ProductCreatedEventHandler(IEventSourcingRepository<ProductManagement.Domain.BoundedContexts.ProductManagement.Aggregates.Product> repository, IProductRepository productsRepository)
		{
			_productsRepository = productsRepository ?? throw new ArgumentNullException(nameof(repository));
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		public async Task<CommandResult> Handle(ProductCreateCommand command, CancellationToken cancellationToken)
		{
			try
			{
				var orderId = Guid.NewGuid();
				var order = new ProductManagement.Domain.BoundedContexts.ProductManagement.Aggregates.Product(orderId, command.Name, command.Description, command.Price, command.IsAvailable, command.Date, command.ModelId, command.PreviewImageId, command.UserId);
				var entity = new Infrastructure.Entities.Product
				{
					ProductId = orderId,
					Name = command.Name,
					Description = command.Description,
					Price = command.Price,
					IsAvailable = command.IsAvailable,
					Date = command.Date,
					ModelId = command.ModelId,
					PreviewImageId = command.PreviewImageId,
					UserId = command.UserId,
				};

				await _productsRepository.CreateProduct(entity);

				await _repository.SaveAsync(order);
				return CommandResult.Success(entity, orderId, command.ModelId, command.PreviewImageId);
			}
			catch (DomainException ex)
			{
				return CommandResult.BusinessFail(ex.Message);
			}
		}
	}
}
