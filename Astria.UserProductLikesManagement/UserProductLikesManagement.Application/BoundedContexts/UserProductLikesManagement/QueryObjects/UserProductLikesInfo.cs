using Astria.QueryRepository.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserProductLikesManagement.Application.BoundedContexts.UserProductLikesManagement.QueryObjects
{
	public class UserProductLikesInfo : IQueryEntity
	{
		public Guid Id { get; set; }
		public Guid UserId { get; set; }
		public Guid ProductId { get; set; }
	}
}
