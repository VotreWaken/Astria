using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using ProductManagement.Infrastructure.DataContext;
using ProductManagement.Infrastructure.Repositories;
using AuthenticationManagement.Authentication.Configuration;
namespace ProductManagement.API.Extensions
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

			services.AddDbContext<ProductDbContext>(o =>
			{
				o.UseMySql
				(
					connectionString.ConnectionString,
					new MySqlServerVersion(new Version(8, 0, 21))
				// b => b.MigrationsAssembly("OrderManagement.API")
				);
				o.EnableDetailedErrors();
			}, ServiceLifetime.Scoped);

			services.AddScoped<IProductRepository, ProductRepository>();

			return services;
		}

		public static IApplicationBuilder UseProductMySqlMigration(this IApplicationBuilder app, ProductDbContext context)
		{
			context.Database.Migrate();
			return app;
		}
	}
}
