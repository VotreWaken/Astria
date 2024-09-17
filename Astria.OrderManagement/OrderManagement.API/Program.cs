using Astria.EventSourcingRepository.Configuration;
using Astria.EventSourcingRepository.Repository;
using Astria.QueryRepository.Configuration;
using MediatR;
using OrderManagement.API.Extensions;
using OrderManagement.API.Middleware;
using OrderManagement.Application.BoundedContexts.Commands;
using OrderManagement.Application.Pipelines;
using System.Reflection;
using OrderManagement.Domain.BoundedContexts.OrderManagment.Aggregates;
using Astria.QueryRepository.Repository;
using OrderManagement.Application.BoundedContexts.QueryObjects;
using Astria.Rabbitmq.Configuration;

using AuthenticationManagement.Authentication.Configuration;
using UserProductLikesManagement.API.Extensions;
using Microsoft.AspNetCore.Http.Features;
namespace OrderManagement.API
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
				options.MultipartBodyLengthLimit = 300000000000; // 300 GB
			});

			builder.WebHost.ConfigureKestrel(options =>
			{
				options.Limits.MaxRequestBodySize = 300000000000; // 300 GB
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
			services.AddMongoDbService(Configuration.GetSection("MongoDb").Get<MongoDbSettings>());
			services.AddOrderServices(Configuration.GetSection("MySqlDb").Get<MySqlSettings>());
			services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(OrderCreateCommand).GetTypeInfo().Assembly));
			// services.AddMediatR(typeof(AdminAccountCreateCommand).Assembly);
			services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
			services.AddTransient(typeof(IPipelineBehavior<,>), typeof(MetricsBehaviour<,>));

			services.AddTransient<IEventSourcingRepository<Order>, EventSourcingRepository<Order>>();
			services.AddTransient<IProjectionRepository<OrderInfo>, MongoDbRepository<OrderInfo>>();
			services.AddTransient<IQueryRepository<OrderInfo>, MongoDbRepository<OrderInfo>>();
			services.AddTransient<IOrderRepository<OrderInfo>, OrderRepository<OrderInfo>>();


			services.AddRabbitMq(Configuration.GetSection("RabbitMq").Get<RabbitMqSettings>());
			// services.AddTransient<IEventSourcingRepository<TourStopPricingModel>, EventSourcingRepository<TourStopPricingModel>>();
		}

	}
}
