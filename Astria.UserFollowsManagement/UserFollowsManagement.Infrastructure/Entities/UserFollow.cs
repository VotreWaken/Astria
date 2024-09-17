using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserFollowsManagement.Infrastructure.Entities
{
	public class UserFollow
	{
		public Guid FollowerId { get; set; }
		public Guid FollowedId { get; set; }
	}
}
