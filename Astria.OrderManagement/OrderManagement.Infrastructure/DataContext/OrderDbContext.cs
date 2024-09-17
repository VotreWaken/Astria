using Microsoft.EntityFrameworkCore;
using OrderManagement.Domain.BoundedContexts.OrderManagment.ValueObjects;
using OrderManagement.Infrastructure.Entities;
// using OrderManagement.Domain.BoundedContexts.OrderManagment.Aggregates;
namespace OrderManagement.Infrastructure.DataContext
{
	public class OrderDbContext : DbContext
	{
		public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
		{ }

		public DbSet<Order> Orders { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Order>().HasKey(o => o.Id);
			modelBuilder.Entity<Order>()
				.Property(o => o.Status)
				.HasConversion(
					v => v.ToString(),
					v => OrderStatus.FromString(v)
				);
		}
	}
}
