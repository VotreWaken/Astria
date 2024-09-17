// using OrderManagement.Infrastructure.Entities;

// using OrderManagement.Domain.BoundedContexts.OrderManagment.Aggregates;

using OrderManagement.Infrastructure.Entities;

namespace OrderManagement.Infrastructure.Repositories
{
	public interface IOrderRepository
	{
		Task CreateOrder(Order order);
		Task<Order> GetOrderById(Guid orderId);
		Task<IEnumerable<Order>> GetOrdersByUserId(Guid userId);
		Task UpdateOrder(Order order);
		Task DeleteOrder(Guid orderId);

		Task ConfirmOrder(Guid orderId);
	}
}
