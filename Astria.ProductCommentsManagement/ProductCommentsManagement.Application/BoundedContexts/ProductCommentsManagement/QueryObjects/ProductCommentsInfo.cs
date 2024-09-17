using Astria.QueryRepository.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCommentsManagement.Application.BoundedContexts.ProductCommentsManagement.QueryObjects
{
	public class ProductCommentsInfo : IQueryEntity
	{
		public Guid Id { get; set; }

		public Guid ProductId { get; set; }

		public Guid UserId { get; set; }

		public string CommentText { get; set; }

		public Guid? ParentCommentId { get; set; }

		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

		public DateTime? UpdatedAt { get; set; }

		public bool IsDeleted { get; set; } = false;
	}
}
