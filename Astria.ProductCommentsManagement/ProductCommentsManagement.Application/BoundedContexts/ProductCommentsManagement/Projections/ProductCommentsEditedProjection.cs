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
	public class ProductCommentsEditedProjection : INotificationHandler<ProductCommentsEditedEvent>
	{
		private readonly IProjectionRepository<ProductCommentsInfo> _repository;

		public ProductCommentsEditedProjection(IProjectionRepository<ProductCommentsInfo> repository)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		public async Task Handle(ProductCommentsEditedEvent @event, CancellationToken cancellationToken)
		{
			try
			{
				Console.WriteLine("Edited Projection with Id: " + @event.Id);
				var comment = await _repository.FindByIdAsync(@event.Id);

				if (comment != null)
				{
					comment.CommentText = @event.NewCommentText;
					comment.UpdatedAt = DateTime.UtcNow;
					await _repository.UpdateAsync(comment);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				throw;
			}

		}
	}
}
