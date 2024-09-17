using Astria.QueryRepository.Repository;
using MediatR;
using OrderManagement.Application.BoundedContexts.QueryObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.BoundedContexts.Queries
{
	public class GetOrdersByUserId : IRequest<IEnumerable<OrderInfo>>
	{
		public Guid UserId { get; set; }

		public GetOrdersByUserId(Guid userId)
		{
			UserId = userId;
		}
	}

	public class GetOrdersByUserIdHandler : IRequestHandler<GetOrdersByUserId, IEnumerable<OrderInfo>>
	{
		private readonly IQueryRepository<OrderInfo> _repository;

		public GetOrdersByUserIdHandler(IQueryRepository<OrderInfo> repository)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		public async Task<IEnumerable<OrderInfo>> Handle(GetOrdersByUserId request, CancellationToken cancellationToken)
		{
			// Получаем все данные и конвертируем в список
			IEnumerable<OrderInfo> result = (await _repository.FindAllAsync(p => p.CustomerId == request.UserId)).ToList();

			Console.WriteLine("Result From MongoDb: " + result.Count());


			Console.WriteLine("Result Count: " + result.Count());

			return result;
		}
	}
}
