using Astria.EventSourcingRepository.Repository;
using MediatR;
using UserProductLikesManagement.Application.BoundedContexts.UserProductLikesManagement.Commands;
using UserProductLikesManagement.Application.Results;
using UserProductLikesManagement.Domain.BoundedContexts.UserProductLikesManagement.Aggregates;
using UserProductLikesManagement.Domain.Exceptions;
using UserProductLikesManagement.Infrastructure.Repositories;

namespace UserProductLikesManagement.Application.BoundedContexts.UserProductLikesManagement.EventHandlers
{
	public class UserProductLikeCreatedEventHandler : IRequestHandler<UserProductLikeCreateCommand, CommandResult>
	{
		private readonly IEventSourcingRepository<UserProductLike> _repository;

		private readonly IUserProductLikesRepository<Infrastructure.Entities.UserProductLike> _productsRepository;

		public UserProductLikeCreatedEventHandler(IEventSourcingRepository<UserProductLike> repository, IUserProductLikesRepository<Infrastructure.Entities.UserProductLike> productsRepository)
		{
			_productsRepository = productsRepository ?? throw new ArgumentNullException(nameof(repository));
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		public async Task<CommandResult> Handle(UserProductLikeCreateCommand command, CancellationToken cancellationToken)
		{
			try
			{
				// Проверяем, есть ли уже лайк для данного пользователя и продукта
				bool isAlreadyLiked = await _productsRepository.IsProductLikedByUserAsync(command.UserId, command.ProductId);

				if (isAlreadyLiked)
				{
					// Если лайк существует, удаляем его
					var entity = new UserProductLike(command.UserId, command.ProductId);
					await _productsRepository.UnlikeProductAsync(command.UserId, command.ProductId);
					await _repository.SaveAsync(entity);
					return CommandResult.Success("Product unliked successfully.");
				}
				else
				{
					// Если лайка нет, создаем новый
					var entity = new UserProductLike(command.UserId, command.ProductId);
					await _productsRepository.LikeProductAsync(command.UserId, command.ProductId);
					await _repository.SaveAsync(entity);
					return CommandResult.Success(entity.UserId);
				}
			}
			catch (DomainException ex)
			{
				return CommandResult.BusinessFail(ex.Message);
			}

		}
	}
}
