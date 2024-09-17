using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelsManagment.API.DTOs;
using ModelsManagment.Application.BoundedContexts.Commands;
using ModelsManagment.Application.BoundedContexts.Queries;
using ModelsManagment.Application.BoundedContexts.QueryObjects;
using ModelsManagment.Application.Results;

namespace ModelsManagment.API.Controllers
{
	[AllowAnonymous]
	[ApiController]
	[Route("api/[controller]")]
	public class ModelsController : ApiController
	{
		private readonly IWebHostEnvironment _environment;
		private readonly IMediator _mediator;
		private readonly ILogger<ModelsController> _logger;
		public ModelsController(IMediator mediator, ILogger<ModelsController> logger, IWebHostEnvironment environment)
		{
			_mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
			_logger = logger;
			_environment = environment;
		}

		[HttpPost]
		[Route("CreateModel")]
		public async Task<IActionResult> CreateModel([FromForm] CreateModelDTO dto)
		{
			Console.WriteLine("TAKE REQUEST!!!!");
			_logger.LogInformation($"Received request to create texture image for Model Texture: {dto.modelTexture}");

			if (dto.objFile == null || dto.objFile.Length == 0)
			{
				_logger.LogInformation("File is not provided or empty.");
				return BadRequest("File is not provided or empty.");
			}
			Console.WriteLine("Model Data Bytes Length: " + dto.objFile.Length);
			if (dto.modelTexture == Guid.Empty)
			{
				_logger.LogInformation("Model Texture Error");
				return BadRequest("Model Texture is not provided.");
			}

			if (dto.ProductId == Guid.Empty)
			{
				_logger.LogInformation("Product Id Error");
				return BadRequest("Product Id is not provided.");
			}

			// Define the file paths
			string modelDataFolderPath = Path.Combine("Models", dto.modelId.ToString());
			string modelDataFilePath = Path.Combine(modelDataFolderPath, "model.obj");
			string binFileDataFolderPath = Path.Combine(modelDataFolderPath, "binfiles");
			string binFileDataFilePath = Path.Combine(binFileDataFolderPath, "binfile.bin");

			// Create the directory for model data
			Directory.CreateDirectory(modelDataFolderPath);
			Directory.CreateDirectory(binFileDataFolderPath);


			// Save model data to file
			using (var fileStream = new FileStream(modelDataFilePath, FileMode.Create, FileAccess.Write))
			{
				await dto.objFile.CopyToAsync(fileStream);
			}

			// Save bin file if provided
			if (dto.binFile != null)
			{
				using (var fileStream = new FileStream(binFileDataFilePath, FileMode.Create, FileAccess.Write))
				{
					await dto.binFile.CopyToAsync(fileStream);
				}
			}

			// Prepare URLs to store in the database
			var modelDataUrl = $"/Models/{dto.modelId}/model.obj";
			var binFileDataUrl = dto.binFile != null ? $"/Models/{dto.modelId}/binfiles/binfile.bin" : null;

			

			Console.WriteLine("MODELID: " + dto.modelId);
			var command = new ModelCreateCommand()
			{
				ModelId = dto.modelId,
				ModelDataUrl = modelDataUrl,
				TextureId = dto.modelTexture,
				ProductId = dto.ProductId,
				ModelType = dto.modelType,
				BinFileDataUrl = binFileDataUrl,
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
		public async Task<IActionResult> GetModelInfo(Guid id)
		{
			var query = new GetModelById(id);

			ModelInfo result = await _mediator.Send(query);
			return result switch
			{
				not null => Ok(result),
				null => NotFound()
			};
		}

		[HttpGet]
		[Route("GetFileModelInfo/{id}")]
		public async Task<IActionResult> GetFileModelInfo(Guid id)
		{
			_logger.LogInformation($"Received request for model with id: {id}");

			var query = new GetModelById(id);

			ModelInfo result = await _mediator.Send(query);
			if (result != null)
			{
				_logger.LogInformation($"ModelInfo found for id: {id}");

				var rootPath = _environment.ContentRootPath;
				_logger.LogInformation($"Trying to access Root Path at path: {rootPath}");
				var relativePath = result.ModelDataUrl.TrimStart('/');

				var filePath = Path.Combine(rootPath, relativePath);
				_logger.LogInformation($"Trying to access file at path: {filePath}");

				if (System.IO.File.Exists(filePath))
				{
					_logger.LogInformation($"File found at path: {filePath}");
					var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
					var fileType = result.ModelType;
					var fileName = Path.GetFileName(filePath);


					return Ok(new
					{
						File = Convert.ToBase64String(fileBytes),
						FileType = result.ModelType,
					});
				}
			}
			return NotFound();
		}

		[HttpGet]
		[Route("DeleteModel/{id}")]
		public async Task<IActionResult> DeleteProduct(Guid id)
		{
			var query = new ModelDeleteCommand(id);

			CommandResult result = await _mediator.Send(query);
			return result switch
			{
				not null => Ok(result),
				null => NotFound()
			};
		}
	}
}
