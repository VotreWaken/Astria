using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Infrastructure.Entities
{
	public class Product
	{
		public Guid ProductId { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public Guid ModelId { get; set; }
		public decimal Price { get; set; }
		public bool IsAvailable { get; set; }
		public DateTime Date { get; set; }
		public Guid PreviewImageId { get; set; }
		public Guid UserId { get; set; }
		public int Views { get ; set; }
	}
}
