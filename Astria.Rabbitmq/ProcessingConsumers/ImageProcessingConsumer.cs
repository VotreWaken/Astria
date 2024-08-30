using MassTransit;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using System.Net.Http;
using Astria.Rabbitmq.Events;

namespace Astria.Rabbitmq.ProcessingConsumers
{
	public class ImageProcessingConsumer : IConsumer<ProductPreviewImageProcessingEvent>
	{
		private readonly ILogger<ImageProcessingConsumer> _logger;
		private readonly IHttpClientFactory _httpClientFactory;

		public ImageProcessingConsumer(ILogger<ImageProcessingConsumer> logger, IHttpClientFactory httpClientFactory)
		{
			_logger = logger;
			_httpClientFactory = httpClientFactory;
		}
		public async Task Consume(ConsumeContext<ProductPreviewImageProcessingEvent> context)
		{
			var message = context.Message;

			_logger.LogInformation($"Processing Preview Image for Product: {message.ProductId}");

			try
			{
				var client = _httpClientFactory.CreateClient();


				using (var content = new MultipartFormDataContent())
				{

					var byteArrayContent = new ByteArrayContent(message.ImageData);
					byteArrayContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
					content.Add(byteArrayContent, "file", "previewImage.png");

					Console.WriteLine("Image Preview Consumer ProductId: " + message.ProductId);

					content.Add(new StringContent(message.ProductId.ToString()), "ProductId");
					content.Add(new StringContent(message.ProductPreviewImageId.ToString()), "previewImageId");

					var response = await client.PostAsync($"https://localhost:7203/api/Images/CreatePreviewImage", content);

					if (response.IsSuccessStatusCode)
					{
						_logger.LogInformation($"Successfully processed Product {message.ProductId}");
					}
					else
					{
						_logger.LogError($"Failed to process product {message.ProductId}. Status code: {response.StatusCode}");
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
