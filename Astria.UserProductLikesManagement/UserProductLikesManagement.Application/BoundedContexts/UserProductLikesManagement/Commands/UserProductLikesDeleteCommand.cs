using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserProductLikesManagement.Application.Results;

namespace UserProductLikesManagement.Application.BoundedContexts.UserProductLikesManagement.Commands
{
	public class UserProductLikesDeleteCommand : IRequest<CommandResult>
	{
		public Guid UserId { get; set; }
		public Guid ProductId { get; set; }
	}
}
