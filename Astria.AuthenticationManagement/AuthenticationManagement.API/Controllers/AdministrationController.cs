using AuthenticationManagement.Application.BoundedContexts.UserAccountManagement.Queries;
using AuthenticationManagement.Application.BoundedContexts.UserAccountManagement.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Astria.Rabbitmq.Events;
using MassTransit;
using AuthenticationManagement.Application.BoundedContexts.UserAccountManagement.QueryObjects;
using AuthenticationManagement.Application.Results;
using AuthenticationManagement.API.DTOs;

namespace AuthenticationManagement.API.Controllers
{
	[Authorize(Roles = "Admin")]
	public class AdministrationController : ApiController
	{
		private readonly IMediator _mediator;
		private readonly IBus _bus;
		public AdministrationController(IMediator mediator, IBus bus)
		{
			_mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
			_bus = bus ?? throw new ArgumentNullException(nameof(bus));
		}

		[HttpPost]
		[AllowAnonymous]
		[Route("Register")]
		public async Task<IActionResult> RegisterAdmin([FromForm] CreateAdminAccountDTO dto)
		{
			byte[] modelDataBytes;
			using (var memoryStream = new MemoryStream())
			{
				await dto.UserImage.CopyToAsync(memoryStream);
				modelDataBytes = memoryStream.ToArray();
			}

			var userImageId = Guid.NewGuid();

			var command = new AdminAccountCreateCommand
			{
				Email = dto.Email,
				PlainPassword = dto.Password,
				Firstname = dto.Firstname,
				Lastname = dto.Surname,
				SecretProductKey = dto.SecretProductKey,
				UserImageId = userImageId,
			};

			var UserImageProcessingEvent = new UserImageCreatedEvent
			{
				ImageData = modelDataBytes,
				UserImageId = userImageId,
			};

			Console.WriteLine("Create Event With UserImageId: " + userImageId);
			await _bus.Publish(UserImageProcessingEvent);

			CommandResult result = await _mediator.Send(command);
			return result.IsSuccess switch
			{
				true => Ok(),
				false => HandleFailedCommand(result)
			};
		}



		[HttpPost]
		[Route("UpdateAdminImage/{id}")]
		public async Task<IActionResult> UpdateAdminImage([FromForm] UpdateAdminImageDTO dto, Guid id)
		{
			byte[] modelDataBytes;
			using (var memoryStream = new MemoryStream())
			{
				await dto.UserImage.CopyToAsync(memoryStream);
				modelDataBytes = memoryStream.ToArray();
			}

			var command = new AdminAccountUpdateImageCommand
			{
				UserId = dto.Id,
			};

			CommandResult result = await _mediator.Send(command);

			var UserImageProcessingEvent = new UserImageCreatedEvent
			{
				ImageData = modelDataBytes,
				UserImageId = command.UserImageId,
			};

			Console.WriteLine("Create Event With UserImageId: " + command.UserImageId);
			await _bus.Publish(UserImageProcessingEvent);

			return result.IsSuccess switch
			{
				true => Ok(),
				false => HandleFailedCommand(result)
			};
		}

		/*

		[HttpPost]
		[Route("UpdateAdminFirstName/{id}")]
		public async Task<IActionResult> UpdateAdminFirstName([FromForm] UpdateAdminImageDTO dto, Guid id)
		{

		}

		[HttpPost]
		[Route("UpdateAdminLastName/{id}")]
		public async Task<IActionResult> UpdateAdminLastName([FromForm] UpdateAdminImageDTO dto, Guid id)
		{

		}

		[HttpPost]
		[Route("ResetPassword/{id}")]
		public async Task<IActionResult> ResetPassword([FromForm] UpdateAdminImageDTO dto, Guid id)
		{

		}
		*/

		[HttpPost]
		[Route("DeleteAdmin/{id}")]
		public async Task<IActionResult> DeleteAdmin(DeleteCustomerDTO dto)
		{
			var command = new AdminAccountDeleteCommand
			{
				UserId = dto.CustomerId,
				CurrentPassword = dto.CurrentPassword,
			};

			CommandResult result = await _mediator.Send(command);

			return result.IsSuccess switch
			{
				true => Ok(),
				false => HandleFailedCommand(result)
			};
		}

		[HttpGet]
		[Route("{id}")]
		[AllowAnonymous]
		public async Task<IActionResult> GetAdminInfo(Guid id)
		{
			var query = new GetAdminInfoQuery(id);

			AdminInfo result = await _mediator.Send(query);
			return result switch
			{
				not null => Ok(result),
				null => NotFound()
			};
		}

		[HttpPatch]
		[Route("{id}/ChangeEmail")]
		public async Task<IActionResult> ChangeEmail([FromForm] UpdateEmailAddressDTO dto, Guid id)
		{
			var command = new AdminAccountChangeEmailCommand
			{
				// ExpectedVersion = dto.ExpectedVersion,
				UserId = id,
				NewEmail = dto.Email
			};

			CommandResult result = await _mediator.Send(command);
			return result.IsSuccess switch
			{
				true => Ok(),
				false => HandleFailedCommand(result)
			};
		}

		[HttpPatch]
		[Route("{id}/ChangeUserFullName")]
		[AllowAnonymous]
		public async Task<IActionResult> ChangeUserFullName([FromForm] UpdateUserNameDTO dto, Guid id)
		{
			var command = new AdminAccountChangeNameAndSurnameCommand
			{
				// ExpectedVersion = dto.ExpectedVersion,
				UserId = id,
				FirstName = dto.FirstName,
				LastName = dto.LastName
			};

			CommandResult result = await _mediator.Send(command);
			return result.IsSuccess switch
			{
				true => Ok(),
				false => HandleFailedCommand(result)
			};
		}

		[HttpPatch]
		[Route("{id}/ChangePassword")]
		public async Task<IActionResult> ChangePassword([FromForm] UpdatePasswordDTO dto, Guid id)
		{
			var command = new AdminAccountChangePasswordCommand
			{
				// ExpectedVersion = dto.ExpectedVersion,
				UserId = id,
				CurrentPassword = dto.CurrentPassword,
				NewPassword = dto.NewPassword
			};

			CommandResult result = await _mediator.Send(command);
			return result.IsSuccess switch
			{
				true => Ok(),
				false => HandleFailedCommand(result)
			};
		}
	}
}
