namespace AuthenticationManagement.API.DTOs
{
	public class CreateCustomerAccountDTO
	{
		public string Email { get; set; }
		public string Password { get; set; }
		public string Firstname { get; set; }
		public string Surname { get; set; }
		public IFormFile UserImage { get; set; }

	}
}
