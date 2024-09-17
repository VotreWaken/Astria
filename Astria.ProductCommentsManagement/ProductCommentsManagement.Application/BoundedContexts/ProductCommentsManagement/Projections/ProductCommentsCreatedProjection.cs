using Astria.QueryRepository.Repository;
using MediatR;
using ProductCommentsManagement.Application.BoundedContexts.ProductCommentsManagement.QueryObjects;
using ProductCommentsManagement.Domain.BoundedContexts.ProductCommentsManagement.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCommentsManagement.Application.BoundedContexts.ProductCommentsManagement.Projections
{
	public class ProductCommentsCreatedProjection : INotificationHandler<ProductCommentsCreatedEvent>
	{
		private readonly IProjectionRepository<ProductCommentsInfo> _repository;

		public ProductCommentsCreatedProjection(IProjectionRepository<ProductCommentsInfo> repository)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		public async Task Handle(ProductCommentsCreatedEvent @event, CancellationToken cancellationToken)
		{
			var order = new ProductCommentsInfo
			{
				Id = @event.Id,
				UserId = @event.UserId,
				ProductId = @event.ProductId,
				CommentText = @event.CommentText,
				CreatedAt = @event.CreatedAt,
				ParentCommentId = @event.ParentCommentId,
			};
			Console.WriteLine("Projection for ProductId And UserId: " + order.ProductId + order.UserId);
			await _repository.InsertAsync(order);
		}
	}
}
