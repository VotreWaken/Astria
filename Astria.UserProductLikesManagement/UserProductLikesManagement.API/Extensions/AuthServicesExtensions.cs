using AuthenticationManagement.Authentication.Configuration;
using AuthenticationManagement.Authentication.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace UserProductLikesManagement.API.Extensions
{
	public static class AuthServicesExtensions
	{
		public static IServiceCollection AddJwtBasedAuth(this IServiceCollection services, JwtSettings jwtSettings)
		{
			services
				.AddScoped<ITokenService, JwtTokenService>()
				.AddHttpContextAccessor()
				.AddAuthorization()
				.AddAuthentication(options =>
				{
					options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
					options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
					options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				})
				.AddJwtBearer(options =>
				{
					options.RequireHttpsMetadata = false;
					options.SaveToken = true;
					options.TokenValidationParameters = new TokenValidationParameters()
					{
						ValidateIssuer = true,
						ValidIssuer = jwtSettings.Issuer,
						ValidateAudience = true,
						ValidAudience = jwtSettings.Issuer,
						ValidateIssuerSigningKey = true,
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
						ValidateLifetime = true,
						ClockSkew = TimeSpan.Zero
					};

					options.Events = new JwtBearerEvents
					{
						OnMessageReceived = context =>
						{
							// Получаем токен из запроса
							var accessToken = context.Request.Query["access_token"];

							// Если это запрос к хабу SignalR
							var path = context.HttpContext.Request.Path;
							if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/notificationHub"))
							{
								context.Token = accessToken;

								// Устанавливаем UserIdentifier как userId из токена
								context.Principal = new ClaimsPrincipal(
									new ClaimsIdentity(new[]
									{
					new Claim(ClaimTypes.NameIdentifier, accessToken)
									})
								);
							}
							return Task.CompletedTask;
						},
					};
				});

			return services;
		}

		public static IApplicationBuilder UseJwtBasedAuth(this IApplicationBuilder app)
		{
			app.UseAuthentication();
			app.UseAuthorization();

			return app;
		}
	}
}
