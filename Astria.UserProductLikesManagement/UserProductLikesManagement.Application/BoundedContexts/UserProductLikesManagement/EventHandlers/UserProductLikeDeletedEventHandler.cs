using Astria.EventSourcingRepository.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserProductLikesManagement.Application.BoundedContexts.UserProductLikesManagement.Commands;
using UserProductLikesManagement.Application.Results;
using UserProductLikesManagement.Domain.BoundedContexts.UserProductLikesManagement.Aggregates;
using UserProductLikesManagement.Domain.Exceptions;
using UserProductLikesManagement.Infrastructure.Repositories;

namespace UserProductLikesManagement.Application.BoundedContexts.UserProductLikesManagement.EventHandlers
{
	public class UserProductLikeDeletedEventHandler : IRequestHandler<UserProductLikesDeleteCommand, CommandResult>
	{
		private readonly IEventSourcingRepository<UserProductLike> _repository;
		private readonly IUserProductLikesRepository<Infrastructure.Entities.UserProductLike> _productsRepository;

		public UserProductLikeDeletedEventHandler(IEventSourcingRepository<UserProductLike> repository, IUserProductLikesRepository<Infrastructure.Entities.UserProductLike> productsRepository)
		{
			_productsRepository = productsRepository ?? throw new ArgumentNullException(nameof(repository));
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		public async Task<CommandResult> Handle(UserProductLikesDeleteCommand command, CancellationToken cancellationToken)
		{
			try
			{
				// Тут вызывать Event у Order 

				// admin.ChangeEmail(command.NewEmail);
				// В Celestial.Application -> Commands -> AdminAccountChangeEmailCommand
				// var entity = await _repository.FindByIdAsync(command.UserId);
				var product = await _productsRepository.GetById(command.UserId);
				await _productsRepository.UnlikeProductAsync(command.UserId, command.ProductId);

				// await _repository.SaveAsync(entity);

				return CommandResult.Success();
			}
			catch (DomainException ex)
			{
				return CommandResult.BusinessFail(ex.Message);
			}
		}
	}
}
