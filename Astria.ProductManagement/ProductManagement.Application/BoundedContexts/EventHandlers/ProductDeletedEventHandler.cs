using Astria.EventSourcingRepository.Repository;
using MediatR;
using ProductManagement.Application.BoundedContexts.Commands;
using ProductManagement.Application.Results;
using ProductManagement.Infrastructure.Repositories;
using ProductManagement.Domain.BoundedContexts.ProductManagement.Aggregates;
using ProductManagement.Domain.Exceptions;

namespace ProductManagement.Application.BoundedContexts.EventHandlers
{
	public class ProductDeletedEventHandler : IRequestHandler<ProductDeleteCommand, CommandResult>
	{
		private readonly IEventSourcingRepository<Product> _repository;
		private readonly IProductRepository _productsRepository;

		public ProductDeletedEventHandler(IEventSourcingRepository<Product> repository, IProductRepository productsRepository)
		{
			_productsRepository = productsRepository ?? throw new ArgumentNullException(nameof(repository));
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		public async Task<CommandResult> Handle(ProductDeleteCommand command, CancellationToken cancellationToken)
		{
			try
			{
				// Тут вызывать Event у Order 

				// admin.ChangeEmail(command.NewEmail);
				// В Celestial.Application -> Commands -> AdminAccountChangeEmailCommand
				var product = await _repository.FindByIdAsync(command.Id);
				Console.WriteLine("Deleted: " + command.Id);
				product.DeleteOrder(command.Id);

				await _productsRepository.DeleteProduct(command.Id);

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
