using AuthenticationManagement.Authentication.Configuration;
using Astria.EventSourcingRepository.Configuration;
using Astria.EventSourcingRepository.Repository;
using Astria.QueryRepository.Configuration;
using Astria.QueryRepository.Repository;
using MediatR;
using Microsoft.AspNetCore.Http.Features;
using UserProductLikesManagement.Application.Pipelines;
using Astria.Rabbitmq.Configuration;
using UserProductLikesManagement.API.Middleware;
using UserProductLikesManagement.API.Extensions;
using UserProductLikesManagement.Application.BoundedContexts.UserProductLikesManagement.QueryObjects;
using UserProductLikesManagement.Application.BoundedContexts.UserProductLikesManagement.Commands;
using System.Reflection;
using UserProductLikesManagement.Domain.BoundedContexts.UserProductLikesManagement.Aggregates;
using UserProductLikesManagement.Infrastructure.Repositories;
using Astria.NotificationService.NotificationHub;

namespace UserProductLikesManagement.API
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

			/*
			builder.Services.AddCors(options =>
			{
				options.AddPolicy("AllowSpecificOrigin",
					builder =>
					{
						builder.WithOrigins("http://localhost:3000") // URL фронтенда
							   .AllowAnyHeader()
							   .AllowAnyMethod()
							   .AllowCredentials();
					});
			});
			*/
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

			app.UseWebSockets();

			app.UseCors("AllowAllOrigins");
			
			app.UseJwtBasedAuth();

			app.MapControllers();

			app.MapHub<NotificationHub>("/notificationHub");

			app.Run();
		}
		static public void ConfigureServices(IServiceCollection services, IConfiguration Configuration)
		{
			services.AddControllers();
			services.AddHttpContextAccessor();
			services.AddSwaggerDocumentation();

			services.AddSingleton<IConnectionMapping, ConnectionMapping>();

			services.AddSignalR();



			services.Configure<JwtSettings>(Configuration.GetSection("JwtSettings"));
			services.AddJwtBasedAuth(Configuration.GetSection("JwtSettings").Get<JwtSettings>());
			services.AddEventStoreDbService(Configuration.GetSection("EventStoreDb").Get<EventStoreDbSettings>());
			services.AddProductServices(Configuration.GetSection("MySqlDb").Get<MySqlSettings>());
			services.AddMongoDbService(Configuration.GetSection("MongoDb").Get<MongoDbSettings>());
			services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(UserProductLikeCreateCommand).GetTypeInfo().Assembly));
			// services.AddMediatR(typeof(AdminAccountCreateCommand).Assembly);
			services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
			services.AddTransient(typeof(IPipelineBehavior<,>), typeof(MetricsBehaviour<,>));

			services.AddTransient<IEventSourcingRepository<UserProductLike>, EventSourcingRepository<UserProductLike>>();
			services.AddTransient<IProjectionRepository<UserProductLikesInfo>, MongoDbRepository<UserProductLikesInfo>>();

			services.AddTransient<IQueryRepository<UserProductLikesInfo>, MongoDbRepository<UserProductLikesInfo>>();
			services.AddTransient<IUserProductLikeRepository<UserProductLikesInfo>, UserProductLikeRepository<UserProductLikesInfo>>();

			services.AddRabbitMq(Configuration.GetSection("RabbitMq").Get<RabbitMqSettings>());
			// services.AddTransient<IEventSourcingRepository<TourStopPricingModel>, EventSourcingRepository<TourStopPricingModel>>();
		}
	}
}
