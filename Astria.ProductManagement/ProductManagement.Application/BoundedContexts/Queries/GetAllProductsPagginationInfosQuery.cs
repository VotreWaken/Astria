using Astria.QueryRepository.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductManagement.Application.BoundedContexts.QueryObjects;
using ProductManagement.Domain.BoundedContexts.ProductManagement.Aggregates;
using System.Linq;

namespace ProductManagement.Application.BoundedContexts.Queries
{
	public class GetProductsByFiltersQuery : IRequest<(IEnumerable<ProductInfo> Items, int TotalCount)>
	{
		public string Position { get; set; }
		public decimal? MinPrice { get; set; }
		public decimal? MaxPrice { get; set; }
		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		public Guid? UserId { get; set; }
		public int Page { get; set; }
		public int PageSize { get; set; }
		public SortState SortOrder { get; set; }

		public GetProductsByFiltersQuery(string position, Guid? userId, int page, int pageSize, SortState sortOrder, decimal? minPrice,
		decimal? maxPrice,
		DateTime? startDate,
		DateTime? endDate)
		{
			Position = position;
			UserId = userId;
			Page = page;
			PageSize = pageSize;
			SortOrder = sortOrder;
			MinPrice = minPrice;
			MaxPrice = maxPrice;
			StartDate = startDate;
			EndDate = endDate;
		}
	}

	public class GetProductsByFiltersHandler : IRequestHandler<GetProductsByFiltersQuery, (IEnumerable<ProductInfo> Items, int TotalCount)>
	{
		private readonly IQueryRepository<ProductInfo> _repository;

		public GetProductsByFiltersHandler(IQueryRepository<ProductInfo> repository)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		public async Task<(IEnumerable<ProductInfo> Items, int TotalCount)> Handle(GetProductsByFiltersQuery request, CancellationToken cancellationToken)
		{
			// Получаем все данные и конвертируем в список
			IEnumerable<ProductInfo> result = (await _repository.FindAllAsync()).ToList();

			Console.WriteLine("Result From MongoDb: " + result.Count());

			// Фильтрация
			if (!string.IsNullOrEmpty(request.Position))
			{
				result = result.Where(p => p.Name.Contains(request.Position));
			}

			if (request.MinPrice.HasValue)
			{
				result = result.Where(p => p.Price >= request.MinPrice.Value);
			}

			if (request.MaxPrice.HasValue)
			{
				result = result.Where(p => p.Price <= request.MaxPrice.Value);
			}

			if (request.StartDate.HasValue)
			{
				result = result.Where(p => p.Date >= request.StartDate.Value);
			}

			if (request.EndDate.HasValue)
			{
				result = result.Where(p => p.Date <= request.EndDate.Value);
			}

			// UserId
			if (request.UserId.HasValue && request.UserId != Guid.Empty)
			{
				result = result.Where(p => p.UserId == request.UserId);
			}
			
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
