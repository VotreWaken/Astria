using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Astria.QueryRepository.Repository
{
	public interface IQueryRepository<T>
	{
		Task<IEnumerable<T>> FindAllAsync();
		Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> predicate);
		Task<T> FindByIdAsync(Guid id);
	}
}
