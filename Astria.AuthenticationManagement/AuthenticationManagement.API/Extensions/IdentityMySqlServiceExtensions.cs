using AuthenticationManagement.Authentication.Configuration;
using AuthenticationManagement.Authentication.Entities;
using AuthenticationManagement.Authentication.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace AuthenticationManagement.API.Extensions
{
	public static class IdentityMySqlServiceExtensions
	{
		public static IServiceCollection AddIdenityServices(this IServiceCollection services, MySqlSettings settings)
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

			services.AddDbContext<IdentityContext>(o =>
			{
				o.UseMySql
				(
					connectionString.ConnectionString,
					new MySqlServerVersion(new Version(8, 0, 21)),
					b => b.MigrationsAssembly("Celestial.API")
				);
				o.EnableDetailedErrors();
			},
				ServiceLifetime.Scoped);

			services.AddIdentity<User, Role>(o =>
			{
				// Password settings.
				o.Password.RequiredLength = 6;
				o.Password.RequireDigit = false;
				o.Password.RequireNonAlphanumeric = false;
				o.Password.RequireUppercase = false;

				// User settings.
				o.User.RequireUniqueEmail = true;

				// SignIn settings.
				o.SignIn.RequireConfirmedEmail = true;
			})
				.AddEntityFrameworkStores<IdentityContext>()
				.AddDefaultTokenProviders();

			services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();

			return services;
		}

		public static IApplicationBuilder UseIdentityMySqlMigration(this IApplicationBuilder app, IdentityContext context)
		{
			context.Database.Migrate();

			return app;
		}
	}
}
