using MediatR;
using OrderManagement.Application.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.BoundedContexts.Commands
{
	public class OrderDeleteCommand : IRequest<CommandResult>
	{
		public Guid Id { get; set; }

		public OrderDeleteCommand(Guid id)
		{
			Id = id;
		}
	}
}
