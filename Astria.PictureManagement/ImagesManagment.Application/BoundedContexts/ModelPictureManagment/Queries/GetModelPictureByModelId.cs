using Astria.QueryRepository.Repository;
using ImagesManagment.Application.BoundedContexts.ModelPictureManagment.QueryObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImagesManagment.Application.BoundedContexts.ModelPictureManagment.Queries
{
	public class GetModelPictureByModelId : IRequest<ModelPictureInfo>
	{
		public Guid ModelId { get; set; }
		public GetModelPictureByModelId(Guid id)
		{
			ModelId = id;
		}
	}

	public class GetModelPictureByModelIdQueryHandler : IRequestHandler<GetModelPictureByModelId, ModelPictureInfo>
	{
		private readonly IQueryRepository<ModelPictureInfo> _repository;

		public GetModelPictureByModelIdQueryHandler(IQueryRepository<ModelPictureInfo> repository)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		public async Task<ModelPictureInfo> Handle(GetModelPictureByModelId request, CancellationToken cancellationToken)
		{
			var result = await _repository.FindAllAsync(p => p.ModelId == request.ModelId);
			return result.SingleOrDefault();
		}
	}
}
