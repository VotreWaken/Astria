using Astria.SharedKernel;
using ImagesManagment.Domain.BoundedContexts.ProductPreviewImageManagment.Events;

namespace ImagesManagment.Domain.BoundedContexts.ProductPreviewImageManagment.Aggregates
{
	public class PreviewProductPicture : AggregateRoot
	{
		public Guid ProductId { get; set; }
		public string ImageUrl { get; set; }

		// Конструктор по умолчанию
		public PreviewProductPicture() { }

		// Конструктор для создания нового заказа
		public PreviewProductPicture(Guid orderId, Guid modelId, string url)
		{
			if (orderId == Guid.Empty)
				throw new ArgumentNullException(nameof(orderId));

			AggregateId = orderId;
			ProductId = modelId;
			ImageUrl = url;

			RaiseEvent(new PreviewProductPictureCreatedEvent(orderId, modelId, url));
		}

		#region Aggregate Methods

		// Методы бизнес-логики, например, создание заказа
		public void CreateOrder(Guid modelId, string url)
		{
			ProductId = modelId;
			ImageUrl = url;
			RaiseEvent(new PreviewProductPictureCreatedEvent(AggregateId, modelId, url));
		}

		public void DeleteOrder(Guid id)
		{
			RaiseEvent(new PreviewProductPictureDeletedEvent(AggregateId, id));
		}

		// Добавьте другие методы бизнес-логики, если необходимо

		#endregion

		#region Event Handling

		// Обработка событий для восстановления состояния агрегата
		protected override void When(IDomainEvent @event)
		{
			switch (@event)
			{
				case PreviewProductPictureCreatedEvent e: OnImageCreatedEvent(e); break;
				case PreviewProductPictureDeletedEvent e: OnImageDeletedEvent(e); break;
					// Добавьте обработку других событий, если необходимо
			}
		}

		private void OnImageCreatedEvent(PreviewProductPictureCreatedEvent @event)
		{
			AggregateId = @event.AggregateId;
			ProductId = @event.ProductId;
			ImageUrl = @event.ImageUrl;
		}

		private void OnImageDeletedEvent(PreviewProductPictureDeletedEvent @event)
		{
			AggregateId = @event.AggregateId;
		}

		#endregion
	}
}
