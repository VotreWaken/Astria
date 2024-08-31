using Astria.EventSourcingRepository.Configuration;
using Astria.EventSourcingRepository.Repository;
using Astria.QueryRepository.Configuration;
using Astria.QueryRepository.Repository;
using MediatR;
using System.Reflection;
using AuthenticationManagement.Authentication.Repository;
using Microsoft.EntityFrameworkCore;
using Astria.Rabbitmq.Configuration;
using AuthenticationManagement.Domain.BoundedContexts.UserAccountManagement.Aggregates;
using AuthenticationManagement.Application.Pipelines;
using AuthenticationManagement.Application.BoundedContexts.UserAccountManagement.QueryObjects;
using AuthenticationManagement.Application.BoundedContexts.UserAccountManagement.Commands;
using AuthenticationManagement.Authentication.Configuration;
using AuthenticationManagement.API.Extensions;
using AuthenticationManagement.API.Middleware;
namespace AuthenticationManagement.API
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
			services.AddEmailService(Configuration);
			services.AddIdenityServices(Configuration.GetSection("MySqlDb").Get<MySqlSettings>());
			services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(AdminAccountCreateCommand).GetTypeInfo().Assembly));
			// services.AddMediatR(typeof(AdminAccountCreateCommand).Assembly);
			services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
			services.AddTransient(typeof(IPipelineBehavior<,>), typeof(MetricsBehaviour<,>));

			services.AddTransient<IEventSourcingRepository<Admin>, EventSourcingRepository<Admin>>();
			services.AddTransient<IProjectionRepository<AdminInfo>, MongoDbRepository<AdminInfo>>();
			services.AddTransient<IQueryRepository<AdminInfo>, MongoDbRepository<AdminInfo>>();

			services.AddTransient<IEventSourcingRepository<Customer>, EventSourcingRepository<Customer>>();
			services.AddTransient<IProjectionRepository<CustomerInfo>, MongoDbRepository<CustomerInfo>>();
			services.AddTransient<IQueryRepository<CustomerInfo>, MongoDbRepository<CustomerInfo>>();

			// services.AddTransient<IEventSourcingRepository<TourStopPricingModel>, EventSourcingRepository<TourStopPricingModel>>();

			services.AddRabbitMq(Configuration.GetSection("RabbitMq").Get<RabbitMqSettings>());
		}

	}
}
