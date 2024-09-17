using Astria.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// UserLikeProductCreatedEvent
namespace UserProductLikesManagement.Domain.BoundedContexts.UserProductLikesManagement.Events
{
	public class UserLikeProductCreatedEvent : DomainEvent
	{
		public Guid UserId { get; set; }
		public Guid ProductId { get; set; }

		public UserLikeProductCreatedEvent(Guid aggregateId, Guid userId, Guid productId)
			: base(aggregateId)
		{
			UserId = userId;
			ProductId = productId;
		}
	}
}
