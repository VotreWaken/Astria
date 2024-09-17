using AuthenticationManagement.Authentication.Configuration;
using AuthenticationManagement.Authentication.Entities;
using AuthenticationManagement.Authentication.Services;
using AuthenticationManagement.Authentication.Exceptions;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationManagement.Authentication.Repository
{
	public class AuthenticationRepository : IAuthenticationRepository
	{
		private readonly UserManager<User> _userManager;
		private readonly RoleManager<Role> _roleManager;
		private readonly ITokenService _tokenService;

		public AuthenticationRepository(UserManager<User> userManager, RoleManager<Role> roleManager, ITokenService tokenService)
		{
			_userManager = userManager;
			_roleManager = roleManager;
			_tokenService = tokenService;
		}

		public async Task CreateAdmin(Guid userId, string email, string hashedPassword, Guid UserImageId)
		{
			IdentityResult userCreateResult = await _userManager.CreateAsync(new User
			{

				UserName = email,
				Id = userId,
				Email = email,
				PasswordHash = hashedPassword,
				UserImageId = UserImageId,
			});

			if (!userCreateResult.Succeeded)
				throw new UserCannotBeCreatedException(email);

			Role matchingRole = await _roleManager.FindByNameAsync(FixedRoles.AdminRole);
			if (matchingRole is null)
				throw new RoleCannotBeFoundException(FixedRoles.AdminRole);

			User matchingUser = await _userManager.FindByEmailAsync(email);
			if (matchingUser is null)
				throw new UserCannotBeFoundException(email);


			IdentityResult roleAssignResult = await _userManager.AddToRoleAsync(matchingUser, matchingRole.Name);
			if (!roleAssignResult.Succeeded)
				throw new RoleCannotBeAssignedException(email);
		}

		public async Task CreateCustomer(Guid userId, string email, string hashedPassword, Guid userImageId)
		{
			IdentityResult userCreateResult = await _userManager.CreateAsync(new User
			{
				UserName = email,
				Id = userId,
				Email = email,
				PasswordHash = hashedPassword,
				UserImageId = userImageId,
				UnConfirmedEmail = false,
			});
			Console.WriteLine("Auth Repository User Image Id: " + userImageId);
			if (!userCreateResult.Succeeded)
				throw new UserCannotBeCreatedException(email);

			User matchingUser = await _userManager.FindByEmailAsync(email);
			if (matchingUser is null)
				throw new UserCannotBeFoundException(email);

			Role matchingRole = await _roleManager.FindByNameAsync(FixedRoles.CustomerRole);
			if (matchingRole is null)
				throw new RoleCannotBeFoundException(FixedRoles.CustomerRole);

			IdentityResult roleAssignResult = await _userManager.AddToRoleAsync(matchingUser, matchingRole.Name);
			if (!roleAssignResult.Succeeded)
				throw new RoleCannotBeAssignedException(email);
		}

		public async Task<string> GenerateEmailValidationToken(Guid userId)
		{
			User matchingUser = await _userManager.FindByIdAsync(userId.ToString());
			if (matchingUser is null)
				throw new UserCannotBeFoundException(userId);

			return await _userManager.GenerateEmailConfirmationTokenAsync(matchingUser);
		}

		public async Task<string> GenerateChangeEmailValidationToken(Guid userId, string email)
		{
			User matchingUser = await _userManager.FindByIdAsync(userId.ToString());
			if (matchingUser is null)
				throw new UserCannotBeFoundException(userId);

			matchingUser.EmailConfirmed = false;
			matchingUser.UnConfirmedEmail = false;
			await _userManager.UpdateAsync(matchingUser);

			return await _userManager.GenerateChangeEmailTokenAsync(matchingUser, email);
		}

		public async Task<string> GenerateHashedPassword(Guid userId, string password)
		{
			User? matchingUser = userId == Guid.Empty
				? new User()
				: await _userManager.FindByIdAsync(userId.ToString());

			if (matchingUser is null)
				throw new UserCannotBeFoundException(userId);

			var ph = new PasswordHasher<User>();
			return ph.HashPassword(matchingUser, password);
		}

		public async Task<string> GenerateJWT(string email, string password)
		{
			User matchingUser = await _userManager.FindByEmailAsync(email);

			if (matchingUser is null)
				throw new InvalidAuthenticationException();

			if (await _userManager.CheckPasswordAsync(matchingUser, password) is false)
				throw new InvalidAuthenticationException();

			var roles = await _userManager.GetRolesAsync(matchingUser);

			return _tokenService.GenerateJwt(matchingUser, roles);
		}


		public async Task<bool> EmailAddressInUse(string email)
			=> await _userManager.FindByEmailAsync(email) != null;

		public async Task<bool> UserHasRole(Guid userId, string role)
		{
			User matchingUser = await _userManager.FindByIdAsync(userId.ToString());
			if (matchingUser is null)
				throw new UserCannotBeFoundException(userId);

			Role matchingRole = await _roleManager.FindByNameAsync(role);
			if (matchingRole is null)
				throw new RoleCannotBeFoundException(FixedRoles.CustomerRole);

			return await _userManager.IsInRoleAsync(matchingUser, matchingRole.Name);
		}

		public async Task<bool> UserExists(Guid userId)
		{
			User matchingUser = await _userManager.FindByIdAsync(userId.ToString());
			return !(matchingUser is null);
		}

		public async Task<bool> IsValidEmailVerificationToken(Guid userId, string verificationToken)
		{
			User matchingUser = await _userManager.FindByIdAsync(userId.ToString());
			if (matchingUser is null)
				throw new UserCannotBeFoundException(userId);

			var result = await _userManager.ConfirmEmailAsync(matchingUser, verificationToken);
			return result.Succeeded;
		}


		public async Task VerifyEmail(Guid userId, string verificationToken)
		{
			User matchingUser = await _userManager.FindByIdAsync(userId.ToString());
			if (matchingUser is null)
				throw new UserCannotBeFoundException(userId);

			var result = await _userManager.ConfirmEmailAsync(matchingUser, verificationToken);
			if (!result.Succeeded)
				throw new InvalidAuthenticationException();

			if (matchingUser.UnConfirmedEmail)
			{
				matchingUser.Email = matchingUser.UnConfirmedEmail.ToString();
				matchingUser.UserName = matchingUser.UnConfirmedEmail.ToString();
				matchingUser.UnConfirmedEmail = true;
			}

			matchingUser.EmailConfirmed = true;

			await _userManager.UpdateAsync(matchingUser);
		}

		public async Task ChangePassword(Guid userId, string hashedPassword)
		{
			User matchingUser = await _userManager.FindByIdAsync(userId.ToString());
			if (matchingUser is null)
				throw new UserCannotBeFoundException(userId);

			matchingUser.PasswordHash = hashedPassword;
			await _userManager.UpdateAsync(matchingUser);
		}

		public async Task ChangeEmail(Guid userId, string newEmail)
		{
			User matchingUser = await _userManager.FindByIdAsync(userId.ToString());

			if (matchingUser is null)
				throw new UserCannotBeFoundException(userId);

			matchingUser.Email = newEmail;
			matchingUser.EmailConfirmed = false;
			await _userManager.UpdateAsync(matchingUser);
		}

		public async Task ChangeUserName(Guid userId, string userFirstName, string userLastName)
		{
			User matchingUser = await _userManager.FindByIdAsync(userId.ToString());

			if (matchingUser is null)
				throw new UserCannotBeFoundException(userId);

			matchingUser.UserName = userFirstName + userLastName;
			await _userManager.UpdateAsync(matchingUser);
		}

		public async Task<bool> IsCurrentPassword(Guid userId, string password)
		{
			User matchingUser = await _userManager.FindByIdAsync(userId.ToString());
			if (matchingUser is null)
				throw new UserCannotBeFoundException(userId);

			return await _userManager.CheckPasswordAsync(matchingUser, password);
		}

		public async Task DeleteUser(Guid userId)
		{
			User matchingUser = await _userManager.FindByIdAsync(userId.ToString());
			if (matchingUser is null)
				throw new UserCannotBeFoundException(userId);

			await _userManager.DeleteAsync(matchingUser);
		}
	}
}
