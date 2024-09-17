using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using AuthenticationManagement.Authentication.Configuration;
using UserProductLikesManagement.Infrastructure.DataContext;
using UserProductLikesManagement.Infrastructure.Repositories;
using UserProductLikesManagement.Infrastructure.Entities;
namespace UserProductLikesManagement.API.Extensions
{
	public static class ProductMySqlServiceExtensions
	{
		public static IServiceCollection AddProductServices(this IServiceCollection services, MySqlSettings settings)
		{
			var connectionString = new MySqlConnectionStringBuilder()
			{
				Server = settings.Url,
				Database = settings.Database,
				Port = (uint)settings.Port,
				Password = settings.Password,
				UserID = settings.Username,
				Pooling = false,
				SslMode = MySqlSslMode.Required
			};

			services.AddDbContext<UserProductLikesDbContext>(o =>
			{
				o.UseMySql
				(
					connectionString.ConnectionString,
					new MySqlServerVersion(new Version(8, 0, 21))
				// b => b.MigrationsAssembly("OrderManagement.API")
				);
				o.EnableDetailedErrors();
			}, ServiceLifetime.Scoped);
			// services.AddTransient<IUserProductLikesRepository<UserProductLikesManagement.Infrastructure.Entities.UserProductLike>, UserProductLikesManagement.Infrastructure.Repositories.UserProductLikesRepository>();
			services.AddScoped<IUserProductLikesRepository<UserProductLikesManagement.Infrastructure.Entities.UserProductLike>, UserProductLikesManagement.Infrastructure.Repositories.UserProductLikesRepository>();

			return services;
		}

		public static IApplicationBuilder UseProductMySqlMigration(this IApplicationBuilder app, UserProductLikesDbContext context)
		{
			context.Database.Migrate();
			return app;
		}
	}
}
