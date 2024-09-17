using Astria.EventSourcingRepository.Repository;
using MediatR;
using ProductManagement.Application.BoundedContexts.Commands;
using ProductManagement.Application.Results;
using ProductManagement.Domain.BoundedContexts.ProductManagement.Aggregates;
using ProductManagement.Domain.Exceptions;
using ProductManagement.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Application.BoundedContexts.EventHandlers
{
	public class ProductUpdateEventHandler : IRequestHandler<ProductUpdateCommand, CommandResult>
	{
		private readonly IEventSourcingRepository<ProductManagement.Domain.BoundedContexts.ProductManagement.Aggregates.Product> _repository;
		private readonly IProductRepository _productsRepository;

		public ProductUpdateEventHandler(
			IEventSourcingRepository<ProductManagement.Domain.BoundedContexts.ProductManagement.Aggregates.Product> repository,
			IProductRepository productsRepository)
		{
			_productsRepository = productsRepository ?? throw new ArgumentNullException(nameof(productsRepository));
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		public async Task<CommandResult> Handle(ProductUpdateCommand command, CancellationToken cancellationToken)
		{
			try
			{
				var product = await _productsRepository.GetProductById(command.ProductId);
				if (product == null)
				{
					// return CommandResult.NotFound($"Product with ID {command.ProductId} not found.");
				}

				// Apply updates only for fields provided in the command
				if (!string.IsNullOrEmpty(command.Name))
				{
					// product.UpdateName(command.Name);
				}

				if (!string.IsNullOrEmpty(command.Description))
				{
					// product.UpdateDescription(command.Description);
				}

				if (command.Price.HasValue)
				{
					// product.UpdatePrice(command.Price.Value);
				}
				product.Price = command.Price.Value;
				product.Name = command.Name;
				product.Description = command.Description;
				

				var ent = new Product();
				ent.UpdateOrder(product.ProductId, product.Name, product.Description, product.Price,
					product.IsAvailable, product.Date, product.ModelId, product.PreviewImageId,
					product.UserId, product.Views);
				
				await _productsRepository.UpdateProduct(product);
				await _repository.SaveAsync(ent);

				return CommandResult.Success(product, command.ProductId);
			}
			catch (DomainException ex)
			{
				return CommandResult.BusinessFail(ex.Message);
			}
		}
	}
}
