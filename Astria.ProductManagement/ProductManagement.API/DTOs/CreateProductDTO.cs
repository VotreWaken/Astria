namespace ProductManagement.API.DTOs
{
	public class CreateProductDTO
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public decimal Price { get; set; }
		public bool IsAvailable { get; set; }
		public Guid UserId { get; set; }
		public DateTime Date { get; private set; } = DateTime.UtcNow;
	}
}
