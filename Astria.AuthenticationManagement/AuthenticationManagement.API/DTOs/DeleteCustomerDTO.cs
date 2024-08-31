namespace AuthenticationManagement.API.DTOs
{
	public class DeleteCustomerDTO
	{
		public Guid CustomerId { get; set; }
		public string CurrentPassword { get; set; }
	}
}
