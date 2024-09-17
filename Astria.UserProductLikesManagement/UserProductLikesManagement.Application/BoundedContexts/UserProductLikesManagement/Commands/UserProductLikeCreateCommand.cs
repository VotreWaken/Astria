using MediatR;
using UserProductLikesManagement.Application.Results;

namespace UserProductLikesManagement.Application.BoundedContexts.UserProductLikesManagement.Commands
{
	public class UserProductLikeCreateCommand : IRequest<CommandResult>
	{
		public Guid UserId { get; set; }
		public Guid ProductId { get; set; }
	}
}
