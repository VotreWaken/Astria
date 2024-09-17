using Astria.QueryRepository.Repository;
using ImagesManagment.Application.BoundedContexts.PreviewProductPictureManagment.QueryObjects;
using MediatR;

namespace ImagesManagment.Application.BoundedContexts.PreviewProductPictureManagment.Queries
{
	public class GetPreviewProductImageById : IRequest<ProductPreviewImageInfo>
	{
		public Guid Id { get; private set; }
		public GetPreviewProductImageById(Guid id)
		{
			Id = id;
		}
	}

	public class GetModelPictureByIdQueryHandler : IRequestHandler<GetPreviewProductImageById, ProductPreviewImageInfo>
	{
		private readonly IQueryRepository<ProductPreviewImageInfo> _repository;

		public GetModelPictureByIdQueryHandler(IQueryRepository<ProductPreviewImageInfo> repository)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		public async Task<ProductPreviewImageInfo> Handle(GetPreviewProductImageById request, CancellationToken cancellationToken)
		{
			return await _repository.FindByIdAsync(request.Id);
		}
	}
}
