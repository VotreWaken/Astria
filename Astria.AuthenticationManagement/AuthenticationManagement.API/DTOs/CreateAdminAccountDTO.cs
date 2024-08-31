using System.ComponentModel.DataAnnotations;

namespace AuthenticationManagement.API.DTOs
{
	public class CreateAdminAccountDTO
	{
		[Required(ErrorMessage = "Email is required.")]
		[EmailAddress(ErrorMessage = "Invalid email format.")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Password is required.")]
		[StringLength(20, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 8)]
		[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[A-Za-z\d]{8,}$", ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number, and be at least 8 characters long.")]
		public string Password { get; set; }

		[Required(ErrorMessage = "First name is required.")]
		public string Firstname { get; set; }

		[Required(ErrorMessage = "Last name is required.")]
		public string Surname { get; set; }

		[Required(ErrorMessage = "Secret product key is required.")]
		public string SecretProductKey { get; set; }

		[Required(ErrorMessage = "User image is required.")]
		public IFormFile UserImage { get; set; }
	}
}
