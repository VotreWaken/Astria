using Astria.QueryRepository.Repository;
using MediatR;
using ProductManagement.Application.BoundedContexts.QueryObjects;
using ProductManagement.Domain.BoundedContexts.ProductManagement.Events;

namespace ProductManagement.Application.BoundedContexts.Projections
{
	public class ProductDeletedProjection : INotificationHandler<ProductDeletedEvent>
	{
		private readonly IProjectionRepository<ProductInfo> _repository;

		public ProductDeletedProjection(IProjectionRepository<ProductInfo> repository)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
			Console.WriteLine("Start");
		}

		public async Task Handle(ProductDeletedEvent @event, CancellationToken cancellationToken)
		{
			try
			{
				Console.WriteLine("Projection Id: " + @event.AggregateId);
				await _repository.DeleteAsync(@event.Id);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				throw;
			}

		}
	}
}
