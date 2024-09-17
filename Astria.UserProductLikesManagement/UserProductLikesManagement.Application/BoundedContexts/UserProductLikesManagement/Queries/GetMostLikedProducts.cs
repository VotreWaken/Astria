using Astria.QueryRepository.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserProductLikesManagement.Application.BoundedContexts.UserProductLikesManagement.QueryObjects;

namespace UserProductLikesManagement.Application.BoundedContexts.UserProductLikesManagement.Queries
{
	public class GetMostLikedProducts : IRequest<List<ProductLikeInfo>>
	{
		public int NumberOfProducts { get; set; }
		public GetMostLikedProducts(int numberOfProducts)
		{
			NumberOfProducts = numberOfProducts;
		}
	}

	public class GetMostLikedProductsInfosQueryHandler : IRequestHandler<GetMostLikedProducts, List<ProductLikeInfo>>
	{
		private readonly IQueryRepository<UserProductLikesInfo> _repository;

		public GetMostLikedProductsInfosQueryHandler(IQueryRepository<UserProductLikesInfo> repository)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		public async Task<List<ProductLikeInfo>> Handle(GetMostLikedProducts request, CancellationToken cancellationToken)
		{
			var likes = await _repository.FindAllAsync();

			// Группируем по ProductId и считаем количество лайков для каждого продукта
			var productLikes = likes
				.GroupBy(like => like.ProductId)
				.Select(group => new ProductLikeInfo
				{
					ProductId = group.Key,
					ProductLikes = group.Count()
				})
				.OrderByDescending(p => p.ProductLikes)
				.Take(request.NumberOfProducts) // Оставляем только нужное количество продуктов
				.ToList();

			foreach (var product in productLikes)
			{
				Console.WriteLine($"ProductId: {product.ProductId}, ProductLikes: {product.ProductLikes}");
			}

			return productLikes;
		}
	}
}
