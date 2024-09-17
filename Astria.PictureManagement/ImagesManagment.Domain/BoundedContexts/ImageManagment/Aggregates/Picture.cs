using Astria.SharedKernel;
using ImagesManagment.Domain.BoundedContexts.ImageManagment.Events;

namespace ImagesManagment.Domain.BoundedContexts.ImageManagment.Aggregates
{
	public class Picture : AggregateRoot
	{
		public string Url { get; set; }

		// Конструктор по умолчанию
		public Picture() { }

		// Конструктор для создания нового заказа
		public Picture(Guid orderId, string url)
		{
			if (orderId == Guid.Empty)
				throw new ArgumentNullException(nameof(orderId));

			AggregateId = orderId;
			Url = url;

			RaiseEvent(new ImageCreatedEvent(orderId, url));
		}

		#region Aggregate Methods

		// Методы бизнес-логики, например, создание заказа
		public void CreateOrder(string url)
		{
			Url = url;

			RaiseEvent(new ImageCreatedEvent(AggregateId, url));
		}

		public void DeleteOrder(Guid id)
		{
			RaiseEvent(new ImageDeletedEvent(AggregateId, id));
		}

		// Добавьте другие методы бизнес-логики, если необходимо

		#endregion

		#region Event Handling

		// Обработка событий для восстановления состояния агрегата
		protected override void When(IDomainEvent @event)
		{
			switch (@event)
			{
				case ImageCreatedEvent e: OnImageCreatedEvent(e); break;
				case ImageDeletedEvent e: OnImageDeletedEvent(e); break;
					// Добавьте обработку других событий, если необходимо
			}
		}

		private void OnImageCreatedEvent(ImageCreatedEvent @event)
		{
			AggregateId = @event.AggregateId;
			Url = @event.Url;
		}

		private void OnImageDeletedEvent(ImageDeletedEvent @event)
		{
			AggregateId = @event.AggregateId;
		}

		#endregion
	}
}
