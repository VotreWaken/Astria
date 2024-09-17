﻿using AuthenticationManagement.Authentication.Configuration;
using AuthenticationManagement.Authentication.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ModelsManagment.API.Extensions
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
