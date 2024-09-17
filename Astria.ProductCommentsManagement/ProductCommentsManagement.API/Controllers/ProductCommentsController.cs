using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductCommentsManagement.Application.BoundedContexts.ProductCommentsManagement.Commands;
using ProductCommentsManagement.Application.BoundedContexts.ProductCommentsManagement.Queries;
using ProductCommentsManagement.Application.BoundedContexts.ProductCommentsManagement.QueryObjects;
using ProductCommentsManagement.Application.Results;

namespace ProductCommentsManagement.API.Controllers
{
	public class ProductCommentsController : Controller
	{
		private readonly IMediator _mediator;
		private readonly ILogger<ProductCommentsController> _logger;

		public ProductCommentsController(IMediator mediator, ILogger<ProductCommentsController> logger)
		{
			_mediator = mediator;
			_logger = logger;
		}

		[HttpPost]
		[Route("CreateProductComment")]
		public async Task<IActionResult> CreateProductComment([FromForm] Guid userId, [FromForm] Guid productId, [FromForm] string messageText, [FromForm] Guid parrentCommentId)
		{
			// var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			var command = new ProductCommentsCreateCommand()
			{
				UserId = userId,
				ProductId = productId,
				CommentText = messageText, 
				ParentCommentId = parrentCommentId,
			};

			CommandResult result = await _mediator.Send(command);
			return result.IsSuccess switch
			{
				true => Ok(result),
				// false => HandleFailedCommand(result)
			};
		}

		[HttpPost]
		[Route("GetCommentsCountByProductId")]
		public async Task<IActionResult> GetCommentsCountByProductId([FromForm] Guid ProductId)
		{
			var command = new GetCommentsCountByProductId(ProductId);

			int result = await _mediator.Send(command);

			return Ok(result);
		}

		[HttpPost]
		[Route("DeleteProductComment")]
		public async Task<IActionResult> DeleteProductComment([FromForm] Guid id)
		{
			// var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			var command = new ProductCommentsDeleteCommand()
			{
				Id = id,
			};

			CommandResult result = await _mediator.Send(command);
			return result.IsSuccess switch
			{
				true => Ok(result),
				// false => HandleFailedCommand(result)
			};
		}

		[HttpPost]
		[Route("EditProductComment")]
		public async Task<IActionResult> EditProductComment([FromForm] Guid messageId, [FromForm] string messageText)
		{
			// var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			var command = new ProductCommentsEditCommand()
			{
				Id = messageId,
				CommentText = messageText,
			};

			CommandResult result = await _mediator.Send(command);
			return result.IsSuccess switch
			{
				true => Ok(result),
				// false => HandleFailedCommand(result)
			};
		}

		// GetAllProductComments
		[HttpGet("GetAllProductComments")]
		public async Task<IActionResult> GetAllProductComments([FromQuery] Guid productId)
		{
			// var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			var query = new GetAllProductComments(productId);

			IEnumerable<ProductCommentsInfo> result = await _mediator.Send(query);
			return result switch
			{
				not null => Ok(result),
				null => NotFound()
			};
		}
	}
}
