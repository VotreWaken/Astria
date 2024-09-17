using Astria.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserFollowsManagement.Domain.BoundedContexts.UserFollowsManagement.Events
{
	public class UserFollowsCreatedEvent : DomainEvent
	{
		public Guid FollowerId { get; set; }
		public Guid FollowedId { get; set; }

		public UserFollowsCreatedEvent(Guid aggregateId, Guid followerId, Guid followedId)
			: base(aggregateId)
		{
			FollowerId = followerId;
			FollowedId = followedId;
		}
	}
}
