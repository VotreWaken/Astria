using Astria.QueryRepository.Repository;
using AuthenticationManagement.Application.BoundedContexts.UserAccountManagement.QueryObjects;
using AuthenticationManagement.Domain.BoundedContexts.UserAccountManagement.Events;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationManagement.Application.BoundedContexts.UserAccountManagement.Projections
{
	public class AdminChangedUserNameProjection : INotificationHandler<AdminAccountChangeNameAndSurname>
	{
		private readonly IProjectionRepository<AdminInfo> _repository;

		public AdminChangedUserNameProjection(IProjectionRepository<AdminInfo> repository)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		public async Task Handle(AdminAccountChangeNameAndSurname @event, CancellationToken cancellationToken)
		{
			Console.WriteLine($"Projection for Admin Change Name {@event.FullName.First} And Surname {@event.FullName.First}") ;
			var admin = await _repository.FindByIdAsync(@event.AggregateId);
			if (admin is not null)
			{
				admin.Version = @event.AggregateVersion;
				admin.FirstName = @event.FullName.First;
				admin.LastName = @event.FullName.Last;
				// admin.Email = @event.Email;
				await _repository.UpdateAsync(admin);
			}
			else
			{

			}
		}
	}
}
