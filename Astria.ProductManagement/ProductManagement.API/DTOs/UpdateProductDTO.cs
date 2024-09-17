namespace ProductManagement.API.DTOs
{
	public class UpdateProductDTO
	{
		public Guid ProductId { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public decimal? Price { get; set; }
	}
}
