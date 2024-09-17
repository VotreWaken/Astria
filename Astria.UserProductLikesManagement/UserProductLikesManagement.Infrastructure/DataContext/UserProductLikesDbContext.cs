using Microsoft.EntityFrameworkCore;
using UserProductLikesManagement.Infrastructure.Entities;

namespace UserProductLikesManagement.Infrastructure.DataContext
{
	public class UserProductLikesDbContext : DbContext
	{
		public UserProductLikesDbContext(DbContextOptions<UserProductLikesDbContext> options) : base(options)
		{ }

		public DbSet<UserProductLike> UserProductLikes { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<UserProductLike>().HasKey(o => o.UserId);
		}
	}
}
