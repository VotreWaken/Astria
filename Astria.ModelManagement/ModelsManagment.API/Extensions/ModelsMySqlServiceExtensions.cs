using AuthenticationManagement.Authentication.Configuration;
using Microsoft.EntityFrameworkCore;
using ModelsManagment.Infrastructure.DataContext;
using ModelsManagment.Infrastructure.Repositories;
using MySqlConnector;

namespace ModelsManagment.API.Extensions
{
	public static class ModelsMySqlServiceExtensions
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

			services.AddDbContext<ModelDbContext>(o =>
			{
				o.UseMySql
				(
					connectionString.ConnectionString,
					new MySqlServerVersion(new Version(8, 0, 21))
				// b => b.MigrationsAssembly("OrderManagement.API")
				);
				o.EnableDetailedErrors();
			}, ServiceLifetime.Scoped);

			services.AddScoped<IModelRepository, ModelRepository>();

			return services;
		}

		public static IApplicationBuilder UseProductMySqlMigration(this IApplicationBuilder app, ModelDbContext context)
		{
			context.Database.Migrate();
			return app;
		}
	}
}
