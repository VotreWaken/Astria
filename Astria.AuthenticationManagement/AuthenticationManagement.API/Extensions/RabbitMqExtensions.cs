using Astria.Rabbitmq.Configuration;
using Astria.Rabbitmq.ProcessingConsumers;
using MassTransit;

namespace AuthenticationManagement.API.Extensions
{
	public static class RabbitMqExtensions
	{
		public static IServiceCollection AddRabbitMq(this IServiceCollection services, RabbitMqSettings rabbitMqSettings)
		{
			services.AddMassTransit(x =>
			{
				x.AddConsumer<ModelCreatedConsumer>();
				x.AddConsumer<TextureProcessingConsumer>();
				x.AddConsumer<ImageProcessingConsumer>();
				x.AddConsumer<UserImageConsumer>();

				x.UsingRabbitMq((context, cfg) =>
				{
					cfg.Host(rabbitMqSettings.Host, h =>
					{
						h.Username(rabbitMqSettings.Username);
						h.Password(rabbitMqSettings.Password);
					});

					cfg.ReceiveEndpoint("model-created-queue", e =>
					{
						e.ConfigureConsumer<ModelCreatedConsumer>(context);
					});

					cfg.ReceiveEndpoint("texture-processing-queue", e =>
					{
						e.ConfigureConsumer<TextureProcessingConsumer>(context);
					});

					cfg.ReceiveEndpoint("image-processing-queue", e =>
					{
						e.ConfigureConsumer<ImageProcessingConsumer>(context);
					});

					cfg.ReceiveEndpoint("user-image-processing-queue", e =>
					{
						e.ConfigureConsumer<UserImageConsumer>(context);
					});
				});
			});

			return services;
		}
	}
}
