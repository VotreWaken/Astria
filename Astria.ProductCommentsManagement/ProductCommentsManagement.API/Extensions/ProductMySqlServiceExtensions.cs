using AuthenticationManagement.Authentication.Configuration;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using ProductCommentsManagement.Infrastructure.DataContext;
using ProductCommentsManagement.Infrastructure.Repositories;

namespace ProductCommentsManagement.API.Extensions
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

			services.AddDbContext<ProductCommentsDbContext>(o =>
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
			services.AddScoped<IProductCommentsRepository<ProductCommentsManagement.Infrastructure.Entities.ProductComments>, ProductCommentsManagement.Infrastructure.Repositories.ProductCommentsRepository>();

			return services;
		}

		public static IApplicationBuilder UseProductMySqlMigration(this IApplicationBuilder app, ProductCommentsDbContext context)
		{
			context.Database.Migrate();
			return app;
		}
	}
}
