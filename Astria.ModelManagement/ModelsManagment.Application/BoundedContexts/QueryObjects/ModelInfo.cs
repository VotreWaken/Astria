using Astria.QueryRepository.Repository;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsManagment.Application.BoundedContexts.QueryObjects
{
	public class ModelInfo : IQueryEntity
	{
		public Guid Id { get; set; }
		public Guid ProductId { get; set; }
		public string ModelDataUrl { get; set; }
		public string ModelType { get; set; }
		public Guid TextureId { get; set; }
	}
}
