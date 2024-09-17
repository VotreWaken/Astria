using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserFollowsManagement.Infrastructure.Entities;

namespace UserFollowsManagement.Infrastructure.DataContext
{
	public class UserFollowsDbContext : DbContext
	{
		public UserFollowsDbContext(DbContextOptions<UserFollowsDbContext> options) : base(options)
		{ }

		public DbSet<UserFollow> UserFollows { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<UserFollow>().HasKey(o => o.FollowerId);
		}
	}
}
