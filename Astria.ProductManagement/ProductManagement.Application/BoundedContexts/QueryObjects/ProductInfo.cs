using Astria.QueryRepository.Repository;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ProductManagement.Application.BoundedContexts.QueryObjects
{
	public class ProductInfo : IQueryEntity
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }

		[BsonRepresentation(BsonType.Decimal128)]
		public decimal Price { get; set; }
		public Guid ModelId {  get; set; }
		public bool IsAvailable { get; set; }
		public DateTime Date { get; set; }
		public Guid PreviewImageId { get; set; }
		public Guid UserId { get; set; }
		public int Views { get; set; }
		public long Version { get; set; }
	}
}
