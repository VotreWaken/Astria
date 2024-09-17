using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Astria.QueryRepository.Repository
{
	public interface IUserProductLikeRepository<T>
	{
		Task<int> CountAsync(Expression<Func<T, bool>> predicate);

		Task<bool> IsProductLikedByUserAsync(Guid userId, Guid productId);
	}
}
