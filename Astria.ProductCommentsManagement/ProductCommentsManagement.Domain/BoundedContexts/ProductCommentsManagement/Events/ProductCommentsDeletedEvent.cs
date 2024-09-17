using Astria.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCommentsManagement.Domain.BoundedContexts.ProductCommentsManagement.Events
{
	public class ProductCommentsDeletedEvent : DomainEvent
	{
		public Guid Id { get; }
		public DateTime DeletedAt { get; }

		public ProductCommentsDeletedEvent(Guid id, DateTime deletedAt)
		{
			Id = id;
			DeletedAt = deletedAt;
			Console.WriteLine("ProductCommentDeletedEvent Constructor");

		}
	}
}
