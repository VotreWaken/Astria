using Astria.NotificationService.NotificationHub;
using Astria.QueryRepository.Repository;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Security.Claims;
using UserProductLikesManagement.Application.BoundedContexts.UserProductLikesManagement.Commands;
using UserProductLikesManagement.Application.BoundedContexts.UserProductLikesManagement.Queries;
using UserProductLikesManagement.Application.BoundedContexts.UserProductLikesManagement.QueryObjects;
using UserProductLikesManagement.Application.Results;
using System.Text.Json;
using Microsoft.IdentityModel.Tokens;

namespace UserProductLikesManagement.API.Controllers
{
	public class UserProductLikesController : Controller
	{
		private readonly IMediator _mediator;
		private readonly IWebHostEnvironment _appEnvironment;
		private readonly ILogger<UserProductLikesController> _logger;

		public UserProductLikesController(IMediator mediator, 
			IWebHostEnvironment appEnvironment, 
			ILogger<UserProductLikesController> logger,
			IHttpClientFactory httpClientFactory, 
			IHubContext<NotificationHub> hubContext,
			IConnectionMapping connectionMapping)
		{
			_mediator = mediator;
			_appEnvironment = appEnvironment;
			_logger = logger;
			_httpClientFactory = httpClientFactory;
			_hubContext = hubContext;
			_connectionMapping = connectionMapping;
		}


		private readonly IHttpClientFactory _httpClientFactory;
		private readonly IHubContext<NotificationHub> _hubContext;
		private readonly IConnectionMapping _connectionMapping;

		[HttpPost]
		[Route("UserProductLike")]
		public async Task<IActionResult> LikeProduct([FromForm] Guid userId, [FromForm] Guid productId)
		{
			// var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			var command = new UserProductLikeCreateCommand()
			{
				UserId = userId,
				ProductId = productId,
			};

			CommandResult result = await _mediator.Send(command);

			if (result.IsSuccess)
			{
				var client = _httpClientFactory.CreateClient();
				var response = await client.GetAsync($"https://localhost:7185/api/Products/{productId}");

				if (!response.IsSuccessStatusCode)
				{
					return StatusCode((int)response.StatusCode, "Failed to get product information.");
				}

				var productJson = await response.Content.ReadAsStringAsync();
				var product = JsonSerializer.Deserialize<ProductInfo>(productJson);

				if (product == null)
				{
					return NotFound("Product not found.");
				}

				if (!string.IsNullOrEmpty(product.userId.ToString()))
				{
					var connectionId = _connectionMapping.GetConnectionId(product.userId);
					Console.WriteLine("ConnectionId: ", connectionId);
					if (!string.IsNullOrEmpty(connectionId))
					{
						await _hubContext.Clients.Client(connectionId).SendAsync("ReceiveNotification", product.userId.ToString(), productId.ToString());
						Console.WriteLine("Notification sent.");
						Console.WriteLine("Product UserId: " + product.userId.ToString());
						Console.WriteLine("Product Id: " + productId.ToString());
					}
					else
					{
						Console.WriteLine("No active connection for the user.");
					}
				}

				return Ok("Like was successful and notification sent.");
			}

			return result.IsSuccess switch
			{
				true => Ok(result),
				// false => HandleFailedCommand(result)
			};
		}

		public class ProductInfo : IQueryEntity
		{
			public Guid Id { get; set; }
			public string Name { get; set; }
			public string Description { get; set; }

			[BsonRepresentation(BsonType.Decimal128)]
			public decimal Price { get; set; }
			public Guid ModelId { get; set; }
			public bool IsAvailable { get; set; }
			public DateTime Date { get; set; }
			public Guid PreviewImageId { get; set; }
			public Guid userId { get; set; }
			public int Views { get; set; }
			public long Version { get; set; }
		}

		[HttpPost]
		[Route("UnlikeUserProduct")]
		public async Task<IActionResult> UnlikeProduct([FromForm] Guid userId, [FromForm] Guid productId)
		{
			var command = new UserProductLikesDeleteCommand()
			{
				UserId = userId,
				ProductId = productId,
			};

			CommandResult result = await _mediator.Send(command);
			return result.IsSuccess switch
			{
				true => Ok(result),
				// false => HandleFailedCommand(result)
			};
		}

		[HttpGet("GetLikedProducts")]
		public async Task<IActionResult> GetLikedProducts([FromQuery] Guid userId)
		{
			// var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			var query = new GetAllUserProductLikesByUserId(userId);

			IEnumerable<UserProductLikesInfo> result = await _mediator.Send(query);
			return result switch
			{
				not null => Ok(result),
				null => NotFound()
			};
		}

		[HttpGet("GetUsersWhoLikedProducts")]
		public async Task<IActionResult> GetUsersWhoLikedProducts([FromQuery] Guid productId)
		{
			// var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			var query = new GetUserProductLikeByProductId(productId);

			IEnumerable<UserProductLikesInfo> result = await _mediator.Send(query);
			return result switch
			{
				not null => Ok(result),
				null => NotFound()
			};
		}

		[HttpGet("GetMostLikedProducts")]
		public async Task<IActionResult> GetMostLikedProducts([FromQuery] int count)
		{
			var query = new GetMostLikedProducts(count);
			List<ProductLikeInfo> likeCount = await _mediator.Send(query);
			return Ok(likeCount);
		}

		[HttpGet("GetProductLikeCount")]
		public async Task<IActionResult> GetProductLikeCount([FromQuery] Guid productId)
		{
			var query = new GetUserProductLikeCountByProductId(productId);
			int likeCount = await _mediator.Send(query);
			return Ok(likeCount);
		}

		[HttpGet("IsProductLikeByUserId")]
		public async Task<IActionResult> IsProductLikeByUserId([FromQuery] Guid userId, [FromQuery] Guid productId)
		{
			var query = new IsProductLikeByUserId(productId, userId);
			bool IsLiked = await _mediator.Send(query);
			return Ok(IsLiked);
		}
	}
}
