using AuthenticationManagement.Authentication.Configuration;
using Astria.EventSourcingRepository.Configuration;
using Astria.EventSourcingRepository.Repository;
using Astria.QueryRepository.Configuration;
using Astria.QueryRepository.Repository;
using Astria.Rabbitmq.Configuration;
using ImagesManagment.API.Extensions;
using ImagesManagment.API.Middleware;
using ImagesManagment.Application.BoundedContexts.ModelPictureManagment.QueryObjects;
using ImagesManagment.Application.BoundedContexts.PreviewProductPictureManagment.QueryObjects;
using ImagesManagment.Application.BoundedContexts.UserImageManagment.Commands;
using ImagesManagment.Application.BoundedContexts.UserImageManagment.QueryObjects;
using ImagesManagment.Application.Pipelines;
using ImagesManagment.Domain.BoundedContexts.ImageManagment.Aggregates;
using ImagesManagment.Domain.BoundedContexts.ModelTextureManagment.Aggregates;
using ImagesManagment.Domain.BoundedContexts.ProductPreviewImageManagment.Aggregates;
using MediatR;
using Microsoft.Extensions.FileProviders;
using System.Reflection;
using Microsoft.AspNetCore.Http.Features;

namespace ImagesManagment.API
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

			app.UseCors("AllowAllOrigins");

			app.UseStaticFiles(new StaticFileOptions
			{
				FileProvider = new PhysicalFileProvider(
					Path.Combine(app.Environment.ContentRootPath, "images")),
				RequestPath = "/images"
			});

			app.UseStaticFiles(new StaticFileOptions
			{
				FileProvider = new PhysicalFileProvider(
					Path.Combine(app.Environment.ContentRootPath, "PreviewImages")),
				RequestPath = "/PreviewImages"
			});

			app.UseStaticFiles(new StaticFileOptions
			{
				FileProvider = new PhysicalFileProvider(
					Path.Combine(app.Environment.ContentRootPath, "UserImages")),
				RequestPath = "/UserImages"
			});

			app.UseSwaggerDocumentation();

			app.UseHttpsRedirection();

			app.UseRouting();

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
			services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ImageCreateCommand).GetTypeInfo().Assembly));
			// services.AddMediatR(typeof(AdminAccountCreateCommand).Assembly);
			services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
			services.AddTransient(typeof(IPipelineBehavior<,>), typeof(MetricsBehaviour<,>));

			services.AddTransient<IEventSourcingRepository<Picture>, EventSourcingRepository<Picture>>();
			services.AddTransient<IProjectionRepository<ImageInfo>, MongoDbRepository<ImageInfo>>();
			services.AddTransient<IQueryRepository<ImageInfo>, MongoDbRepository<ImageInfo>>();

			services.AddTransient<IEventSourcingRepository<ModelPicture>, EventSourcingRepository<ModelPicture>>();
			services.AddTransient<IProjectionRepository<ModelPictureInfo>, MongoDbRepository<ModelPictureInfo>>();
			services.AddTransient<IQueryRepository<ModelPictureInfo>, MongoDbRepository<ModelPictureInfo>>();
			// services.AddTransient<IEventSourcingRepository<TourStopPricingModel>, EventSourcingRepository<TourStopPricingModel>>();

			services.AddTransient<IEventSourcingRepository<PreviewProductPicture>, EventSourcingRepository<PreviewProductPicture>>();
			services.AddTransient<IProjectionRepository<ProductPreviewImageInfo>, MongoDbRepository<ProductPreviewImageInfo>>();
			services.AddTransient<IQueryRepository<ProductPreviewImageInfo>, MongoDbRepository<ProductPreviewImageInfo>>();

			// Конфигурация RabbitMQ
			services.AddRabbitMq(Configuration.GetSection("RabbitMq").Get<RabbitMqSettings>());
		}

	}
}
