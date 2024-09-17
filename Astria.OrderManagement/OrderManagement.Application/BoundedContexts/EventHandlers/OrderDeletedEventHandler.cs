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
	public class OrderDeletedEventHandler : IRequestHandler<OrderDeleteCommand, CommandResult>
	{
		private readonly IEventSourcingRepository<Order> _repository;
		private readonly IOrderRepository _productsRepository;

		public OrderDeletedEventHandler(IEventSourcingRepository<Order> repository, IOrderRepository productsRepository)
		{
			_productsRepository = productsRepository ?? throw new ArgumentNullException(nameof(repository));
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		public async Task<CommandResult> Handle(OrderDeleteCommand command, CancellationToken cancellationToken)
		{
			try
			{
				// Тут вызывать Event у Order 

				// admin.ChangeEmail(command.NewEmail);
				// В Celestial.Application -> Commands -> AdminAccountChangeEmailCommand
				var product = await _repository.FindByIdAsync(command.Id);
				Console.WriteLine("Deleted: " + command.Id);
				// product.DeleteOrder(command.Id);

				await _productsRepository.DeleteOrder(command.Id);

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
