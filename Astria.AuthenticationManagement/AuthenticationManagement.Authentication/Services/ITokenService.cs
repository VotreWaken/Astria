using AuthenticationManagement.Authentication.Entities;

namespace AuthenticationManagement.Authentication.Services
{
	public interface ITokenService
	{
		string GenerateJwt(User user, IList<string> roles);
	}
}
