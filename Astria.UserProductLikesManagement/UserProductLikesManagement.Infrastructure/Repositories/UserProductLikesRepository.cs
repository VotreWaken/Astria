using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserProductLikesManagement.Infrastructure.DataContext;
using UserProductLikesManagement.Infrastructure.Entities;

namespace UserProductLikesManagement.Infrastructure.Repositories
{
	public class UserProductLikesRepository : IUserProductLikesRepository<UserProductLike>
	{
		private readonly UserProductLikesDbContext _context;

		public UserProductLikesRepository(UserProductLikesDbContext context)
		{
			_context = context;
		}

		public async Task<bool> IsProductLikedByUserAsync(Guid userId, Guid productId)
		{
			return await _context.UserProductLikes.AnyAsync(upl => upl.UserId == userId && upl.ProductId == productId);
		}

		public async Task<bool> LikeProductAsync(Guid userId, Guid productId)
		{
			if (await _context.UserProductLikes.AnyAsync(upl => upl.UserId == userId && upl.ProductId == productId))
			{
				return false;
			}

			var like = new UserProductLike
			{
				UserId = userId,
				ProductId = productId
			};

			_context.UserProductLikes.Add(like);
			await _context.SaveChangesAsync();
			return true;
		}

		public async Task<bool> UnlikeProductAsync(Guid userId, Guid productId)
		{
			var like = await _context.UserProductLikes.FirstOrDefaultAsync(upl => upl.UserId == userId && upl.ProductId == productId);

			if (like == null)
			{
				return false; // Лайк не существует
			}

			_context.UserProductLikes.Remove(like);
			await _context.SaveChangesAsync();
			return true;
		}

		public async Task<List<Guid>> GetLikedProductIdsAsync(Guid userId)
		{
			return await _context.UserProductLikes
				.Where(upl => upl.UserId == userId)
				.Select(upl => upl.ProductId)
				.ToListAsync();
		}

		// Get All Images
		public async Task<List<UserProductLike>> GetAll()
		{
			return await _context.UserProductLikes.ToListAsync();
		}

		// Get Image By Id
		public async Task<UserProductLike> GetById(Guid id)
		{
			var image = await _context.UserProductLikes.FindAsync(id);
			if (image == null)
			{
				throw new Exception($"User Product Like with ID {id} not found");
			}

			return image;
		}

		// Create Image
		public async Task<UserProductLike> Create(UserProductLike image)
		{
			/*
			var existingImage = await _context.UserProductLikes.FirstOrDefaultAsync(c => c.TextureUrl == image.TextureUrl);
			if (existingImage != null)
			{
				throw new InvalidOperationException("Image with the same name already exists");
			}
			*/

			_context.UserProductLikes.Add(image);
			await _context.SaveChangesAsync();
			return image;
		}

		// Delete Image
		public async Task Delete(Guid id)
		{
			var image = await _context.UserProductLikes.FindAsync(id);
			if (image != null)
			{
				_context.UserProductLikes.Remove(image);
				await _context.SaveChangesAsync();
			}
		}
	}
}
