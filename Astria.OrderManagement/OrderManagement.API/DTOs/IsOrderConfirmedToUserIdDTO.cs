namespace OrderManagement.API.DTOs
{
	public class IsOrderConfirmedToUserIdDTO
	{
		public Guid Id { get; set; }
		public Guid CustomerId { get; set; }
	}
}
