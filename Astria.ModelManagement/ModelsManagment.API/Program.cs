
using AuthenticationManagement.Authentication.Configuration;
using Astria.EventSourcingRepository.Configuration;
using Astria.EventSourcingRepository.Repository;
using Astria.QueryRepository.Configuration;
using Astria.QueryRepository.Repository;
using Astria.Rabbitmq.Configuration;
using MediatR;
using ModelsManagment.API.Extensions;
using ModelsManagment.API.Middleware;
using ModelsManagment.Application.BoundedContexts.Commands;
using ModelsManagment.Application.BoundedContexts.QueryObjects;
using ModelsManagment.Application.Pipelines;
using ModelsManagment.Domain.BoundedContexts.ModelsManagment.Aggregates;
using System.Reflection;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Http.Features;
namespace ModelsManagment.API
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			ConfigureServices(builder.Services, builder.Configuration);

			/*
			builder.WebHost.ConfigureKestrel(options =>
			{
				// Установка максимального размера тела запроса в байтах
				options.Limits.MaxRequestBodySize = 104857600; // 100 MB
			});
			*/

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
			services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ModelCreateCommand).GetTypeInfo().Assembly));
			// services.AddMediatR(typeof(AdminAccountCreateCommand).Assembly);
			services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
			services.AddTransient(typeof(IPipelineBehavior<,>), typeof(MetricsBehaviour<,>));

			services.AddTransient<IEventSourcingRepository<Model>, EventSourcingRepository<Model>>();
			services.AddTransient<IProjectionRepository<ModelInfo>, MongoDbRepository<ModelInfo>>();
			services.AddTransient<IQueryRepository<ModelInfo>, MongoDbRepository<ModelInfo>>();

			services.AddRabbitMq(Configuration.GetSection("RabbitMq").Get<RabbitMqSettings>());
			// services.AddTransient<IEventSourcingRepository<TourStopPricingModel>, EventSourcingRepository<TourStopPricingModel>>();
		}

	}
}
