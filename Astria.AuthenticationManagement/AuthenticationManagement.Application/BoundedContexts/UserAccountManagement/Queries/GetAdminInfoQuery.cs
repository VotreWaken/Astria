using AuthenticationManagement.Application.BoundedContexts.UserAccountManagement.QueryObjects;
using Astria.QueryRepository.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationManagement.Application.BoundedContexts.UserAccountManagement.Queries
{
	public class GetAdminInfoQuery : IRequest<AdminInfo>
	{
		public Guid Id { get; private set; }
		public GetAdminInfoQuery(Guid Id)
		{
			this.Id = Id;
		}
	}

	public class GetAdminInfoQueryHandler : IRequestHandler<GetAdminInfoQuery, AdminInfo>
	{
		private readonly IQueryRepository<AdminInfo> _repository;

		public GetAdminInfoQueryHandler(IQueryRepository<AdminInfo> repository)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		public async Task<AdminInfo> Handle(GetAdminInfoQuery request, CancellationToken cancellationToken)
		{
			return await _repository.FindByIdAsync(request.Id);
		}
	}
}
