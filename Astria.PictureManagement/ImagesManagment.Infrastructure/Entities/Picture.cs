﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImagesManagment.Infrastructure.Entities
{
	public class Picture
	{
		public Guid ImageId { get; set; }
		public string ImageUrl { get; set; }
	}
}
