﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserProductLikesManagement.Infrastructure.Entities
{
	public class UserProductLike
	{
		public Guid UserId { get; set; }
		public Guid ProductId { get; set; }
	}
}
