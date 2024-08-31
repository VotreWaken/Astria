using Celestial.EmailService.Services;

namespace AuthenticationManagement.API.Extensions
{
	public static class EmailServiceExtensions
	{
		public static IServiceCollection AddEmailService(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddScoped<IEmailService>(provider =>
				new DemoEmailService(
					provider.GetRequiredService<ILogger<DemoEmailService>>(),
					provider.GetRequiredService<IHttpContextAccessor>(),
					configuration
				));

			return services;
		}
	}
}
