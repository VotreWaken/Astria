﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserProductLikesManagement.Application.BoundedContexts.UserProductLikesManagement.QueryObjects
{
	public class ProductLikeInfo
	{
		public Guid ProductId { get; set; }
		public int ProductLikes { get; set; }
	}
}
