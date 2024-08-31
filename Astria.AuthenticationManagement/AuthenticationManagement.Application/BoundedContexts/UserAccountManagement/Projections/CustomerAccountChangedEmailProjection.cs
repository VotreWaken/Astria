using AuthenticationManagement.Application.BoundedContexts.UserAccountManagement.QueryObjects;
using AuthenticationManagement.Domain.BoundedContexts.UserAccountManagement.Events;
using Astria.QueryRepository.Repository;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AuthenticationManagement.Application.BoundedContexts.UserAccountManagement.Projections
{
	public class CustomerAccountChangedEmailProjection : INotificationHandler<CustomerAccountChangedEmailEvent>
	{
		private readonly IProjectionRepository<CustomerInfo> _repository;

		public CustomerAccountChangedEmailProjection(IProjectionRepository<CustomerInfo> repository)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		public async Task Handle(CustomerAccountChangedEmailEvent @event, CancellationToken cancellationToken)
		{
			var customer = await _repository.FindByIdAsync(@event.AggregateId);
			if (customer is not null)
			{
				customer.Version = @event.AggregateVersion;
				await _repository.UpdateAsync(customer);
			}
			else
			{

			}
		}
	}
}
