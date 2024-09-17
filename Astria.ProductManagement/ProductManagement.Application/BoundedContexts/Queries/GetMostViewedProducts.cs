using Astria.QueryRepository.Repository;
using MediatR;
using ProductManagement.Application.BoundedContexts.QueryObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Application.BoundedContexts.Queries
{
	public class GetMostViewedProductInfosQuery : IRequest<List<ProductInfo>>
	{ }

	public class GetMostViewedProductInfosQueryHandler : IRequestHandler<GetMostViewedProductInfosQuery, List<ProductInfo>>
	{
		private readonly IQueryRepository<ProductInfo> _repository;

		public GetMostViewedProductInfosQueryHandler(IQueryRepository<ProductInfo> repository)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		public async Task<List<ProductInfo>> Handle(GetMostViewedProductInfosQuery request, CancellationToken cancellationToken)
		{
			// Получаем все продукты
			var products = await _repository.FindAllAsync();

			// Сортируем продукты по количеству просмотров в порядке убывания и берем только 6 самых популярных
			var sortedProducts = products
				.OrderByDescending(p => p.Views)
				.Take(6) // Ограничиваем количество возвращаемых элементов
				.ToList(); // Преобразуем в List

			return sortedProducts;
		}
	}
}
