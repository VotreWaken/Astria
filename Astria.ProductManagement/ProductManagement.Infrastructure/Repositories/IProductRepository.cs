using ProductManagement.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Infrastructure.Repositories
{
	public interface IProductRepository
	{
		Task CreateProduct(Product order);
		Task<Product> GetProductById(Guid orderId);
		Task<IEnumerable<Product>> GetProductsByUserId(Guid userId);
		Task UpdateProduct(Product order);
		Task DeleteProduct(Guid orderId);
	}
}
