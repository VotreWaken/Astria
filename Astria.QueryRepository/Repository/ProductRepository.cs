using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Astria.QueryRepository.Repository
{
	public class ProductRepository<T> : IProductRepository<T> where T : IQueryEntity
	{
		private readonly IMongoDatabase _mongoDatabase;
		private static string CollectionName => typeof(T).Name;

		public ProductRepository(IMongoDatabase mongoDatabase)
		{
			_mongoDatabase = mongoDatabase;
		}
		public async Task<T> FindByIdAsync(Guid id)
		{
			return await _mongoDatabase.GetCollection<T>(CollectionName)
				.Find(x => x.Id == id)
				.SingleOrDefaultAsync();
		}

		public async Task IncrementFieldAsync(Guid id, string fieldName)
		{
			var collection = _mongoDatabase.GetCollection<T>(CollectionName);

			var filter = Builders<T>.Filter.Eq("Id", id);
			var update = Builders<T>.Update.Inc(fieldName, 1);

			await collection.UpdateOneAsync(filter, update);
		}
	}
}
