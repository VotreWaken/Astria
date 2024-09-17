
using Astria.EventSourcingRepository.Configuration;
using Astria.EventSourcingRepository.Repository;
using Astria.QueryRepository.Configuration;
using Astria.QueryRepository.Repository;
using Astria.Rabbitmq.Configuration;
using AuthenticationManagement.Authentication.Configuration;
using MediatR;
using Microsoft.AspNetCore.Http.Features;
using System.Reflection;
using UserFollowsManagement.API.Extensions;
using UserFollowsManagement.API.Middleware;
using UserFollowsManagement.Application.BoundedContexts.UserFollowsManagement.Commands;
using UserFollowsManagement.Application.BoundedContexts.UserFollowsManagement.QueryObjects;
using UserFollowsManagement.Application.Pipelines;
using UserFollowsManagement.Domain.BoundedContexts.UserFollowsManagement.Aggregates;

namespace UserFollowsManagement.API
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			ConfigureServices(builder.Services, builder.Configuration);

			builder.Services.AddControllers();

			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();
			builder.Services.AddHttpClient();

			builder.Services.Configure<FormOptions>(options =>
			{
				options.MultipartBodyLengthLimit = 300000000000;
			});

			builder.WebHost.ConfigureKestrel(options =>
			{
				options.Limits.MaxRequestBodySize = 300000000000;
			});

			builder.Services.AddCors(options =>
			{
				options.AddPolicy("AllowAllOrigins",
					policy =>
					{
						policy.AllowAnyOrigin()
							  .AllowAnyMethod()
							  .AllowAnyHeader();
					});
			});

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseGlobalExceptionMiddleware();
			}

			app.UseSwaggerDocumentation();

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseCors("AllowAllOrigins");

			app.UseJwtBasedAuth();

			app.MapControllers();

			app.Run();
		}
		static public void ConfigureServices(IServiceCollection services, IConfiguration Configuration)
		{
			services.AddControllers();
			services.AddHttpContextAccessor();
			services.AddSwaggerDocumentation();



			services.Configure<JwtSettings>(Configuration.GetSection("JwtSettings"));
			services.AddJwtBasedAuth(Configuration.GetSection("JwtSettings").Get<JwtSettings>());
			services.AddEventStoreDbService(Configuration.GetSection("EventStoreDb").Get<EventStoreDbSettings>());
			services.AddProductServices(Configuration.GetSection("MySqlDb").Get<MySqlSettings>());
			services.AddMongoDbService(Configuration.GetSection("MongoDb").Get<MongoDbSettings>());
			services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(UserFollowCreateCommand).GetTypeInfo().Assembly));
			// services.AddMediatR(typeof(AdminAccountCreateCommand).Assembly);
			services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
			services.AddTransient(typeof(IPipelineBehavior<,>), typeof(MetricsBehaviour<,>));

			services.AddTransient<IEventSourcingRepository<UserFollows>, EventSourcingRepository<UserFollows>>();
			services.AddTransient<IProjectionRepository<UserFollowInfo>, MongoDbRepository<UserFollowInfo>>();

			services.AddTransient<IQueryRepository<UserFollowInfo>, MongoDbRepository<UserFollowInfo>>();
			services.AddTransient<IUserFollowRepository<UserFollowInfo>, UserFollowRepository<UserFollowInfo>>();

			services.AddRabbitMq(Configuration.GetSection("RabbitMq").Get<RabbitMqSettings>());
			// services.AddTransient<IEventSourcingRepository<TourStopPricingModel>, EventSourcingRepository<TourStopPricingModel>>();
		}
	}
}
