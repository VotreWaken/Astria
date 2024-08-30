using Astria.Rabbitmq.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;

namespace Astria.Rabbitmq.ProcessingConsumers
{
	public class TextureProcessingConsumer : IConsumer<TextureProcessingEvent>
	{
		private readonly ILogger<TextureProcessingConsumer> _logger;
		private readonly IHttpClientFactory _httpClientFactory;

		public TextureProcessingConsumer(IHttpClientFactory httpClientFactory, ILogger<TextureProcessingConsumer> logger)
		{
			_httpClientFactory = httpClientFactory;
			_logger = logger;
		}

		public async Task Consume(ConsumeContext<TextureProcessingEvent> context)
		{
			var message = context.Message;
			_logger.LogInformation($"Processing Texture Consumer for Model: {message.ModelId}");

			var client = _httpClientFactory.CreateClient();

			// Список текстур для обработки
			var textures = new List<(byte[] Data, string Name, string FileName)>
	{
		(message.BaseColorData, "baseColorFile", "baseColor.png"),
		(message.NormalMapData, "normalMapFile", "normalMap.png"),
		(message.DisplacementData, "displacementFile", "displacement.png"),
		(message.MetallicData, "metallicFile", "metallic.png"),
		(message.RoughnessData, "roughnessFile", "roughness.png"),
		(message.EmissiveData, "emissiveFile", "emissive.png")
	};

			foreach (var (data, name, fileName) in textures)
			{
				if (data == null || data.Length == 0)
				{
					_logger.LogError($"The file {fileName} is empty for ModelId: {message.ModelId}");
					continue;
				}

				using (var content = new MultipartFormDataContent())
				{
					try
					{
						var byteArrayContent = new ByteArrayContent(data);
						byteArrayContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
						content.Add(byteArrayContent, "file", fileName);

						content.Add(new StringContent(name), "textureType"); // Обязательно добавьте тип текстуры
						content.Add(new StringContent(message.ModelId.ToString()), "modelId");

						var response = await client.PostAsync("https://localhost:7203/api/Images/CreateModelTextureImage", content);
						if (response.IsSuccessStatusCode)
						{
							_logger.LogInformation($"Successfully uploaded {name} for ModelId: {message.ModelId}");
						}
						else
						{
							var errorContent = await response.Content.ReadAsStringAsync();
							_logger.LogError($"Failed to upload {name} for ModelId: {message.ModelId}, StatusCode: {response.StatusCode}, Error: {errorContent}");
						}
					}
					catch (Exception ex)
					{
						_logger.LogError(ex, $"Exception occurred while processing {name}.");
					}
				}
			}
		}
	}

}

