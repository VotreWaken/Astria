namespace AuthenticationManagement.API.DTOs
{
	public class UpdateAdminImageDTO
	{
		public Guid Id { get; set; }
		public IFormFile UserImage { get; set; }
	}
}
