using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Astria.QueryRepository.Repository
{
	public interface IProductRepository<T>
	{
		Task<T> FindByIdAsync(Guid id);
		Task IncrementFieldAsync(Guid id, string fieldName);
	}
}
