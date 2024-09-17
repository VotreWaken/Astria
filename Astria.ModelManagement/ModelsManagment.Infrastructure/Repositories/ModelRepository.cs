using Microsoft.EntityFrameworkCore;
using ModelsManagment.Infrastructure.DataContext;
using ModelsManagment.Infrastructure.Entities;

namespace ModelsManagment.Infrastructure.Repositories
{
	public class ModelRepository : IModelRepository
	{
		private readonly ModelDbContext _context;

		public ModelRepository(ModelDbContext context)
		{
			_context = context;
		}

		public async Task CreateModel(Model model)
		{
			await _context.Models.AddAsync(model);
			await _context.SaveChangesAsync();
		}

		public async Task<Model> GetModelById(Guid modelId)
		{
			return await _context.Models.FindAsync(modelId);
		}

		public async Task<IEnumerable<Model>> GetModelsByProductId(Guid productId)
		{
			return await _context.Models.Where(o => o.ModelId == productId).ToListAsync();
		}

		public async Task UpdateModel(Model model)
		{
			_context.Models.Update(model);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteModel(Guid modelId)
		{
			var model = await _context.Models.FindAsync(modelId);
			if (model != null)
			{
				_context.Models.Remove(model);
				await _context.SaveChangesAsync();
			}
		}
	}
}
