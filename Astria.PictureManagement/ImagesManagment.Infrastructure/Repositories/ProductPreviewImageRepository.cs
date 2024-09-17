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
	public class ProductPreviewImageRepository : IPictureRepository<PreviewPicture>
	{
		// Context
		private readonly PictureDbContext _context;

		// Constructor
		public ProductPreviewImageRepository(PictureDbContext context)
		{
			_context = context;
		}

		// Get All Images
		public async Task<List<PreviewPicture>> GetAll()
		{
			return await _context.ProductPreviewImages.ToListAsync();
		}

		// Get Image By Id
		public async Task<PreviewPicture> GetById(Guid id)
		{
			var image = await _context.ProductPreviewImages.FindAsync(id);
			if (image == null)
			{
				throw new Exception($"Preview Image with ID {id} not found");
			}

			return image;
		}

		// Create Image
		public async Task<PreviewPicture> Create(PreviewPicture image)
		{
			var existingImage = await _context.ProductPreviewImages.FirstOrDefaultAsync(c => c.ImageUrl == image.ImageUrl);
			if (existingImage != null)
			{
				throw new InvalidOperationException("Preview Image with the same name already exists");
			}

			_context.ProductPreviewImages.Add(image);
			await _context.SaveChangesAsync();
			return image;
		}

		// Update Image
		public async Task Update(PreviewPicture image)
		{
			var existingImage = await _context.ProductPreviewImages.FindAsync(image.PreviewImageId);
			if (existingImage == null)
			{
				throw new ArgumentException("Preview Image not found");
			}

			_context.Entry(existingImage).CurrentValues.SetValues(image);

			await _context.SaveChangesAsync();
		}

		// Delete Image
		public async Task Delete(Guid id)
		{
			var image = await _context.ProductPreviewImages.FindAsync(id);
			if (image != null)
			{
				_context.ProductPreviewImages.Remove(image);
				await _context.SaveChangesAsync();
			}
		}
	}
}
