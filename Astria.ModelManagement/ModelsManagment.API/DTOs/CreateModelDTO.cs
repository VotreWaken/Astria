namespace ModelsManagment.API.DTOs
{
	public class CreateModelDTO
	{
		public IFormFile objFile { get; set; }
		public Guid modelTexture { get; set; }
		public Guid ProductId { get; set; }
		public Guid modelId { get; set; }
		public string modelType { get; set; }
		public IFormFile? binFile { get; set; }
	}
}
