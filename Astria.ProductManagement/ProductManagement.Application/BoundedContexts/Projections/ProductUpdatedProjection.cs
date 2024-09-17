using Astria.QueryRepository.Repository;
using MediatR;
using ProductManagement.Application.BoundedContexts.QueryObjects;
using ProductManagement.Domain.BoundedContexts.ProductManagment.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Application.BoundedContexts.Projections
{
	public class ProductUpdatedProjection : INotificationHandler<ProductUpdatedEvent>
	{
		private readonly IProjectionRepository<ProductInfo> _repository;

		public ProductUpdatedProjection(IProjectionRepository<ProductInfo> repository)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		public async Task Handle(ProductUpdatedEvent @event, CancellationToken cancellationToken)
		{
			Console.WriteLine("Projection For Update");
			var order = new ProductInfo
			{
				Id = @event.AggregateId,
				Name = @event.Name,
				Date = @event.Date,
				Description = @event.Description,
				IsAvailable = @event.IsAvailable,
				Price = @event.Price,
				Version = @event.AggregateVersion,
				PreviewImageId = @event.PreviewImageId,
				ModelId = @event.ModelId,
				UserId = @event.UserId,
				Views = @event.Views,
			};

			await _repository.UpdateAsync(order);
		}
	}
}
