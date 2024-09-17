using Astria.SharedKernel;
using ImagesManagment.Domain.BoundedContexts.ModelTextureManagment.Events;

namespace ImagesManagment.Domain.BoundedContexts.ModelTextureManagment.Aggregates
{
	public class ModelPicture : AggregateRoot
	{
		public Guid ModelId { get; set; }
		public string BaseColorUrl { get; set; }
		public string NormalMapUrl { get; set; }
		public string DisplacementUrl { get; set; }
		public string MetallicUrl { get; set; }
		public string RoughnessUrl { get; set; }
		public string EmissiveUrl { get; set; }

		// Конструктор по умолчанию
		public ModelPicture() { }

		// Конструктор для создания нового заказа
		public ModelPicture(Guid orderId,Guid modelId, string baseColorUrl, 
			string normalMapUrl, string displacementUrl, string metallicUrl, 
			string roughnessUrl, string emissiveUrl)
		{
			if (orderId == Guid.Empty)
				throw new ArgumentNullException(nameof(orderId));

			AggregateId = orderId;
			ModelId = modelId;
			BaseColorUrl = baseColorUrl;
			NormalMapUrl = normalMapUrl;
			DisplacementUrl = displacementUrl;
			MetallicUrl = metallicUrl;
			RoughnessUrl = roughnessUrl;
			EmissiveUrl = emissiveUrl;

			RaiseEvent(new ModelPictureCreatedEvent(orderId, modelId, BaseColorUrl,
				NormalMapUrl, DisplacementUrl, MetallicUrl, RoughnessUrl, EmissiveUrl));
		}

		#region Aggregate Methods

		// Методы бизнес-логики, например, создание заказа
		public void CreateOrder(Guid modelId, string baseColorUrl,
			string normalMapUrl, string displacementUrl, string metallicUrl,
			string roughnessUrl, string emissiveUrl)
		{
			ModelId = modelId;
			BaseColorUrl = baseColorUrl;
			NormalMapUrl = normalMapUrl;
			DisplacementUrl = displacementUrl;
			MetallicUrl = metallicUrl;
			RoughnessUrl = roughnessUrl;
			EmissiveUrl = emissiveUrl;
			RaiseEvent(new ModelPictureCreatedEvent(AggregateId, modelId, BaseColorUrl,
				NormalMapUrl, DisplacementUrl, MetallicUrl, RoughnessUrl, EmissiveUrl));
		}

		public void DeleteOrder(Guid id)
		{
			RaiseEvent(new ModelPictureDeletedEvent(AggregateId, id));
		}

		// Добавьте другие методы бизнес-логики, если необходимо

		#endregion

		#region Event Handling

		// Обработка событий для восстановления состояния агрегата
		protected override void When(IDomainEvent @event)
		{
			switch (@event)
			{
				case ModelPictureCreatedEvent e: OnImageCreatedEvent(e); break;
				case ModelPictureDeletedEvent e: OnImageDeletedEvent(e); break;
					// Добавьте обработку других событий, если необходимо
			}
		}

		private void OnImageCreatedEvent(ModelPictureCreatedEvent @event)
		{
			AggregateId = @event.AggregateId;
			ModelId = @event.ModelId;
			BaseColorUrl = @event.BaseColorUrl;
			NormalMapUrl = @event.NormalMapUrl;
			DisplacementUrl = @event.DisplacementUrl;
			MetallicUrl = @event.MetallicUrl;
			RoughnessUrl = @event.RoughnessUrl;
			EmissiveUrl = @event.EmissiveUrl;
		}

		private void OnImageDeletedEvent(ModelPictureDeletedEvent @event)
		{
			AggregateId = @event.AggregateId;
		}

		#endregion
	}
}
