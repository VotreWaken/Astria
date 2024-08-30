using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astria.QueryRepository.Repository
{
	public interface IProjectionRepository<T> : IQueryRepository<T>
	{
		Task InsertAsync(T entity);
		Task UpdateAsync(T entity);
		Task DeleteAsync(Guid Id);
	}
}
