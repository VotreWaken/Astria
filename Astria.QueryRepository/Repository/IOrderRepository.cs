using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Astria.QueryRepository.Repository
{
	public interface IOrderRepository<T>
	{
		Task<bool> IsOrderConfirmedByUserAsync(Guid userId, Guid productId);
	}
}
