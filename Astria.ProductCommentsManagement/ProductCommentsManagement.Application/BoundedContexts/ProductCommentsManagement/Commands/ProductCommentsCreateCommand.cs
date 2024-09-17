using MediatR;
using ProductCommentsManagement.Application.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCommentsManagement.Application.BoundedContexts.ProductCommentsManagement.Commands
{
	public class ProductCommentsCreateCommand : IRequest<CommandResult>
	{
		public Guid Id { get; set; }

		public Guid ProductId { get; set; }

		public Guid UserId { get; set; }

		public string CommentText { get; set; }

		public Guid ParentCommentId { get; set; }

		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

		public DateTime? UpdatedAt { get; set; }

		public bool IsDeleted { get; set; } = false;
	}
}
