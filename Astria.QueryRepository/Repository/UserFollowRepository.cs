using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Astria.QueryRepository.Repository
{
	public class UserFollowRepository<T> : IUserFollowRepository<T> where T : IQueryEntity
	{
		private readonly IMongoDatabase _mongoDatabase;
		private static string CollectionName => typeof(T).Name;

		public UserFollowRepository(IMongoDatabase mongoDatabase)
		{
			_mongoDatabase = mongoDatabase;
		}
		public async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
		{
			var collection = _mongoDatabase.GetCollection<T>(CollectionName);

			var count = await collection.CountDocumentsAsync(predicate);

			return (int)count;
		}

		public async Task<bool> IsUserAlreadyFollowsAsync(Guid followerId, Guid followedId)
		{
			var collection = _mongoDatabase.GetCollection<T>(CollectionName);

			var filter = Builders<T>.Filter.And(
				Builders<T>.Filter.Eq("FollowerId", followerId),
				Builders<T>.Filter.Eq("FollowedId", followedId)
			);

			var isLiked = await collection.Find(filter).AnyAsync();

			return isLiked;
		}

		public async Task DeleteAsync(Guid followerId, Guid followedId)
		{
			try
			{
				var filter = Builders<T>.Filter.And(
					Builders<T>.Filter.Eq("FollowerId", followerId),
					Builders<T>.Filter.Eq("FollowedId", followedId)
				);

				var collection = _mongoDatabase.GetCollection<T>(CollectionName);
				var result = await collection.DeleteOneAsync(filter);

				if (result.DeletedCount == 0)
				{
					throw new Exception($"No document found with FollowerId {followerId} and FollowedId {followedId}.");
				}
			}
			catch (MongoWriteException ex)
			{
				throw new MongoDbException($"Cannot execute deletion of entity with FollowerId {followerId} and FollowedId {followedId}.", ex);
			}
		}
	}
}
