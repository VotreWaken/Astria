using ImagesManagment.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImagesManagment.Infrastructure.DataContext
{
	public class PictureDbContext : DbContext
	{
		public PictureDbContext(DbContextOptions<PictureDbContext> options) : base(options)
		{ }

		public DbSet<Picture> UserImages { get; set; }
		public DbSet<ModelPicture> ModelTextures { get; set; }
		public DbSet<PreviewPicture> ProductPreviewImages { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Picture>().HasKey(o => o.ImageId);
			modelBuilder.Entity<ModelPicture>().HasKey(o => o.TextureId);
			modelBuilder.Entity<PreviewPicture>().HasKey(o => o.PreviewImageId);
		}
	}
}
