using Astria.Rabbitmq.Events;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductManagement.API.DTOs;
using ProductManagement.Application.BoundedContexts.Commands;
using ProductManagement.Application.BoundedContexts.Queries;
using ProductManagement.Application.Results;
using System.Text.Json;
using ProductManagement.Domain.BoundedContexts.ProductManagement.Aggregates;

namespace ProductManagement.API.Controllers
{
	[AllowAnonymous]
	[ApiController]
	[Route("api/[controller]")]
	public class ProductsController : ApiController
	{
		private readonly IMediator _mediator;
		private readonly IBus _bus;
		private readonly IHttpClientFactory _httpClientFactory;
		public ProductsController(IMediator mediator, IBus bus, IHttpClientFactory httpClientFactory)
		{
			_mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
			_bus = bus ?? throw new ArgumentNullException(nameof(bus));
			_httpClientFactory = httpClientFactory;
		}

		
		[HttpPost]
		[Route("EditProduct")]
		public async Task<IActionResult> EditProduct(UpdateProductDTO dto)
		{
			var command = new ProductUpdateCommand()
			{
				ProductId = dto.ProductId,
				Name = dto.Name,
				Description = dto.Description,
				Price = dto.Price,
			};

			CommandResult result = await _mediator.Send(command);
			if (!result.IsSuccess)
			{
				return HandleFailedCommand(result);
			}

			return Ok(result);
		}
		

		[HttpPost]
		[Route("CreateProduct")]
		public async Task<IActionResult> CreateOrder(
			[FromForm] CreateProductDTO dto,
			IFormFile modelData,
			IFormFile previewImage,
			IFormFile baseColorUrl,
			IFormFile normalMapUrl,
			IFormFile displacementUrl,
			IFormFile metallicUrl,
			IFormFile roughnessUrl,
			IFormFile emissiveUrl,
			IFormFile? binFileData = null)
		{
			// Обработка OBJ файла
			byte[] modelDataBytes;
			using (var memoryStream = new MemoryStream())
			{
				await modelData.CopyToAsync(memoryStream);
				modelDataBytes = memoryStream.ToArray();
			}

			var modelExtension = Path.GetExtension(modelData.FileName).ToLower();
			string modelType;
			byte[] BinFileData = null;

			switch (modelExtension)
			{
				case ".obj":
					modelType = "OBJ";
					break;
				case ".fbx":
					modelType = "FBX";
					break;
				case ".glb":
					modelType = "GLB";
					break;
				case ".gltf":
					modelType = "GLTF";
					using (var memoryStream = new MemoryStream())
					{
						await binFileData.CopyToAsync(memoryStream);
						BinFileData = memoryStream.ToArray();
					}
					break;
				default:
					modelType = "Unknown";
					break;
			}

			// Обработка файлов текстур
			byte[] baseColorData = baseColorUrl != null ? await ReadFileAsync(baseColorUrl) : Array.Empty<byte>();
			byte[] normalMapData = normalMapUrl != null ? await ReadFileAsync(normalMapUrl) : Array.Empty<byte>();
			byte[] displacementData = displacementUrl != null ? await ReadFileAsync(displacementUrl) : Array.Empty<byte>();
			byte[] metallicData = metallicUrl != null ? await ReadFileAsync(metallicUrl) : Array.Empty<byte>();
			byte[] roughnessData = roughnessUrl != null ? await ReadFileAsync(roughnessUrl) : Array.Empty<byte>();
			byte[] emissiveData = emissiveUrl != null ? await ReadFileAsync(emissiveUrl) : Array.Empty<byte>();

			// Обработка превью изображения
			byte[] previewImageDataBytes;
			using (var memoryStream = new MemoryStream())
			{
				await previewImage.CopyToAsync(memoryStream);
				previewImageDataBytes = memoryStream.ToArray();
			}

			var createdModelId = Guid.NewGuid();
			var previewImageId = Guid.NewGuid();

			var command = new ProductCreateCommand()
			{
				Name = dto.Name,
				Description = dto.Description,
				Price = dto.Price,
				IsAvailable = dto.IsAvailable,
				Date = dto.Date,
				ModelId = createdModelId,
				PreviewImageId = previewImageId,
				UserId = dto.UserId,
			};

			CommandResult result = await _mediator.Send(command);
			if (!result.IsSuccess)
			{
				return HandleFailedCommand(result);
			}
			var productId = result.ProductId;
			var modelId = result.ModelId;

			var previewImageProcessingEvent = new ProductPreviewImageProcessingEvent
			{
				ProductPreviewImageId = previewImageId,
				ProductId = productId,
				ImageData = previewImageDataBytes,
			};
			await _bus.Publish(previewImageProcessingEvent);

			var productCreatedEvent = new ProductCreatedEvent
			{
				ProductId = productId,
				Name = dto.Name,
				Description = dto.Description,
				Price = dto.Price,
				IsAvailable = dto.IsAvailable,
				Date = dto.Date,
				ModelId = modelId,
				PreviewImageId = previewImageId
			};
			await _bus.Publish(productCreatedEvent);

			var createdTextureId = Guid.NewGuid();

			// Публикация события TextureProcessingEvent с несколькими текстурами
			var textureProcessingEvent = new TextureProcessingEvent
			{
				TextureId = createdTextureId,
				ProductId = productId,
				ModelId = modelId,
				BaseColorData = baseColorData,
				NormalMapData = normalMapData,
				DisplacementData = displacementData,
				MetallicData = metallicData,
				RoughnessData = roughnessData,
				EmissiveData = emissiveData,
				ModelData = modelDataBytes,
			};
			await _bus.Publish(textureProcessingEvent);

			// Отправляем ModelCreatedEvent после обработки всех текстур
			Console.WriteLine("Model Data Bytes Length: " + modelDataBytes.Length);
			var modelCreatedEvent = new ModelCreatedEvent
			{
				ProductId = productId,
				ModelId = modelId,
				TextureId = createdTextureId,  // Здесь предполагается, что вам нужно создать новый TextureId для всех текстур.
				ModelData = modelDataBytes,
				BinFileData = BinFileData, 
				ModelType = modelType,
			};

			await _bus.Publish(modelCreatedEvent);

			return Ok();
		}

		

		// Helper method to read file data
		private async Task<byte[]> ReadFileAsync(IFormFile file)
		{
			using (var memoryStream = new MemoryStream())
			{
				await file.CopyToAsync(memoryStream);
				return memoryStream.ToArray();
			}
		}




		[HttpGet]
		[Route("{id}")]
		public async Task<IActionResult> GetProductInfo(Guid id)
		{
			Console.WriteLine("ID: " + id);
			var query = new GetProductById(id);

			ProductManagement.Application.BoundedContexts.QueryObjects.ProductInfo result = await _mediator.Send(query);
			return result switch
			{
				not null => Ok(result),
				null => NotFound()
			};
		}

		[HttpGet]
		[Route("TestGetProductInfo/{id}")]
		public async Task<IActionResult> TestGetProductInfo(Guid id)
		{
			var productClient = _httpClientFactory.CreateClient();
			var modelClient = _httpClientFactory.CreateClient();
			var textureClient = _httpClientFactory.CreateClient();
			var previewClient = _httpClientFactory.CreateClient();
			// Получение информации о продукте
			var productResponse = await productClient.GetAsync($"https://localhost:7185/api/Products/{id}");

			var productContent = await productResponse.Content.ReadAsByteArrayAsync();
			Console.WriteLine(productContent);

			if (!productResponse.IsSuccessStatusCode)
			{
				return NotFound("Product not found");
			}


			var product = JsonSerializer.Deserialize<ProductInfo>(productContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

			// await productResponse.Content.ReadFromJsonAsync<ProductInfo>();

			// Получение информации о модели
			var modelResponse = await modelClient.GetAsync($"https://localhost:7109/api/Models/GetFileModelInfo/{product.modelId}");
			if (!modelResponse.IsSuccessStatusCode)
			{
				return NotFound($"Model with ID {product.modelId} not found");
			}

			var modelContent = await modelResponse.Content.ReadAsByteArrayAsync();

			
			// await modelResponse.Content.ReadFromJsonAsync<ModelInfo>();

			// Получение информации о текстуре
			var textureResponse = await textureClient.GetAsync($"https://localhost:7203/api/Images/GetModelTextureByModelId/{product.modelId}");
			if (!textureResponse.IsSuccessStatusCode)
			{
				return NotFound($"Texture for Model ID {product.modelId} not found");
			}

			var textureContent = await textureResponse.Content.ReadAsStringAsync();

			var texture = JsonSerializer.Deserialize<ModelPictureInfo>(textureContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

			// Получение информации о Preview Image
			var previewResponse = await previewClient.GetAsync($"https://localhost:7203/api/Images/GetPreviewImage/{product.previewImageId}");
			if (!previewResponse.IsSuccessStatusCode)
			{
				// return NotFound($"Preview Image ID {product.previewImageId} not found");
			}

			var previewContent = await previewResponse.Content.ReadAsStringAsync();

			var preview = JsonSerializer.Deserialize<ModelPictureInfo>(previewContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });


			var productDetail = new
			{
				Product = product,
				Model = modelContent,
				previewImageUrl = preview.Url,
				Texture = new
				{
					texture.ModelId,
					TextureUrl = $"https://localhost:7203/{texture.Url}"
				}

			};

			return Ok(productDetail);
		}

		public class ModelInfo
		{
			public Guid Id { get; set; }
			public byte[] Data { get; set; }
			// другие свойства
		}

		public class ModelPictureInfo
		{
			public Guid ModelId { get; set; }
			public Guid Id { get; set; }
			public string Url { get; set; }
			// другие свойства
		}

		public class ProductInfo
		{
			public Guid id { get; set; }
			public Guid userId { get; set; }
			public string name { get; set; }
			public string description { get; set; }
			public decimal price { get; set; }
			public Guid modelId { get; set; }

			public int Views { get; set; }
			public bool isAvailable { get; set; }
			public DateTime date { get; set; }
			public Guid previewImageId { get; set; }
		}


		/*
		[HttpGet]
		[Route("GetProductModel/{id}")]
		public async Task<IActionResult> GetProductModel(Guid id)
		{
			var query = new GetProductById(id);
			// Ваш код для получения модели по ID из репозитория или базы данных
			var model = await _mediator.Send(query);
			if (model == null)
			{
				return NotFound();
			}

			// return File(model, "application/octet-stream", "model.obj");
		}
		*/

		[HttpGet]
		[Route("GetAllProducts")]
		public async Task<IActionResult> GetAllProductsInfo()
		{
			var query = new GetAllProductsInfosQuery();

			List<ProductManagement.Application.BoundedContexts.QueryObjects.ProductInfo> result = await _mediator.Send(query);
			return result switch
			{
				not null => Ok(result),
				null => NotFound()
			};
		}

		[HttpGet]
		[Route("GetProductsByPrice")]
		public async Task<IActionResult> GetProductsByPrice(decimal price)
		{
			var query = new GetProductsByPriceQuery(price);

			IEnumerable<ProductManagement.Application.BoundedContexts.QueryObjects.ProductInfo> result = await _mediator.Send(query);
			return result switch
			{
				not null => Ok(result),
				null => NotFound()
			};
		}

		[HttpGet]
		[Route("GetProductsByName")]
		public async Task<IActionResult> GetProductsByName(string name)
		{
			var query = new GetProductsByNameQuery(name);

			IEnumerable<ProductManagement.Application.BoundedContexts.QueryObjects.ProductInfo> result = await _mediator.Send(query);
			return result switch
			{
				not null => Ok(result),
				null => NotFound()
			};
		}

		[HttpPost]
		[Route("GetMostViewedProductInfosQuery")]
		public async Task<IActionResult> GetMostViewedProduct()
		{
			var query = new GetMostViewedProductInfosQuery();
			var result = await _mediator.Send(query);

			return result switch
			{
				not null => Ok(result),
				null => NotFound()
			};
		}

		[HttpGet("GetFilteredProducts")]
		public async Task<IActionResult> GetFilteredProducts(string? position, 
			Guid? teamId, 
			int page = 1, 
			int pageSize = 10, 
			SortState sortOrder = SortState.PriceAsc,
			decimal? minPrice = null,
			decimal? maxPrice = null,
			DateTime? startDate = null,
			DateTime? endDate = null)
		{
			var query = new GetProductsByFiltersQuery(position, teamId, page, pageSize, 
				sortOrder, 
				minPrice,
				maxPrice,
				startDate,
				endDate
			);
			var result = await _mediator.Send(query);

			int totalCount = result.TotalCount;
			int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

			var response = new
			{
				Items = result.Items,
				TotalItems = totalCount,
				CurrentPage = page,
				PageSize = pageSize,
				TotalPages = totalPages
			};

			return Ok(new
			{
				response
			});
		}

		[HttpGet("GetProductsByUserId")]
		public async Task<IActionResult> GetProductsByUserId(
			Guid userId,
			int page = 1,
			int pageSize = 10,
			SortState sortOrder = SortState.PriceAsc
			)
		{
			var query = new GetAllProductsByUserId(userId, page, pageSize,
				sortOrder
			);
			var result = await _mediator.Send(query);
			return Ok(result.Items);
		}


		[HttpGet]
		[Route("DeleteProduct/{id}")]
		public async Task<IActionResult> DeleteProduct(Guid id)
		{
			var query = new ProductDeleteCommand(id);

			CommandResult result = await _mediator.Send(query);
			return result switch
			{
				not null => Ok(result),
				null => NotFound()
			};
		}

	}
}
