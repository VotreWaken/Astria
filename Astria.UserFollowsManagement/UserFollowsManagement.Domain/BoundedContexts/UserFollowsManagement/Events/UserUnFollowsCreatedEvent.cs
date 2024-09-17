using Astria.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserFollowsManagement.Domain.BoundedContexts.UserFollowsManagement.Events
{
	public class UserUnFollowsCreatedEvent : DomainEvent
	{
		public Guid FollowerId { get; set; }
		public Guid FollowedId { get; set; }

		public UserUnFollowsCreatedEvent(Guid aggregateId, Guid followerId, Guid followedId)
			: base(aggregateId)
		{
			FollowerId = followerId;
			FollowedId = followedId;
		}
	}
}
