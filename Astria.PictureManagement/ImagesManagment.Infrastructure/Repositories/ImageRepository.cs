using ImagesManagment.Infrastructure.DataContext;
using ImagesManagment.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace ImagesManagment.Infrastructure.Repositories
{
	public class PicturesRepository : IPictureRepository<Picture>
	{
		// Context
		private readonly PictureDbContext _context;

		// Constructor
		public PicturesRepository(PictureDbContext context)
		{
			_context = context;
		}

		// Get All Images
		public async Task<List<Picture>> GetAll()
		{
			return await _context.UserImages.ToListAsync();
		}

		// Get Image By Id
		public async Task<Picture> GetById(Guid id)
		{
			var image = await _context.UserImages.FindAsync(id);
			if (image == null)
			{
				return null;
				// throw new Exception($"Image with ID {id} not found");
			}

			return image;
		}

		// Create Image
		public async Task<Picture> Create(Picture image)
		{
			var existingImage = await _context.UserImages.FirstOrDefaultAsync(c => c.ImageUrl == image.ImageUrl);
			if (existingImage != null)
			{
				throw new InvalidOperationException("Image with the same name already exists");
			}

			_context.UserImages.Add(image);
			await _context.SaveChangesAsync();
			return image;
		}

		// Update Image
		public async Task Update(Picture image)
		{
			var existingImage = await _context.UserImages.FindAsync(image.ImageId);
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
			var image = await _context.UserImages.FindAsync(id);
			if (image != null)
			{
				_context.UserImages.Remove(image);
				await _context.SaveChangesAsync();
			}
		}
	}
}
