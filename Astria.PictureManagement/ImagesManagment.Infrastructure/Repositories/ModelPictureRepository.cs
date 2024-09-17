using ImagesManagment.Infrastructure.DataContext;
using ImagesManagment.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImagesManagment.Infrastructure.Repositories
{
	public class ModelPictureRepository : IModelTextureRepository<ModelPicture>
	{
		// Context
		private readonly PictureDbContext _context;

		// Constructor
		public ModelPictureRepository(PictureDbContext context)
		{
			_context = context;
		}

		// Get All Images
		public async Task<List<ModelPicture>> GetAll()
		{
			return await _context.ModelTextures.ToListAsync();
		}

		// Get Image By Id
		public async Task<ModelPicture> GetById(Guid id)
		{
			var image = await _context.ModelTextures.FindAsync(id);
			if (image == null)
			{
				throw new Exception($"Image with ID {id} not found");
			}

			return image;
		}

		public async Task<ModelPicture> GetByModelId(Guid id)
		{
			var image = await _context.ModelTextures.FirstOrDefaultAsync(c => c.ModelId == id);
			if (image == null)
			{
				return null;
			}

			return image;
		}

		// Create Image
		public async Task<ModelPicture> Create(ModelPicture image)
		{
			var existingImage = await _context.ModelTextures.FirstOrDefaultAsync(c => c.TextureId == image.TextureId);
			if (existingImage != null)
			{
				throw new InvalidOperationException("Image with the same name already exists");
			}

			_context.ModelTextures.Add(image);
			await _context.SaveChangesAsync();
			return image;
		}

		// Update Image
		public async Task Update(ModelPicture image)
		{
			var existingImage = await _context.ModelTextures.FindAsync(image.TextureId);
			if (existingImage == null)
			{
				throw new ArgumentException("Image not found");
			}

			_context.Entry(existingImage).CurrentValues.SetValues(image);

			await _context.SaveChangesAsync();
		}

		// Delete Image
		public async Task Delete(Guid id)
		{
			var image = await _context.ModelTextures.FindAsync(id);
			if (image != null)
			{
				_context.ModelTextures.Remove(image);
				await _context.SaveChangesAsync();
			}
		}
	}
}
