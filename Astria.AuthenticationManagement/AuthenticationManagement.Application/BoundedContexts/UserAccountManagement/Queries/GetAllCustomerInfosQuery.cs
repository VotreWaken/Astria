﻿using AuthenticationManagement.Application.BoundedContexts.UserAccountManagement.QueryObjects;
using Astria.QueryRepository.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationManagement.Application.BoundedContexts.UserAccountManagement.Queries
{
	public class GetAllCustomerInfosQuery : IRequest<List<CustomerInfo>>
	{ }

	public class GetAllCustomerInfosQueryHandler : IRequestHandler<GetAllCustomerInfosQuery, List<CustomerInfo>>
	{
		private readonly IQueryRepository<CustomerInfo> _repository;

		public GetAllCustomerInfosQueryHandler(IQueryRepository<CustomerInfo> repository)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		public async Task<List<CustomerInfo>> Handle(GetAllCustomerInfosQuery request, CancellationToken cancellationToken)
		{
			var customers = await _repository.FindAllAsync();
			return customers?.ToList();
		}
	}
}
