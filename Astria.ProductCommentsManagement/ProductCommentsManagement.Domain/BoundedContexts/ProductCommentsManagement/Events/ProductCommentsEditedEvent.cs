using Astria.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCommentsManagement.Domain.BoundedContexts.ProductCommentsManagement.Events
{
	public class ProductCommentsEditedEvent : DomainEvent
	{
		public Guid Id { get; }
		public string NewCommentText { get; }
		public DateTime UpdatedAt { get; }

		public ProductCommentsEditedEvent(Guid id, string newCommentText, DateTime updatedAt)
		{
			Id = id;
			NewCommentText = newCommentText;
			UpdatedAt = updatedAt;
		}
	}
}
