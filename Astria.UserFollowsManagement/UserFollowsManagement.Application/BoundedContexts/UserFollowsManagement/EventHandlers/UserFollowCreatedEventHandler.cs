using Astria.EventSourcingRepository.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserFollowsManagement.Application.BoundedContexts.UserFollowsManagement.Commands;
using UserFollowsManagement.Application.BoundedContexts.UserFollowsManagement.QueryObjects;
using UserFollowsManagement.Application.Results;
using UserFollowsManagement.Domain.BoundedContexts.UserFollowsManagement.Aggregates;
using UserFollowsManagement.Domain.Exceptions;
using UserFollowsManagement.Infrastructure.Repositories;

namespace UserFollowsManagement.Application.BoundedContexts.UserFollowsManagement.EventHandlers
{
	public class UserFollowCreatedEventHandler : IRequestHandler<UserFollowCreateCommand, CommandResult>
	{
		private readonly IEventSourcingRepository<UserFollows> _repository;

		private readonly IUserFollowsRepository<Infrastructure.Entities.UserFollow> _productsRepository;

		public UserFollowCreatedEventHandler(IEventSourcingRepository<UserFollows> repository, IUserFollowsRepository<Infrastructure.Entities.UserFollow> productsRepository)
		{
			_productsRepository = productsRepository ?? throw new ArgumentNullException(nameof(repository));
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		public async Task<CommandResult> Handle(UserFollowCreateCommand command, CancellationToken cancellationToken)
		{
			try
			{
				// Проверяем, есть ли уже лайк для данного пользователя и продукта
				bool isAlreadyLiked = await _productsRepository.IsUserFollowsAsync(command.FollowerId, command.FollowedId);

				if (isAlreadyLiked)
				{
					// Если лайк существует, удаляем его
					var entity = new UserFollows(command.FollowerId, command.FollowedId);
					await _productsRepository.UnFollowUser(command.FollowerId, command.FollowedId);
					await _repository.SaveAsync(entity);
					return CommandResult.Success("User unfollow successfully.");
				}
				else
				{
					// Если лайка нет, создаем новый
					var entity = new UserFollows(command.FollowerId, command.FollowedId);
					await _productsRepository.FollowUser(command.FollowerId, command.FollowedId);
					await _repository.SaveAsync(entity);
					return CommandResult.Success(entity.FollowerId);
				}
			}
			catch (DomainException ex)
			{
				return CommandResult.BusinessFail(ex.Message);
			}
		}
	}
}
