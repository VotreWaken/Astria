using MediatR;
using OrderManagement.Application.Results;
using OrderManagement.Domain.BoundedContexts.OrderManagment.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.BoundedContexts.Commands
{
	public class OrderChangeStatusCommand : IRequest<CommandResult>
	{
		public Guid OrderId { get; set; }
		public OrderStatus Status { get; set; }
	}
}
