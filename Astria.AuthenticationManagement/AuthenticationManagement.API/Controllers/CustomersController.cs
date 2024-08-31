using AuthenticationManagement.Application.BoundedContexts.UserAccountManagement.Commands;
using AuthenticationManagement.Application.BoundedContexts.UserAccountManagement.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Astria.Rabbitmq.Events;
using MassTransit;
using AuthenticationManagement.Domain.BoundedContexts.UserAccountManagement.Aggregates;
using AuthenticationManagement.Application.BoundedContexts.UserAccountManagement.QueryObjects;
using AuthenticationManagement.Application.Results;
using AuthenticationManagement.API.DTOs;

namespace AuthenticationManagement.API.Controllers
{

	[Authorize(Roles = "Admin, Customer")]
	public class CustomersController : ApiController
	{
		private readonly IMediator _mediator;
		private readonly IBus _bus;
		public CustomersController(IMediator mediator, IBus bus)
		{
			_mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
			_bus = bus ?? throw new ArgumentNullException(nameof(bus));
		}

		[Authorize(Roles = "Admin")]
		[HttpGet]
		public async Task<IActionResult> GetAllCustomersInfo()
		{
			var query = new GetAllCustomerInfosQuery();

			List<CustomerInfo> result = await _mediator.Send(query);
			return result switch
			{
				not null => Ok(result),
				null => NotFound()
			};
		}

		[HttpGet]
		[Route("{id}")]
		[AllowAnonymous]
		public async Task<IActionResult> GetCustomerInfo(Guid id)
		{
			var query = new GetCustomerInfoQuery(id);

			CustomerInfo result = await _mediator.Send(query);
			return result switch
			{
				not null => Ok(result),
				null => NotFound()
			};
		}

		[AllowAnonymous]
		[HttpPost]
		public async Task<IActionResult> RegisterCustomer(CreateCustomerAccountDTO dto)
		{
			byte[] modelDataBytes;
			using (var memoryStream = new MemoryStream())
			{
				await dto.UserImage.CopyToAsync(memoryStream);
				modelDataBytes = memoryStream.ToArray();
			}

			var userImageId = Guid.NewGuid();

			var command = new CustomerAccountCreateCommand
			{
				Email = dto.Email,
				PlainPassword = dto.Password,
				Firstname = dto.Firstname,
				Lastname = dto.Surname,
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

		[Authorize(Roles = "CUSTOMER")]
		[HttpPatch]
		[Route("{id}/ChangeEmail")]
		public async Task<IActionResult> ChangeEmail(UpdateEmailAddressDTO dto, Guid id)
		{
			var command = new CustomerAccountChangeEmailCommand
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

		[HttpPost]
		[Route("DeleteCustomer/{id}")]
		[AllowAnonymous]
		public async Task<IActionResult> DeleteCustomer(DeleteCustomerDTO dto)
		{
			var command = new CustomerAccountDeleteCommand
			{
				UserId = dto.CustomerId,
				CurrentPassword = dto.CurrentPassword
			};

			CommandResult result = await _mediator.Send(command);

			return result.IsSuccess switch
			{
				true => Ok(),
				false => HandleFailedCommand(result)
			};
		}

		[Authorize(Roles = "CUSTOMER")]
		[HttpPatch]
		[Route("{id}/ChangePassword")]
		public async Task<IActionResult> ChangePassword(UpdatePasswordDTO dto, Guid id)
		{
			var command = new CustomerAccountChangePasswordCommand
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
