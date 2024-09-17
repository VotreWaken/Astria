using OrderManagement.Application.Results;
using OrderManagement.Application.BoundedContexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OrderManagement.API.Controllers
{
	[Authorize(AuthenticationSchemes = "Bearer")]
	[Route("api/[controller]")]
	[ApiController]
	public abstract class ApiController : ControllerBase
	{
		protected IActionResult HandleFailedCommand(CommandResult result)
		{
			return result.FailureType switch
			{
				FailureTypes.NotFound => NotFound(),
				FailureTypes.Duplicate => BadRequest(result.FailureReasons),
				FailureTypes.BusinessRule => BadRequest(result.FailureReasons),
				_ => BadRequest()
			};
		}
	}
}
