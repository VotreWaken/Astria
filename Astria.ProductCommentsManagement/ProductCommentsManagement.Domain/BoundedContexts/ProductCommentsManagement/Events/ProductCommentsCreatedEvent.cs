using Astria.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCommentsManagement.Domain.BoundedContexts.ProductCommentsManagement.Events
{
	public class ProductCommentsCreatedEvent : DomainEvent
	{
		public Guid Id { get; }
		public Guid ProductId { get; }
		public Guid UserId { get; }
		public string CommentText { get; }
		public Guid ParentCommentId { get; }
		public DateTime CreatedAt { get; }

		public ProductCommentsCreatedEvent(Guid id, Guid productId, Guid userId, string commentText, Guid parentCommentId, DateTime createdAt)
		{
			Id = id;
			ProductId = productId;
			UserId = userId;
			CommentText = commentText;
			ParentCommentId = parentCommentId;
			CreatedAt = createdAt;
		}
	}
}
