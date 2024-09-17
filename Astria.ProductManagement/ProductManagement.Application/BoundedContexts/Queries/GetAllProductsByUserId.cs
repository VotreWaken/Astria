using Astria.QueryRepository.Repository;
using MediatR;
using ProductManagement.Application.BoundedContexts.QueryObjects;
using ProductManagement.Domain.BoundedContexts.ProductManagement.Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Application.BoundedContexts.Queries
{
	public class GetAllProductsByUserId : IRequest<(IEnumerable<ProductInfo> Items, int TotalCount)>
	{
		public Guid UserId { get; set; }
		public int Page { get; set; }
		public int PageSize { get; set; }
		public SortState SortOrder { get; set; }

		public GetAllProductsByUserId(Guid userId, int page, int pageSize, 
			SortState sortOrder
			)
		{
			UserId = userId;
			Page = page;
			PageSize = pageSize;
			SortOrder = sortOrder;
		}
	}

	public class GetAllProductsByUserIdHandler : IRequestHandler<GetAllProductsByUserId, (IEnumerable<ProductInfo> Items, int TotalCount)>
	{
		private readonly IQueryRepository<ProductInfo> _repository;

		public GetAllProductsByUserIdHandler(IQueryRepository<ProductInfo> repository)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		public async Task<(IEnumerable<ProductInfo> Items, int TotalCount)> Handle(GetAllProductsByUserId request, CancellationToken cancellationToken)
		{
			// Получаем все данные и конвертируем в список
			IEnumerable<ProductInfo> result = (await _repository.FindAllAsync(p => p.UserId == request.UserId)).ToList();

			Console.WriteLine("Result From MongoDb: " + result.Count());

			// Сортировка
			result = request.SortOrder switch
			{
				SortState.PriceAsc => result.OrderBy(p => p.Price),
				SortState.PriceDesc => result.OrderByDescending(p => p.Price),
				SortState.PublishedDateAsc => result.OrderBy(p => p.Date),
				SortState.PublishedDateDesc => result.OrderByDescending(p => p.Date),
				_ => result
			};

			// Пагинация
			int totalCount = result.Count();
			List<ProductInfo> items = result.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize).ToList();

			Console.WriteLine("Result Count: " + result.Count());

			foreach (var item in items)
			{
				Console.WriteLine(item.Name);
			}

			return (items, totalCount);
		}
	}
}
