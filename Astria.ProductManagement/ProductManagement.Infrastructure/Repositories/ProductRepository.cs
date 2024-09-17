using Microsoft.EntityFrameworkCore;
using ProductManagement.Infrastructure.DataContext;
using ProductManagement.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Infrastructure.Repositories
{
	public class ProductRepository : IProductRepository
	{
		private readonly ProductDbContext _context;

		public ProductRepository(ProductDbContext context)
		{
			_context = context;
		}

		public async Task CreateProduct(Product product)
		{
			await _context.Products.AddAsync(product);
			await _context.SaveChangesAsync();
		}

		public async Task<Product> GetProductById(Guid productId)
		{
			return await _context.Products.FindAsync(productId);
		}

		public async Task<IEnumerable<Product>> GetProductsByUserId(Guid userId)
		{
			return await _context.Products.Where(o => o.ProductId == userId).ToListAsync();
		}

		public async Task UpdateProduct(Product product)
		{
			_context.Products.Update(product);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteProduct(Guid ProductId)
		{
			var order = await _context.Products.FindAsync(ProductId);
			if (order != null)
			{
				_context.Products.Remove(order);
				await _context.SaveChangesAsync();
			}
		}
	}
}
