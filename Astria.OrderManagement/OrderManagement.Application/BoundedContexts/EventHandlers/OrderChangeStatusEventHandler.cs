using Astria.EventSourcingRepository.Repository;
using MediatR;
using OrderManagement.Application.BoundedContexts.Commands;
using OrderManagement.Application.Results;
using OrderManagement.Domain.BoundedContexts.OrderManagment.ValueObjects;
using OrderManagement.Domain.Exceptions;
using OrderManagement.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.BoundedContexts.EventHandlers
{
	public class OrderChangeStatusEventHandler : IRequestHandler<OrderChangeStatusCommand, CommandResult>
	{
		private readonly IEventSourcingRepository<OrderManagement.Domain.BoundedContexts.OrderManagment.Aggregates.Order> _repository;
		private readonly IOrderRepository _productsRepository;

		public OrderChangeStatusEventHandler(
			IEventSourcingRepository<OrderManagement.Domain.BoundedContexts.OrderManagment.Aggregates.Order> repository,
			IOrderRepository productsRepository)
		{
			_productsRepository = productsRepository ?? throw new ArgumentNullException(nameof(productsRepository));
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		public async Task<CommandResult> Handle(OrderChangeStatusCommand command, CancellationToken cancellationToken)
		{
			try
			{
				var product = await _productsRepository.GetOrderById(command.OrderId);
				if (product == null)
				{
					// return CommandResult.NotFound($"Product with ID {command.ProductId} not found.");
				}


				product.Status = command.Status;


				var ent = new Domain.BoundedContexts.OrderManagment.Aggregates.Order();
				ent.ChangeOrderStatus(product.Id, product.Status);

				// Вместо Confirm Change Status делать 

				await _productsRepository.UpdateOrder(product);
				await _repository.SaveAsync(ent);

				return CommandResult.Success(product, command.OrderId);
			}
			catch (DomainException ex)
			{
				return CommandResult.BusinessFail(ex.Message);
			}
		}
	}
}
