using Astria.QueryRepository.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImagesManagment.Application.BoundedContexts.PreviewProductPictureManagment.QueryObjects
{
	public class ProductPreviewImageInfo : IQueryEntity
	{
		public Guid Id { get; set; }
		public Guid ProductId { get; set; }
		public string Url { get; set; }
	}
}
