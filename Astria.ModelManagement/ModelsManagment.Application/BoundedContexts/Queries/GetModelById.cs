using Astria.QueryRepository.Repository;
using MediatR;
using ModelsManagment.Application.BoundedContexts.QueryObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsManagment.Application.BoundedContexts.Queries
{
	public class GetModelById : IRequest<ModelInfo>
	{
		public Guid Id { get; private set; }
		public GetModelById(Guid id)
		{
			Id = id;
		}
	}

	public class GetProductByIdQueryHandler : IRequestHandler<GetModelById, ModelInfo>
	{
		private readonly IQueryRepository<ModelInfo> _repository;

		public GetProductByIdQueryHandler(IQueryRepository<ModelInfo> repository)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		public async Task<ModelInfo> Handle(GetModelById request, CancellationToken cancellationToken)
		{
			return await _repository.FindByIdAsync(request.Id);
		}
	}
}
