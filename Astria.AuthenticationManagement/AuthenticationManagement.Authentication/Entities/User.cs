using Microsoft.AspNetCore.Identity;

namespace AuthenticationManagement.Authentication.Entities
{
	public class User : IdentityUser<Guid>
	{
		public bool UnConfirmedEmail { get; set; }
		public Guid UserImageId { get; set; }
	}
}
