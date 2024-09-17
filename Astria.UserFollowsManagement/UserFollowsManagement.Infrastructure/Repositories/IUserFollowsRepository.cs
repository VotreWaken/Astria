using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserFollowsManagement.Infrastructure.Repositories
{
	public interface IUserFollowsRepository<T> where T : class
	{
		Task<bool> FollowUser(Guid userId, Guid followedId);
		Task<bool> UnFollowUser(Guid userId, Guid followedId);
		Task<List<Guid>> GetUserFollows(Guid userId);

		Task<bool> IsUserFollowsAsync(Guid followerId, Guid followedId);
	}
}
