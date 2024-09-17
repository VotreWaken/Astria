using Astria.Rabbitmq.Configuration;
using Astria.Rabbitmq.Events;
using ImagesManagment.API.Controllers;
using MongoDB.Driver.Core.Connections;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
namespace ImagesManagment.API.Listeners
{
	public class TextureProcessingListener : BackgroundService
	{
		private readonly IModel _channel;
		private readonly ILogger<TextureProcessingListener> _logger;

		public TextureProcessingListener(IConfiguration configuration, ILogger<TextureProcessingListener> logger)
		{
			_logger = logger;
			var rabbitMqSettings = configuration.GetSection("RabbitMq").Get<RabbitMqSettings>();

			var factory = new ConnectionFactory()
			{
				HostName = rabbitMqSettings.Host,
				UserName = rabbitMqSettings.Username,
				Password = rabbitMqSettings.Password
			};

			var connection = factory.CreateConnection();
			_channel = connection.CreateModel();

			// Создайте очередь с параметром durable
			_channel.QueueDeclare(queue: "texture-processing-queue", durable: true, exclusive: false, autoDelete: false, arguments: null);
		}

		protected override Task ExecuteAsync(CancellationToken stoppingToken)
		{
			var consumer = new EventingBasicConsumer(_channel);
			consumer.Received += (model, ea) =>
			{
				var body = ea.Body.ToArray();
				var message = Encoding.UTF8.GetString(body);
				_logger.LogInformation($"Received message: {message}");

				// Обработка сообщения
			};

			_channel.BasicConsume(queue: "texture-processing-queue", autoAck: true, consumer: consumer);

			return Task.CompletedTask;
		}

		public override void Dispose()
		{
			_channel?.Close();
			_channel?.Dispose();
			base.Dispose();
		}
	}
}
