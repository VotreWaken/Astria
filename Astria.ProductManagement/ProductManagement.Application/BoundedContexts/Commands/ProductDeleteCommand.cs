using MediatR;
using ProductManagement.Application.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Application.BoundedContexts.Commands
{
	public class ProductDeleteCommand : IRequest<CommandResult>
	{
		public Guid Id { get; set; }

		public ProductDeleteCommand(Guid id)
		{
			Id = id;
		}
	}
}
