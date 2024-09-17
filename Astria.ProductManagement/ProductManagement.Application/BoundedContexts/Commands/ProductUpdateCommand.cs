using MediatR;
using ProductManagement.Application.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Application.BoundedContexts.Commands
{
	public class ProductUpdateCommand : IRequest<CommandResult>
	{
		public Guid ProductId { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public decimal? Price { get; set; }
	}
}
