using ImagesManagment.Application.BoundedContexts.ModelPictureManagment.Commands;
using ImagesManagment.Application.BoundedContexts.ModelPictureManagment.Queries;
using ImagesManagment.Application.BoundedContexts.ModelPictureManagment.QueryObjects;
using ImagesManagment.Application.BoundedContexts.PreviewProductPictureManagment.Commands;
using ImagesManagment.Application.BoundedContexts.PreviewProductPictureManagment.Queries;
using ImagesManagment.Application.BoundedContexts.PreviewProductPictureManagment.QueryObjects;
using ImagesManagment.Application.BoundedContexts.UserImageManagment.Commands;
using ImagesManagment.Application.BoundedContexts.UserImageManagment.Queries;
using ImagesManagment.Application.BoundedContexts.UserImageManagment.QueryObjects;
using ImagesManagment.Application.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ImagesManagment.API.Controllers
{
	[AllowAnonymous]
	[ApiController]
	[Route("api/[controller]")]
	public class ImagesController : ApiController
	{
		private readonly IMediator _mediator;
		private readonly IWebHostEnvironment _appEnvironment;
		private readonly ILogger<ImagesController> _logger;
		public ImagesController(IMediator mediator, IWebHostEnvironment appEnvironment, ILogger<ImagesController> logger)
		{
			_mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
			_appEnvironment = appEnvironment;
			_logger = logger;
		}

		[HttpPost]
		[Route("CreateUserImage")]
		public async Task<IActionResult> CreateUserImage([FromForm] IFormFile file, [FromForm] Guid ImageId)
		{
			var imageName = ImageId.ToString() + Path.GetExtension(file.FileName);
			var imagePath = Path.Combine("UserImages", imageName);
			Console.WriteLine("ImageFullPath: " + imagePath);
			if (System.IO.File.Exists(imagePath))
			{
				System.IO.File.Delete(imagePath);
			}

			using (var stream = new FileStream(imagePath, FileMode.Create))
			{
				await file.CopyToAsync(stream);
			}

			Console.WriteLine("Controller ImageId: " + ImageId);

			var command = new ImageCreateCommand()
			{
				ImageId = ImageId,
				UserImageUrl = imagePath,
			};

			CommandResult result = await _mediator.Send(command);
			return result.IsSuccess switch
			{
				true => Ok(result),
				false => HandleFailedCommand(result)
			};
		}

		/*
		[HttpPost]
		[Route("UpdateUserImage")]
		public async Task<IActionResult> UpdateUserImage([FromForm] IFormFile file, [FromForm] Guid ImageId)
		{
			if (System.IO.File.Exists(imagePath))
			{
				System.IO.File.Delete(imagePath);
			}


		}
		*/
		[HttpGet]
		[Route("GetUserImage/{id}")]
		public async Task<IActionResult> GetUserImageInfo(Guid id)
		{
			var query = new GetImageById(id);

			ImageInfo result = await _mediator.Send(query);
			return result switch
			{
				not null => Ok(result),
				null => NotFound()
			};
		}

		[HttpGet]
		[Route("DeleteUserImage/{id}")]
		public async Task<IActionResult> DeleteUserImage(Guid id)
		{
			var query = new ImageDeleteCommand(id);

			CommandResult result = await _mediator.Send(query);

			Console.WriteLine("Preview Image Id: " + id);

			var imageDirectory = "PreviewImages";
			var imageName = id.ToString();
			var imagePath = Directory.GetFiles(imageDirectory, imageName + ".*").FirstOrDefault();

			if (imagePath != null && System.IO.File.Exists(imagePath))
			{
				try
				{
					System.IO.File.Delete(imagePath);
					return Ok();
				}
				catch (Exception ex)
				{
					return StatusCode(500, "Не удалось удалить файл: " + ex.Message);
				}
			}

			return result switch
			{
				not null => Ok(result),
				null => NotFound()
			};
		}

		[HttpPost]
		[Route("CreatePreviewImage")]
		public async Task<IActionResult> CreatePreviewImage([FromForm] IFormFile file, [FromForm] Guid productId, [FromForm] Guid previewImageId)
		{
			_logger.LogInformation($"Received request to create Preview image for productId: {productId}");

			if (file == null || file.Length == 0)
			{
				_logger.LogInformation("File is not provided or empty.");
				return BadRequest("File is not provided or empty.");
			}

			if (productId == Guid.Empty)
			{
				_logger.LogInformation("ProductId Error");
				return BadRequest("ProductId is not provided.");
			}

			var imageName = previewImageId.ToString() + Path.GetExtension(file.FileName);
			var imagePath = Path.Combine("PreviewImages", imageName);

			using (var stream = new FileStream(imagePath, FileMode.Create))
			{
				await file.CopyToAsync(stream);
			}

			Console.WriteLine("Sending to command ImagePath:" +  imagePath);
			var command = new ProductPreviewPictureCreateCommand()
			{
				ProductPreviewImageId = previewImageId,
				Url = imagePath,
				ProductId = productId,
			};

			CommandResult result = await _mediator.Send(command);
			return result.IsSuccess switch
			{
				true => Ok(result.SuccessObject.ToString()),
				false => HandleFailedCommand(result)
			};
		}

		[HttpGet]
		[Route("GetTextureFiles")]
		public async Task<IActionResult> GetTextureFiles(Guid productId)
		{
			var rootPath = _appEnvironment.ContentRootPath;
			var productDirectory = Path.Combine(rootPath, "images", productId.ToString());
			Console.WriteLine(Path.Combine(rootPath, "images", productId.ToString()));
			if (!Directory.Exists(productDirectory))
			{
				return NotFound("Product directory not found.");
			}

			Console.WriteLine($"Directory: {productDirectory}");

			var files = new List<object>();

			foreach (var directoryPath in Directory.GetDirectories(productDirectory))
			{
				// Получаем все файлы внутри поддиректории (предположим, что в каждой директории один файл)
				var filePaths = Directory.GetFiles(directoryPath);
				// Обрабатываем каждый файл внутри поддиректории
				foreach (var filePath in filePaths)
				{
					Console.WriteLine($"FilePath: {filePath}");
					var fileName = Path.GetFileName(filePath);
					var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath); // Асинхронное чтение
					var fileContent = Convert.ToBase64String(fileBytes);

					files.Add(new
					{
						FileName = fileName,
						FileContent = fileContent
					});
				}
			}

			return Ok(files);
		}

		[HttpGet]
		[Route("GetPreviewImage/{id}")]
		public async Task<IActionResult> GetPreviewImageInfo(Guid id)
		{
			var query = new GetPreviewProductImageById(id);

			ProductPreviewImageInfo result = await _mediator.Send(query);
			return result switch
			{
				not null => Ok(result),
				null => NotFound()
			};
		}

		[HttpGet]
		[Route("DeletePreviewImage/{id}")]
		public async Task<IActionResult> DeletePreviewImage(Guid id)
		{
			var query = new ProductPreviewPictureDeleteCommand(id);

			CommandResult result = await _mediator.Send(query);

			Console.WriteLine("DeletePreviewImageId: " + id);

			var imageDirectory = "PreviewImages";
			var imageName = id.ToString();
			var imagePath = Directory.GetFiles(imageDirectory, imageName + ".*").FirstOrDefault();

			if (imagePath != null && System.IO.File.Exists(imagePath))
			{
				try
				{
					System.IO.File.Delete(imagePath);
					return Ok();
				}
				catch (Exception ex)
				{
					return StatusCode(500, "Не удалось удалить файл: " + ex.Message);
				}
			}

			return result switch
			{
				not null => Ok(result),
				null => NotFound()
			};
		}

		[HttpPost]
		[Route("CreateModelTextureImage")]
		public async Task<IActionResult> CreateModelTextureImage(
			   [FromForm] IFormFile file,
			   [FromForm] string textureType,
			   [FromForm] Guid modelId)
		{
			_logger.LogInformation($"Received request to create texture image for modelId: {modelId}, textureType: {textureType}");

			Console.WriteLine("Texture Type: " + textureType);
			Console.WriteLine("File Name: " + file.Name);
			Console.WriteLine("File Length: " + file.Length);
			Console.WriteLine("Model Id: " + modelId);

			if (modelId == Guid.Empty)
			{
				_logger.LogInformation("ModelId Error");
				return BadRequest("ModelId is not provided.");
			}

			if (file == null || file.Length == 0)
			{
				_logger.LogInformation("File Error");
				return BadRequest("File is not provided or empty.");
			}

			var directoryPath = Path.Combine("images", modelId.ToString(), textureType);
			Directory.CreateDirectory(directoryPath); // Create directory if it doesn't exist

			var imageName = $"{Guid.NewGuid()}_{textureType}{Path.GetExtension(file.FileName)}";
			var imagePath = Path.Combine(directoryPath, imageName);

			try
			{
				using (var stream = new FileStream(imagePath, FileMode.Create))
				{
					await file.CopyToAsync(stream);
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error saving file");
				return StatusCode(500, "Internal server error");
			}

			var command = new ModelPictureCreateOrUpdateCommand()
			{
				ModelId = modelId,
				TextureType = textureType,
				ImageUrl = imagePath
			};

			CommandResult result = await _mediator.Send(command);
			return result.IsSuccess ? Ok(result.SuccessObject.ToString()) : HandleFailedCommand(result);
		}


		[HttpGet]
		[Route("GetModelTexture/{id}")]
		public async Task<IActionResult> GetModelTextureImageInfo(Guid id)
		{
			var query = new GetModelPictureById(id);

			ModelPictureInfo result = await _mediator.Send(query);
			return result switch
			{
				not null => Ok(result),
				null => NotFound()
			};
		}

		[HttpGet]
		[Route("GetModelTextureByModelId/{id}")]
		public async Task<IActionResult> GetModelTextureImageInfoByModelId(Guid id)
		{
			var query = new GetModelPictureByModelId(id);

			ModelPictureInfo result = await _mediator.Send(query);

			return result switch
			{
				not null => Ok(result),
				null => NotFound()
			};
		}

		[HttpGet]
		[Route("DeleteModelTextureImage/{id}")]
		public async Task<IActionResult> DeleteModelTextureImage(Guid id)
		{
			var query = new ModelPictureDeleteCommand(id);

			CommandResult result = await _mediator.Send(query);

			var imageDirectory = "images";
			var imageName = id.ToString();
			var imagePath = Directory.GetFiles(imageDirectory, imageName + ".*").FirstOrDefault();

			if (imagePath != null && System.IO.File.Exists(imagePath))
			{
				try
				{
					System.IO.File.Delete(imagePath);
					return Ok();
				}
				catch (Exception ex)
				{
					return StatusCode(500, "Не удалось удалить файл: " + ex.Message);
				}
			}

			return result switch
			{
				not null => Ok(result),
				null => NotFound()
			};
		}
	}
}
