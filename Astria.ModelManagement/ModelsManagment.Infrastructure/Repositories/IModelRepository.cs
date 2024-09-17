using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelsManagment.Infrastructure.Entities;

namespace ModelsManagment.Infrastructure.Repositories
{
	public interface IModelRepository
	{
		Task CreateModel(Model order);
		Task<Model> GetModelById(Guid orderId);
		Task<IEnumerable<Model>> GetModelsByProductId(Guid userId);
		Task UpdateModel(Model order);
		Task DeleteModel(Guid orderId);
	}
}
