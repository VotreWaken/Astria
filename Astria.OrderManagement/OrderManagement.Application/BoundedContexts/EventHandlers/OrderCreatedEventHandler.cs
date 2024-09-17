using Astria.EventSourcingRepository.Repository;

using MediatR;
using OrderManagement.Application.BoundedContexts.Commands;
using OrderManagement.Application.Results;
using OrderManagement.Domain.BoundedContexts.OrderManagment.Aggregates;
using OrderManagement.Domain.Exceptions;
using OrderManagement.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.BoundedContexts.EventHandlers
{
	public class OrderCreateCommandHandler : IRequestHandler<OrderCreateCommand, CommandResult>
	{
		private readonly IEventSourcingRepository<OrderManagement.Domain.BoundedContexts.OrderManagment.Aggregates.Order> _repository;
		private readonly IOrderRepository _productsRepository;

		public OrderCreateCommandHandler(IEventSourcingRepository<OrderManagement.Domain.BoundedContexts.OrderManagment.Aggregates.Order> repository, IOrderRepository productsRepository)
		{
			_productsRepository = productsRepository ?? throw new ArgumentNullException(nameof(repository));
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		public async Task<CommandResult> Handle(OrderCreateCommand command, CancellationToken cancellationToken)
		{
			try
			{
				var orderId = Guid.NewGuid();
				var order = new OrderManagement.Domain.BoundedContexts.OrderManagment.
					Aggregates.Order(orderId, command.CustomerId, command.ProductId, 
					command.OrderDate, command.OrderAmount, command.Status);

				var entity = new Infrastructure.Entities.Order
				{
					Id = orderId,
					ProductId = orderId,
					CustomerId = command.CustomerId,
					OrderAmount = command.OrderAmount,
					OrderDate = command.OrderDate,
					Status = command.Status,
				};

				await _productsRepository.CreateOrder(entity);

				await _repository.SaveAsync(order);
				return CommandResult.Success(entity, orderId);
			}
			catch (DomainException ex)
			{
				return CommandResult.BusinessFail(ex.Message);
			}
		}
	}
}
