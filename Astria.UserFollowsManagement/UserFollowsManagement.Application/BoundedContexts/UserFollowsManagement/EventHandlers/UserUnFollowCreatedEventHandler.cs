using Astria.EventSourcingRepository.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserFollowsManagement.Application.BoundedContexts.UserFollowsManagement.Commands;
using UserFollowsManagement.Application.Results;
using UserFollowsManagement.Domain.BoundedContexts.UserFollowsManagement.Aggregates;
using UserFollowsManagement.Domain.Exceptions;
using UserFollowsManagement.Infrastructure.Repositories;

namespace UserFollowsManagement.Application.BoundedContexts.UserFollowsManagement.EventHandlers
{
	public class UserUnFollowCreatedEventHandler : IRequestHandler<UserUnFollowsCreatedCommand, CommandResult>
	{
		private readonly IEventSourcingRepository<UserFollows> _repository;

		private readonly IUserFollowsRepository<Infrastructure.Entities.UserFollow> _productsRepository;

		public UserUnFollowCreatedEventHandler(IEventSourcingRepository<UserFollows> repository, IUserFollowsRepository<Infrastructure.Entities.UserFollow> productsRepository)
		{
			_productsRepository = productsRepository ?? throw new ArgumentNullException(nameof(repository));
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		public async Task<CommandResult> Handle(UserUnFollowsCreatedCommand command, CancellationToken cancellationToken)
		{
			try
			{
				var entity = new UserFollows(command.FollowerId, command.FollowedId);

				await _productsRepository.UnFollowUser(command.FollowerId, command.FollowedId);
				entity.DeleteOrder(command.FollowerId, command.FollowedId);
				await _repository.SaveAsync(entity);
				return CommandResult.Success(entity.FollowerId);
			}
			catch (DomainException ex)
			{
				return CommandResult.BusinessFail(ex.Message);
			}
		}
	}
}
