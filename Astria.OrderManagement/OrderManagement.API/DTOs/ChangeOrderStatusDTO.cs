using OrderManagement.Domain.BoundedContexts.OrderManagment.ValueObjects;

namespace OrderManagement.API.DTOs
{
	public class ChangeOrderStatusDTO
	{
		public Guid Id { get; set; }
		public string Status { get; set; }
	}
}
