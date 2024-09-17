using Astria.QueryRepository.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImagesManagment.Application.BoundedContexts.ModelPictureManagment.QueryObjects
{
	public class ModelPictureInfo : IQueryEntity
	{
		public Guid Id { get; set; }
		public Guid ModelId { get; set; }
		public string BaseColorUrl { get; set; }
		public string NormalMapUrl { get; set; }
		public string DisplacementUrl { get; set; }
		public string MetallicUrl { get; set; }
		public string RoughnessUrl { get; set; }
		public string EmissiveUrl { get; set; }
	}
}
