using Microsoft.EntityFrameworkCore;
using ProductCommentsManagement.Infrastructure.DataContext;
using ProductCommentsManagement.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCommentsManagement.Infrastructure.Repositories
{
	public class ProductCommentsRepository : IProductCommentsRepository<ProductComments>
	{
		private readonly ProductCommentsDbContext _context;

		public ProductCommentsRepository(ProductCommentsDbContext context)
		{
			_context = context;
		}

		public async Task<List<Guid>> GetAllComments(Guid productId)
		{
			return await _context.ProductComments
				.Where(c => c.ProductId == productId && !c.IsDeleted)
				.Select(c => c.CommentId)
				.ToListAsync();
		}

		public async Task<ProductComments> FindByIdAsync(Guid commentId)
		{
			return await _context.ProductComments
				.Where(c => c.CommentId == commentId && !c.IsDeleted)
				.FirstOrDefaultAsync();
		}

		public async Task CreateComment(Guid userId, Guid ProductId, string message)
		{
			var comment = new ProductComments
			{
				CommentId = Guid.NewGuid(),
				ProductId = ProductId,
				UserId = userId,
				CommentText = message,
				CreatedAt = DateTime.UtcNow,
				IsDeleted = false
			};

			_context.ProductComments.Add(comment);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteComment(Guid messageId)
		{
			var comment = await _context.ProductComments
				.FirstOrDefaultAsync(c => c.CommentId == messageId);

			if (comment != null)
			{
				comment.IsDeleted = true;
				comment.UpdatedAt = DateTime.UtcNow;
				await _context.SaveChangesAsync();
			}
		}

		public async Task EditComment(Guid messageId, string newMessage)
		{
			var comment = await _context.ProductComments
				.FirstOrDefaultAsync(c => c.CommentId == messageId && !c.IsDeleted);

			if (comment != null)
			{
				comment.CommentText = newMessage;
				comment.UpdatedAt = DateTime.UtcNow;
				await _context.SaveChangesAsync();
			}
		}
	}
}
