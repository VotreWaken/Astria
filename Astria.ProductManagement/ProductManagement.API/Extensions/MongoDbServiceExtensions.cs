using Astria.QueryRepository.Configuration;
using MongoDB.Driver;

namespace ProductManagement.API.Extensions
{
	public static class MongoDbServiceExtensions
	{
		public static IServiceCollection AddMongoDbService(this IServiceCollection services, MongoDbSettings settings)
		{
			services.AddSingleton(x => new MongoClient($"mongodb://{settings.Username}:{settings.Password}@{settings.Url}:{settings.Port}/"));
			services.AddSingleton(x => x.GetService<MongoClient>().GetDatabase(settings.Database));

			return services;
		}
	}
}
