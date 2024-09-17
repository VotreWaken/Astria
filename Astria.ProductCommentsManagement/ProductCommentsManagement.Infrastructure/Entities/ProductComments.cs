using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCommentsManagement.Infrastructure.Entities
{
	public class ProductComments
	{
		[Key]
		[Required]
		[StringLength(36)]
		public Guid CommentId { get; set; }

		[Required]
		[StringLength(36)]
		public Guid ProductId { get; set; }

		[Required]
		[StringLength(36)]
		public Guid UserId { get; set; }

		[Required]
		public string CommentText { get; set; }

		public Guid? ParentCommentId { get; set; }

		[Required]
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

		public DateTime? UpdatedAt { get; set; }

		[Required]
		public bool IsDeleted { get; set; } = false;
	}
}
