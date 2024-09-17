using Astria.QueryRepository.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserFollowsManagement.Application.BoundedContexts.UserFollowsManagement.QueryObjects
{
	public class UserFollowInfo : IQueryEntity
	{
		public Guid Id { get; set; }
		public Guid FollowerId { get; set; }
		public Guid FollowedId { get; set; }
	}
}
