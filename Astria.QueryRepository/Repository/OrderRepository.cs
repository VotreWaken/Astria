using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Astria.QueryRepository.Repository
{
	public class OrderRepository<T> : IOrderRepository<T> where T : IQueryEntity
	{
		private readonly IMongoDatabase _mongoDatabase;
		private static string CollectionName => typeof(T).Name;

		public OrderRepository(IMongoDatabase mongoDatabase)
		{
			_mongoDatabase = mongoDatabase;
		}

		public async Task<bool> IsOrderConfirmedByUserAsync(Guid userId, Guid productId)
		{
			var collection = _mongoDatabase.GetCollection<T>(CollectionName);

			var filter = Builders<T>.Filter.And(
				Builders<T>.Filter.Eq("CustomerId", userId),
				Builders<T>.Filter.Eq("ProductId", productId)
			);

			var isLiked = await collection.Find(filter).AnyAsync();

			return isLiked;
		}
	}
}
