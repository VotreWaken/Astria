using MediatR;
using ProductCommentsManagement.Application.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCommentsManagement.Application.BoundedContexts.ProductCommentsManagement.Commands
{
	public class ProductCommentsDeleteCommand : IRequest<CommandResult>
	{
		public Guid Id { get; set; }
	}
}
