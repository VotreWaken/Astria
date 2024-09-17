using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserProductLikesManagement.Infrastructure.Repositories
{
	public interface IUserProductLikesRepository<T> where T : class
	{
		Task<List<Guid>> GetLikedProductIdsAsync(Guid userId);
		Task<bool> LikeProductAsync(Guid userId, Guid productId);
		Task<bool> UnlikeProductAsync(Guid userId, Guid productId);

		Task<bool> IsProductLikedByUserAsync(Guid userId, Guid productId);

		// Not Used 
		Task<List<T>> GetAll();
		Task<T> GetById(Guid id);
		Task<T> Create(T image);
		Task Delete(Guid id);
	}
}
