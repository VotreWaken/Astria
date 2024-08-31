using Astria.QueryRepository.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationManagement.Application.BoundedContexts.UserAccountManagement.QueryObjects
{
	public class AdminInfo : QueryEntity
	{
		public string Email { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string HashedPassword { get; set; }
		public Guid AdminImageId { get; set; }

	}
}
