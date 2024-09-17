using Astria.QueryRepository.Repository;
using MediatR;
using OrderManagement.Application.BoundedContexts.QueryObjects;

namespace OrderManagement.Application.BoundedContexts.Queries
{
	public class GetOrderByIdQuery : IRequest<OrderInfo>
	{
		public Guid Id { get; private set; }
		public GetOrderByIdQuery(Guid id)
		{
			Id = id;
		}
	}

	public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, OrderInfo>
	{
		private readonly IQueryRepository<OrderInfo> _repository;

		public GetOrderByIdQueryHandler(IQueryRepository<OrderInfo> repository)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		public async Task<OrderInfo> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
		{
			return await _repository.FindByIdAsync(request.Id);
		}
	}
}
