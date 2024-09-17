using MediatR;
using Microsoft.AspNetCore.Mvc;
using UserFollowsManagement.Application.BoundedContexts.UserFollowsManagement.Commands;
using UserFollowsManagement.Application.BoundedContexts.UserFollowsManagement.Queries;
using UserFollowsManagement.Application.BoundedContexts.UserFollowsManagement.QueryObjects;
using UserFollowsManagement.Application.Results;

namespace UserFollowsManagement.API.Controllers
{
	public class UserFollowsController : Controller
	{
		private readonly IMediator _mediator;
		private readonly IWebHostEnvironment _appEnvironment;
		private readonly ILogger<UserFollowsController> _logger;

		public UserFollowsController(IMediator mediator, IWebHostEnvironment appEnvironment, ILogger<UserFollowsController> logger)
		{
			_mediator = mediator;
			_appEnvironment = appEnvironment;
			_logger = logger;
		}

		[HttpGet("IsAlreadyFollowed")]
		public async Task<IActionResult> IsAlreadyFollowed([FromQuery] Guid userId, [FromQuery] Guid productId)
		{
			var query = new GetUserAlreadyFollows(productId, userId);
			bool IsLiked = await _mediator.Send(query);
			return Ok(IsLiked);
		}

		[HttpPost]
		[Route("FollowUser")]
		public async Task<IActionResult> FollowUser([FromForm] Guid followerId, [FromForm] Guid followedId)
		{
			// var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			var command = new UserFollowCreateCommand()
			{
				FollowerId = followerId,
				FollowedId = followedId,
			};

			CommandResult result = await _mediator.Send(command);
			return result.IsSuccess switch
			{
				true => Ok(result),
				// false => HandleFailedCommand(result)
			};
		}

		[HttpPost]
		[Route("UnFollowUser")]
		public async Task<IActionResult> UnFollowUser([FromForm] Guid followerId, [FromForm] Guid followedId)
		{
			var command = new UserUnFollowsCreatedCommand()
			{
				FollowerId = followerId,
				FollowedId = followedId,
			};

			CommandResult result = await _mediator.Send(command);
			return result.IsSuccess switch
			{
				true => Ok(result),
				// false => HandleFailedCommand(result)
			};
		}

		[HttpGet("GetUserFollows")]
		public async Task<IActionResult> GetUserFollows([FromQuery] Guid userId)
		{
			// var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			var query = new GetAllUserFollows(userId);

			IEnumerable<UserFollowInfo> result = await _mediator.Send(query);
			return result switch
			{
				not null => Ok(result),
				null => NotFound()
			};
		}

		[HttpGet("GetUserFollowers")]
		public async Task<IActionResult> GetUserFollowers([FromQuery] Guid userId)
		{
			// var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			var query = new GetAllUserFollowers(userId);

			IEnumerable<UserFollowInfo> result = await _mediator.Send(query);
			return result switch
			{
				not null => Ok(result),
				null => NotFound()
			};
		}

		[HttpGet("GetFollowerCount")]
		public async Task<IActionResult> GetFollowerCount([FromQuery] Guid productId)
		{
			var query = new GetFollowerCountByUserId(productId);
			int likeCount = await _mediator.Send(query);
			return Ok(likeCount);
		}

		[HttpGet("GetFollowedCount")]
		public async Task<IActionResult> GetFollowedCount([FromQuery] Guid productId)
		{
			var query = new GetFollowedCountByUserId(productId);
			int likeCount = await _mediator.Send(query);
			return Ok(likeCount);
		}
	}
}
