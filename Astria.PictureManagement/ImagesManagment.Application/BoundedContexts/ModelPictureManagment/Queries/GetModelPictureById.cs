using Astria.QueryRepository.Repository;
using ImagesManagment.Application.BoundedContexts.ModelPictureManagment.QueryObjects;
using ImagesManagment.Application.BoundedContexts.UserImageManagment.Queries;
using ImagesManagment.Application.BoundedContexts.UserImageManagment.QueryObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImagesManagment.Application.BoundedContexts.ModelPictureManagment.Queries
{
	public class GetModelPictureById : IRequest<ModelPictureInfo>
	{
		public Guid Id { get; private set; }
		public GetModelPictureById(Guid id)
		{
			Id = id;
		}
	}

	public class GetModelPictureByIdQueryHandler : IRequestHandler<GetModelPictureById, ModelPictureInfo>
	{
		private readonly IQueryRepository<ModelPictureInfo> _repository;

		public GetModelPictureByIdQueryHandler(IQueryRepository<ModelPictureInfo> repository)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		public async Task<ModelPictureInfo> Handle(GetModelPictureById request, CancellationToken cancellationToken)
		{
			return await _repository.FindByIdAsync(request.Id);
		}
	}
}
