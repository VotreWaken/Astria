using Astria.QueryRepository.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserProductLikesManagement.Application.BoundedContexts.UserProductLikesManagement.QueryObjects;

namespace UserProductLikesManagement.Application.BoundedContexts.UserProductLikesManagement.Queries
{
	public class IsProductLikeByUserId : IRequest<bool>
	{
		public Guid ProductId;
		public Guid UserId;

		public IsProductLikeByUserId(Guid productId, Guid userId)
		{
			ProductId = productId;
			UserId = userId;
		}
	}

	public class IsProductLikeByUserIdInfosQueryHandler : IRequestHandler<IsProductLikeByUserId, bool>
	{
		private readonly IUserProductLikeRepository<UserProductLikesInfo> _repository;

		public IsProductLikeByUserIdInfosQueryHandler(IUserProductLikeRepository<UserProductLikesInfo> repository)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		public async Task<bool> Handle(IsProductLikeByUserId request, CancellationToken cancellationToken)
		{
			return await _repository.IsProductLikedByUserAsync(request.UserId, request.ProductId);
		}
	}
}
