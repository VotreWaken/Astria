using Astria.EventSourcingRepository.Client;
using Astria.EventSourcingRepository.Configuration;
using EventStore.Client;
using System.Net.Security;

namespace OrderManagement.API.Extensions
{
	public static class EventStoreDbServiceExtensions
	{
		public static IServiceCollection AddEventStoreDbService(this IServiceCollection services, EventStoreDbSettings settings)
		{

			var eventStoreClientSettings = new EventStoreClientSettings
			{
				ConnectivitySettings =
				{
					Address = new Uri(settings.Uri)
				},
				DefaultCredentials = new UserCredentials(settings.Username, settings.Password),
				CreateHttpMessageHandler = () =>
				new SocketsHttpHandler
				{
					SslOptions = new SslClientAuthenticationOptions
					{
						RemoteCertificateValidationCallback = delegate { return true; }
					}
				}
			};

			services.AddSingleton(_ => new EventStoreClient(eventStoreClientSettings)).BuildServiceProvider();
			services.AddScoped<IEventSourcingClient, EventStoreDbClient>();

			return services;
		}
	}
}
