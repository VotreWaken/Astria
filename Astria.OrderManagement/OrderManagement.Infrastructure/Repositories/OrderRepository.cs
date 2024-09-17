using OrderManagement.Infrastructure.DataContext;
using OrderManagement.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Domain.BoundedContexts.OrderManagment.Events;
using OrderManagement.Domain.BoundedContexts.OrderManagment.ValueObjects;

namespace OrderManagement.Infrastructure.Repositories
{
	public class OrderRepository : IOrderRepository
	{
		private readonly OrderDbContext _context;

		public OrderRepository(OrderDbContext context)
		{
			_context = context;
		}

		public async Task CreateOrder(Order order)
		{
			await _context.Orders.AddAsync(order);
			await _context.SaveChangesAsync();
		}

		public async Task<Order> GetOrderById(Guid orderId)
		{
			return await _context.Orders.FindAsync(orderId);
		}

		public async Task<IEnumerable<Order>> GetOrdersByUserId(Guid userId)
		{
			return await _context.Orders.Where(o => o.CustomerId == userId).ToListAsync();
		}

		public async Task UpdateOrder(Order order)
		{
			_context.Orders.Update(order);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteOrder(Guid orderId)
		{
			var order = await _context.Orders.FindAsync(orderId);
			if (order != null)
			{
				_context.Orders.Remove(order);
				await _context.SaveChangesAsync();
			}
		}

		public async Task ConfirmOrder(Guid orderId)
		{
			var order = await _context.Orders.FindAsync(orderId);
			if (order != null)
			{
				order.Status = OrderStatus.Confirmed;

				// Сохраняем изменения в базе данных
				_context.Orders.Update(order);
				await _context.SaveChangesAsync();
			}
		}
	}
}
