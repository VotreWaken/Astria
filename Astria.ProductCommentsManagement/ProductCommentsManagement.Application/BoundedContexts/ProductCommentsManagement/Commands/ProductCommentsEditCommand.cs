using MediatR;
using ProductCommentsManagement.Application.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCommentsManagement.Application.BoundedContexts.ProductCommentsManagement.Commands
{
	public class ProductCommentsEditCommand : IRequest<CommandResult>
	{
		public Guid Id { get; set; }

		public string CommentText { get; set; }

		public DateTime? UpdatedAt { get; set; }
	}
}
