using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Astria.QueryRepository.Repository
{
	public class UserProductLikeRepository<T> : IUserProductLikeRepository<T> where T : IQueryEntity
	{
		private readonly IMongoDatabase _mongoDatabase;
		private static string CollectionName => typeof(T).Name;

		public UserProductLikeRepository(IMongoDatabase mongoDatabase)
		{
			_mongoDatabase = mongoDatabase;
		}
		public async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
		{
			var collection = _mongoDatabase.GetCollection<T>(CollectionName);

			var count = await collection.CountDocumentsAsync(predicate);

			return (int)count;
		}

		public async Task<bool> IsProductLikedByUserAsync(Guid userId, Guid productId)
		{
			var collection = _mongoDatabase.GetCollection<T>(CollectionName);

			var filter = Builders<T>.Filter.And(
				Builders<T>.Filter.Eq("UserId", userId),
				Builders<T>.Filter.Eq("ProductId", productId)
			);

			var isLiked = await collection.Find(filter).AnyAsync();

			return isLiked;
		}
	}
}
