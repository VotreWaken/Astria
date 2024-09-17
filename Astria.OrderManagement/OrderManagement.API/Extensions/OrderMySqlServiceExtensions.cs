using AuthenticationManagement.Authentication.Configuration;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using OrderManagement.Infrastructure.DataContext;
using OrderManagement.Infrastructure.Repositories;

namespace OrderManagement.API.Extensions
{
	public static class OrderMySqlServiceExtensions
	{
		public static IServiceCollection AddOrderServices(this IServiceCollection services, MySqlSettings settings)
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

			services.AddDbContext<OrderDbContext>(o =>
			{
				o.UseMySql
				(
					connectionString.ConnectionString,
					new MySqlServerVersion(new Version(8, 0, 21))
					// b => b.MigrationsAssembly("OrderManagement.API")
				);
				o.EnableDetailedErrors();
			}, ServiceLifetime.Scoped);

			services.AddScoped<IOrderRepository, OrderRepository>();

			return services;
		}

		public static IApplicationBuilder UseOrderMySqlMigration(this IApplicationBuilder app, OrderDbContext context)
		{
			context.Database.Migrate();
			return app;
		}
	}
}
