using OrderManagement.Application.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.API.DTOs;
using OrderManagement.Application.BoundedContexts.Commands;
using MassTransit;
using Astria.Rabbitmq.Events;
using OrderManagement.Application.BoundedContexts.Queries;
using OrderManagement.Application.BoundedContexts.QueryObjects;
using OrderManagement.Domain.BoundedContexts.OrderManagment.ValueObjects;
using System.IO.Compression;
using System.Net.Http;
namespace OrderManagement.API.Controllers
{
	[AllowAnonymous]
	[ApiController]
	[Route("api/[controller]")]
	public class OrdersController : ApiController
	{
		private readonly IMediator _mediator;
		private readonly IBus _bus;
		private readonly HttpClient _httpClient;
		public OrdersController(IMediator mediator, IBus bus, HttpClient httpClientFactory)
		{
			_mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
			_bus = bus ?? throw new ArgumentNullException(nameof(bus));
			_httpClient = httpClientFactory;
		}

		[HttpPost]
		[Route("CreateOrder")]
		public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDTO dto)
		{
			var command = new OrderCreateCommand()
			{
				ProductId = dto.ProductId,
				CustomerId = dto.CustomerId,
				OrderAmount = dto.OrderAmount,
				OrderDate = dto.OrderDate,
				Status = OrderStatus.FromString(dto.Status.ToString()),
			};

			CommandResult result = await _mediator.Send(command);

			if (result.IsSuccess)
			{
				// Опубликуйте событие ModelCreatedEvent в шину
				var modelCreatedEvent = new ModelCreatedEvent
				{
					ModelId = Guid.NewGuid(), // Предполагается, что OrderId возвращается как часть CommandResult
				};

				await _bus.Publish(modelCreatedEvent);
				
				return Ok();
			}

			return HandleFailedCommand(result);
		}

		public class ModelFileResponse
		{
			public string File { get; set; }
			public string FileType { get; set; }
		}

		public class TextureFileResponse
		{
			public string FileName { get; set; }
			public string FileContent { get; set; }
		}

		[HttpGet]
		[Route("GetOrderFiles")]
		public async Task<IActionResult> GetOrderFiles(Guid modelId)
		{
			try
			{
				// Запрос к Model Service
				var modelResponse = await _httpClient.GetAsync($"https://localhost:7109/api/Models/GetFileModelInfo/{modelId}");
				modelResponse.EnsureSuccessStatusCode();
				var modelData = await modelResponse.Content.ReadFromJsonAsync<ModelFileResponse>();
				var modelBytes = Convert.FromBase64String(modelData.File);
				Console.WriteLine(modelData.FileType);
				// Запрос к Image Service
				var textureResponse = await _httpClient.GetAsync($"https://localhost:7203/api/Images/GetTextureFiles/?productId={modelId}");
				textureResponse.EnsureSuccessStatusCode();
				var textureFiles = await textureResponse.Content.ReadFromJsonAsync<List<TextureFileResponse>>();

				foreach (var item in textureFiles)
				{
					Console.WriteLine($"Texture Files Names: {item.FileName}");
				}

				// Создание временной директории для сохранения файлов
				var tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
				Directory.CreateDirectory(tempDirectory);

				// Сохранение модели
				var modelPath = Path.Combine(tempDirectory, $"model.{modelData.FileType}");
				await System.IO.File.WriteAllBytesAsync(modelPath, modelBytes);

				// Сохранение текстур
				foreach (var texture in textureFiles)
				{
					var texturePath = Path.Combine(tempDirectory, texture.FileName);
					await System.IO.File.WriteAllBytesAsync(texturePath, Convert.FromBase64String(texture.FileContent));
				}

				// Архивирование директории
				var zipPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.zip");
				ZipFile.CreateFromDirectory(tempDirectory, zipPath);

				// Чистка временной директории
				Directory.Delete(tempDirectory, true);

				// Отправка архива клиенту
				var zipBytes = await System.IO.File.ReadAllBytesAsync(zipPath);
				System.IO.File.Delete(zipPath); // Удаление временного ZIP файла

				return Ok(File(zipBytes, "application/zip", "model_and_textures.zip"));
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message, "Error while processing the order");
				return StatusCode(500, "Internal server error");
			}
		}

		[HttpPost]
		[Route("ChangeOrderStatus")]
		public async Task<IActionResult> ChangeOrderStatus(ChangeOrderStatusDTO dto)
		{
			var command = new OrderChangeStatusCommand()
			{
				OrderId = dto.Id,
				Status = OrderStatus.FromString(dto.Status.ToString()),
			};

			CommandResult result = await _mediator.Send(command);
			if (!result.IsSuccess)
			{
				return HandleFailedCommand(result);
			}

			return Ok(result);
		}

		[HttpGet]
		[Route("DeleteOrder/{id}")]
		public async Task<IActionResult> DeleteOrder(Guid id)
		{
			var query = new OrderDeleteCommand(id);

			CommandResult result = await _mediator.Send(query);
			return result switch
			{
				not null => Ok(result),
				null => NotFound()
			};
		}

		[HttpGet("GetAllOrders")]
		public async Task<IActionResult> GetAllOrders()
		{
			// var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			var query = new GetAllOrders();

			IEnumerable<OrderInfo> result = await _mediator.Send(query);
			return result switch
			{
				not null => Ok(result),
				null => NotFound()
			};
		}

		[HttpGet("GetOrderById")]
		public async Task<IActionResult> GetOrderById([FromQuery] Guid orderId)
		{
			// var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			var query = new GetOrderByIdQuery(orderId);

			OrderInfo result = await _mediator.Send(query);
			return result switch
			{
				not null => Ok(result),
				null => NotFound()
			};
		}

		[HttpGet("GetOrdersByUserId")]
		public async Task<IActionResult> GetOrdersByUserId([FromQuery] Guid productId)
		{
			// var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			var query = new GetOrdersByUserId(productId);

			IEnumerable<OrderInfo> result = await _mediator.Send(query);
			return result switch
			{
				not null => Ok(result),
				null => NotFound()
			};
		}

		[HttpGet("IsOrderConfirmedToUserId")]
		public async Task<IActionResult> IsOrderConfirmedToUserId([FromQuery] Guid userId, [FromQuery] Guid productId)
		{
			var query = new IsOrderConfirmedToUserId(productId, userId);
			bool IsLiked = await _mediator.Send(query);
			return Ok(IsLiked);
		}

	}
}
