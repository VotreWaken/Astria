using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserFollowsManagement.Infrastructure.DataContext;
using UserFollowsManagement.Infrastructure.Entities;

namespace UserFollowsManagement.Infrastructure.Repositories
{
	public class UserFollowsRepository : IUserFollowsRepository<UserFollow>
	{
		private readonly UserFollowsDbContext _context;

		public UserFollowsRepository(UserFollowsDbContext context)
		{
			_context = context;
		}

		public async Task<bool> IsUserFollowsAsync(Guid followerId, Guid followedId)
		{
			return await _context.UserFollows.AnyAsync(upl => upl.FollowerId == followerId && upl.FollowedId == followedId);
		}

		public async Task<bool> FollowUser(Guid userId, Guid followedId)
		{
			var existingFollow = await _context.UserFollows
				.FirstOrDefaultAsync(uf => uf.FollowerId == userId && uf.FollowedId == followedId);

			if (existingFollow != null)
			{
				return false;
			}

			var follow = new UserFollow
			{
				FollowerId = userId,
				FollowedId = followedId,
			};

			_context.UserFollows.Add(follow);
			await _context.SaveChangesAsync();

			return true;
		}

		public async Task<bool> UnFollowUser(Guid userId, Guid followedId)
		{
			var follow = await _context.UserFollows
				.FirstOrDefaultAsync(uf => uf.FollowerId == userId && uf.FollowedId == followedId);

			if (follow == null)
			{
				return false;
			}

			_context.UserFollows.Remove(follow);
			await _context.SaveChangesAsync();

			return true;
		}

		public async Task<List<Guid>> GetUserFollows(Guid userId)
		{
			return await _context.UserFollows
				.Where(uf => uf.FollowerId == userId)
				.Select(uf => uf.FollowedId)
				.ToListAsync();
		}
	}
}
