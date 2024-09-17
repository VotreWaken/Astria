using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Astria.QueryRepository.Repository
{
	public class ProductCommentsRepository<T> : IProductCommentsRepository<T> where T : IQueryEntity
	{
		private readonly IMongoDatabase _mongoDatabase;
		private static string CollectionName => typeof(T).Name;

		public ProductCommentsRepository(IMongoDatabase mongoDatabase)
		{
			_mongoDatabase = mongoDatabase;
		}
		public async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
		{
			var collection = _mongoDatabase.GetCollection<T>(CollectionName);

			var count = await collection.CountDocumentsAsync(predicate);

			return (int)count;
		}
	}
}
