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
	public class ProductCommentsCreatedEventHandler : IRequestHandler<ProductCommentsCreateCommand, CommandResult>
	{
		private readonly IEventSourcingRepository<ProductComments> _repository;

		private readonly IProductCommentsRepository<Infrastructure.Entities.ProductComments> _productsRepository;

		public ProductCommentsCreatedEventHandler(IEventSourcingRepository<ProductComments> repository, IProductCommentsRepository<Infrastructure.Entities.ProductComments> productsRepository)
		{
			_productsRepository = productsRepository ?? throw new ArgumentNullException(nameof(repository));
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		public async Task<CommandResult> Handle(ProductCommentsCreateCommand command, CancellationToken cancellationToken)
		{
			try
			{
				command.Id = Guid.NewGuid();
				var entity = new ProductComments(command.Id, command.ProductId, command.UserId, command.CommentText, command.ParentCommentId);

				await _productsRepository.CreateComment(command.UserId, command.ProductId, command.CommentText);

				await _repository.SaveAsync(entity);
				return CommandResult.Success(entity.UserId);
			}
			catch (DomainException ex)
			{
				return CommandResult.BusinessFail(ex.Message);
			}
		}
	}
}
