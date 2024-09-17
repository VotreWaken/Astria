using Microsoft.EntityFrameworkCore;
using ProductCommentsManagement.Infrastructure.Entities;

namespace ProductCommentsManagement.Infrastructure.DataContext
{
	public class ProductCommentsDbContext : DbContext
	{
		public ProductCommentsDbContext(DbContextOptions<ProductCommentsDbContext> options) : base(options)
		{ }

		public DbSet<ProductComments> ProductComments { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<ProductComments>()
				.HasIndex(p => p.ProductId)
				.HasDatabaseName("idx_product");

			modelBuilder.Entity<ProductComments>()
				.HasIndex(p => p.UserId)
				.HasDatabaseName("idx_user");

			modelBuilder.Entity<ProductComments>()
				.HasIndex(p => p.ParentCommentId)
				.HasDatabaseName("idx_parent_comment");
		}
	}
}
