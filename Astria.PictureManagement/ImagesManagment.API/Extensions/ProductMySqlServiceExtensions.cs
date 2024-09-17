using AuthenticationManagement.Authentication.Configuration;
using ImagesManagment.Infrastructure.DataContext;
using ImagesManagment.Infrastructure.Entities;
using ImagesManagment.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace ImagesManagment.API.Extensions
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

			services.AddDbContext<PictureDbContext>(o =>
			{
				o.UseMySql
				(
					connectionString.ConnectionString,
					new MySqlServerVersion(new Version(8, 0, 21))
				// b => b.MigrationsAssembly("OrderManagement.API")
				);
				o.EnableDetailedErrors();
			}, ServiceLifetime.Scoped);

			services.AddScoped<IPictureRepository<Picture>, PicturesRepository>();
			services.AddScoped<IModelTextureRepository<ModelPicture>, ModelPictureRepository>();
			services.AddScoped<IPictureRepository<PreviewPicture>, ProductPreviewImageRepository>();

			return services;
		}

		public static IApplicationBuilder UseProductMySqlMigration(this IApplicationBuilder app, PictureDbContext context)
		{
			context.Database.Migrate();
			return app;
		}
	}
}
