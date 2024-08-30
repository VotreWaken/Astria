using Astria.Rabbitmq.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astria.Rabbitmq.ProcessingConsumers
{
	public class ProductCreatedConsumer : IConsumer<ProductCreatedEvent>
	{
		private readonly ILogger<ProductCreatedConsumer> _logger;

		public ProductCreatedConsumer(ILogger<ProductCreatedConsumer> logger)
		{
			_logger = logger;
		}
		public async Task Consume(ConsumeContext<ProductCreatedEvent> context)
		{
			var message = context.Message;

			_logger.LogInformation($"Processing Product Consumer for Model: {message.ModelId}");


			// Публикация события ImageProcessingEvent
			var imageProcessingEvent = new ProductPreviewImageProcessingEvent
			{
				ProductId = message.ProductId,
				ImageData = null
			};
			await context.Publish(imageProcessingEvent);


			// Публикация события TextureProcessingEvent
			var textureProcessingEvent = new TextureProcessingEvent
			{
				ProductId = message.ProductId,
				ModelId = message.ModelId,
				BaseColorData = null,
				DisplacementData = null,
				EmissiveData = null,
				MetallicData = null,
				RoughnessData = null,
				NormalMapData = null,
			};
			await context.Publish(textureProcessingEvent);
		}
	}
}
