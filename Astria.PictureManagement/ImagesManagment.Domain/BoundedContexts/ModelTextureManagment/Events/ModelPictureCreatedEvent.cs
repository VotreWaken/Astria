using Astria.SharedKernel;

namespace ImagesManagment.Domain.BoundedContexts.ModelTextureManagment.Events
{
	public class ModelPictureCreatedEvent : DomainEvent
	{
		public Guid TextureId { get; set; }
		public Guid ModelId { get; set; }
		public string BaseColorUrl { get; set; }
		public string NormalMapUrl { get; set; }
		public string DisplacementUrl { get; set; }
		public string MetallicUrl { get; set; }
		public string RoughnessUrl { get; set; }
		public string EmissiveUrl { get; set; }

		public ModelPictureCreatedEvent(Guid aggregateId, Guid modelId,
			string baseColorUrl, string normalMapUrl, string displacementUrl, 
			string metallicUrl, string roughnessUrl, string emissiveUrl)
			: base(aggregateId)
		{
			TextureId = aggregateId;
			ModelId = modelId;
			BaseColorUrl = baseColorUrl;
			NormalMapUrl = normalMapUrl;
			DisplacementUrl = displacementUrl;
			MetallicUrl = metallicUrl;
			RoughnessUrl = roughnessUrl;
			EmissiveUrl = emissiveUrl;
		}
	}
}
