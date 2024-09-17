using Astria.QueryRepository.Repository;
using ImagesManagment.Application.BoundedContexts.UserImageManagment.QueryObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImagesManagment.Application.BoundedContexts.UserImageManagment.Queries
{
    public class GetImageById : IRequest<ImageInfo>
    {
        public Guid Id { get; private set; }
        public GetImageById(Guid id)
        {
            Id = id;
        }
    }

    public class GetProductByIdQueryHandler : IRequestHandler<GetImageById, ImageInfo>
    {
        private readonly IQueryRepository<ImageInfo> _repository;

        public GetProductByIdQueryHandler(IQueryRepository<ImageInfo> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<ImageInfo> Handle(GetImageById request, CancellationToken cancellationToken)
        {
            return await _repository.FindByIdAsync(request.Id);
        }
    }
}
