using MassTransit;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using Astria.Rabbitmq.Events;

namespace Astria.Rabbitmq.ProcessingConsumers
{
	public class ModelCreatedConsumer : IConsumer<ModelCreatedEvent>
	{
		private readonly ILogger<ModelCreatedConsumer> _logger;
		private readonly IHttpClientFactory _httpClientFactory;

		public ModelCreatedConsumer(IHttpClientFactory httpClientFactory, ILogger<ModelCreatedConsumer> logger)
		{
			_httpClientFactory = httpClientFactory;
			_logger = logger;
		}

		public async Task Consume(ConsumeContext<ModelCreatedEvent> context)
		{
			var message = context.Message;

			_logger.LogInformation($"Processing Model Consumer for Model: {message.ModelId}");
			_logger.LogInformation($"Processing ProductId Consumer for Model: {message.ProductId}");
			_logger.LogInformation($"Processing TextureId Consumer for Model: {message.TextureId}");

			try
			{
				var client = _httpClientFactory.CreateClient();

				// Создаем объект содержимого HTTP-запроса
				using (var content = new MultipartFormDataContent())
				{
					// Добавляем данные модели в виде байтового массива
					var modelByteArrayContent = new ByteArrayContent(message.ModelData);
					modelByteArrayContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
					content.Add(modelByteArrayContent, "objFile", "model.obj");

					// Добавляем другие необходимые параметры в качестве форм данных
					content.Add(new StringContent(message.TextureId.ToString()), "modelTexture");
					content.Add(new StringContent(message.ProductId.ToString()), "ProductId");
					content.Add(new StringContent(message.ModelId.ToString()), "modelId");
					content.Add(new StringContent(message.ModelType.ToString()), "modelType");

					// Добавляем BinFileData в виде байтового массива
					if (message.BinFileData != null)
					{
						var binFileByteArrayContent = new ByteArrayContent(message.BinFileData);
						binFileByteArrayContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
						content.Add(binFileByteArrayContent, "binFile", "model.bin");
					}

					// Отправляем запрос на создание модели через API контроллера
					var response = await client.PostAsync("https://localhost:7109/api/Models/CreateModel", content);

					if (response.IsSuccessStatusCode)
					{
						_logger.LogInformation($"Successfully processed model {message.ModelId}");
					}
					else
					{
						_logger.LogError($"Failed to process model {message.ModelId}. Status code: {response.StatusCode}");
					}
				}
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while processing model {message.ModelId}: {ex.Message}");
			}
		}
	}
}
