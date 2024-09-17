using Microsoft.EntityFrameworkCore;
using ModelsManagment.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsManagment.Infrastructure.DataContext
{
	public class ModelDbContext : DbContext
	{
		public ModelDbContext(DbContextOptions<ModelDbContext> options) : base(options)
		{ }

		public DbSet<Model> Models { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Model>().HasKey(o => o.ModelId);
		}
	}
}
