using Astria.EventSourcingRepository.Repository;
using MediatR;
using ProductCommentsManagement.Application.BoundedContexts.ProductCommentsManagement.Commands;
using ProductCommentsManagement.Application.Results;
using ProductCommentsManagement.Domain.BoundedContexts.ProductCommentsManagement.Aggregates;
using ProductCommentsManagement.Domain.Exceptions;
using ProductCommentsManagement.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCommentsManagement.Application.BoundedContexts.ProductCommentsManagement.EventHandlers
{
	public class ProductCommentsDeletedEventHandler : IRequestHandler<ProductCommentsDeleteCommand, CommandResult>
	{
		private readonly IEventSourcingRepository<ProductComments> _repository;
		private readonly IProductCommentsRepository<Infrastructure.Entities.ProductComments> _productsRepository;

		public ProductCommentsDeletedEventHandler(IEventSourcingRepository<ProductComments> repository, IProductCommentsRepository<Infrastructure.Entities.ProductComments> productsRepository)
		{
			_productsRepository = productsRepository ?? throw new ArgumentNullException(nameof(repository));
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		public async Task<CommandResult> Handle(ProductCommentsDeleteCommand command, CancellationToken cancellationToken)
		{
			try
			{
				var product = await _productsRepository.FindByIdAsync(command.Id);
				Console.WriteLine("Id: " + command.Id);
				var entity = new ProductComments();
				entity.DeleteComment(command.Id);
				
				await _productsRepository.DeleteComment(command.Id);
				
				await _repository.SaveAsync(entity);

				return CommandResult.Success();
			}
			catch (DomainException ex)
			{
				return CommandResult.BusinessFail(ex.Message);
			}
		}
	}
}
