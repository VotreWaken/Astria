using OrderManagement.Domain.BoundedContexts.OrderManagment.ValueObjects;

namespace OrderManagement.API.DTOs
{
	public class CreateOrderDTO
	{
		public Guid Id { get; set; }
		public Guid CustomerId { get; set; }
		public Guid ProductId { get; set; }
		public DateTime OrderDate { get; set; }
		public decimal OrderAmount { get; set; }
		public string Status { get; set; }
	}
}
