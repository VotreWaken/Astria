using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCommentsManagement.Infrastructure.Repositories
{
	public interface IProductCommentsRepository<T> where T : class
	{
		Task<T> FindByIdAsync(Guid commentId);
		Task<List<Guid>> GetAllComments(Guid productId);
		Task CreateComment(Guid userId, Guid productId, string message);
		Task DeleteComment(Guid messageId);
		Task EditComment(Guid messageId, string newMessage);
	}
}
