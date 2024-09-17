using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserFollowsManagement.Application.Results;

namespace UserFollowsManagement.Application.BoundedContexts.UserFollowsManagement.Commands
{
	public class UserUnFollowsCreatedCommand : IRequest<CommandResult>
	{
		public Guid FollowerId { get; set; }
		public Guid FollowedId { get; set; }
	}
}
