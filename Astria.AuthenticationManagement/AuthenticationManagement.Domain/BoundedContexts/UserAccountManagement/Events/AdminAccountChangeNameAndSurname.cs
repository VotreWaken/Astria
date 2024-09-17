using Astria.SharedKernel;
using AuthenticationManagement.Domain.BoundedContexts.UserAccountManagement.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationManagement.Domain.BoundedContexts.UserAccountManagement.Events
{
	public class AdminAccountChangeNameAndSurname : DomainEvent
	{
		public UserFullName FullName { get; protected set; }

		private AdminAccountChangeNameAndSurname()
		{ }

		public AdminAccountChangeNameAndSurname(Guid userId, UserFullName newName)
		{
			AggregateId = userId;
			FullName = newName;
		}
	}
}
