using Astria.QueryRepository.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductCommentsManagement.Application.BoundedContexts.ProductCommentsManagement.QueryObjects;
using ProductCommentsManagement.Domain.BoundedContexts.ProductCommentsManagement.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCommentsManagement.Application.BoundedContexts.ProductCommentsManagement.Projections
{
	internal class ProductCommentsDeletedProjection : INotificationHandler<ProductCommentsDeletedEvent>
	{
		private readonly IProjectionRepository<ProductCommentsInfo> _repository;

		public ProductCommentsDeletedProjection(IProjectionRepository<ProductCommentsInfo> repository)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		public async Task Handle(ProductCommentsDeletedEvent @event, CancellationToken cancellationToken)
		{
			try
			{
				Console.WriteLine("Deleted Projection with Id: " +  @event.Id);
				var comment = await _repository.FindByIdAsync(@event.Id);

				if (comment != null)
				{
					comment.IsDeleted = true;
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
