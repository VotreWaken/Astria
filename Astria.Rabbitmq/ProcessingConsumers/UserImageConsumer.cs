using MassTransit;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using System.Net.Http;
using Astria.Rabbitmq.Events;

namespace Astria.Rabbitmq.ProcessingConsumers
{
	public class UserImageConsumer : IConsumer<UserImageCreatedEvent>
	{
		private readonly ILogger<UserImageConsumer> _logger;
		private readonly IHttpClientFactory _httpClientFactory;

		public UserImageConsumer(ILogger<UserImageConsumer> logger, IHttpClientFactory httpClientFactory)
		{
			_logger = logger;
			_httpClientFactory = httpClientFactory;
		}
		public async Task Consume(ConsumeContext<UserImageCreatedEvent> context)
		{
			var message = context.Message;

			_logger.LogInformation($"Processing User Image: {message.UserImageId}");

			try
			{
				var client = _httpClientFactory.CreateClient();

				// Создаем объект содержимого HTTP-запроса
				using (var content = new MultipartFormDataContent())
				{
					// Добавляем данные модели в виде байтового массива
					var byteArrayContent = new ByteArrayContent(message.ImageData);
					byteArrayContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
					content.Add(byteArrayContent, "file", "previewImage.png");

					Console.WriteLine("Image Preview Consumer ProductId: " + message.UserImageId);
					// Добавляем TextureId и ProductId в качестве форм данных
					content.Add(new StringContent(message.UserImageId.ToString()), "ImageId");

					// Отправляем запрос на создание модели через API контроллера
					var response = await client.PostAsync($"https://localhost:7203/api/Images/CreateUserImage", content);

					if (response.IsSuccessStatusCode)
					{
						_logger.LogInformation($"Successfully processed Product {message.UserImageId}");
					}
					else
					{
						_logger.LogError($"Failed to process product {message.UserImageId}. Status code: {response.StatusCode}");
					}
				}
			}
			catch (Exception ex)
			{
				_logger.LogError($"Exception occurred while processing ModelCreatedEvent: {ex.Message}");
			}
		}
	}
}
