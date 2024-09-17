using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImagesManagment.Infrastructure.Entities
{
	public class PreviewPicture
	{
		public Guid PreviewImageId { get; set; }
		public Guid ProductId { get; set; }
		public string ImageUrl { get; set; }
	}
}
