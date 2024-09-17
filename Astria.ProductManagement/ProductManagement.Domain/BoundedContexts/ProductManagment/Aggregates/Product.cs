using Astria.SharedKernel;
using ProductManagement.Domain.BoundedContexts.ProductManagement.Events;
using ProductManagement.Domain.BoundedContexts.ProductManagment.Events;

namespace ProductManagement.Domain.BoundedContexts.ProductManagement.Aggregates
{
	public class Product : AggregateRoot
	{
		public string Name { get; private set; }
		public string Description { get; private set; }
		public decimal Price { get; private set; }
		public bool IsAvailable { get; private set; }
		public DateTime Date { get; private set; }
		public Guid ModelId { get; private set; }
		public Guid UserId { get; private set; }
		public Guid PreviewImageId { get ; private set; }

		public int Views {  get; private set; }

		// Конструктор по умолчанию
		public Product() { }

		// Конструктор для создания нового заказа
		public Product(Guid orderId, string productName, string productDescription, decimal productPrice, bool productIsAvailable, DateTime orderDate, Guid modelId, Guid previewImageId, Guid userId)
		{
			if (orderId == Guid.Empty)
				throw new ArgumentNullException(nameof(orderId));

			AggregateId = orderId;
			Name = productName;
			Description = productDescription;
			Price = productPrice;
			IsAvailable = productIsAvailable;
			Date = orderDate;
			ModelId = modelId;
			PreviewImageId = previewImageId;
			UserId = userId;
			RaiseEvent(new ProductCreatedEvent(orderId, productName, productDescription, productPrice, productIsAvailable, orderDate, modelId, previewImageId, userId));
		}

		#region Aggregate Methods

		// Методы бизнес-логики, например, создание заказа
		public void CreateOrder(string productName, string productDescription, decimal productPrice, bool productIsAvailable, DateTime orderDate, Guid modelId, Guid previewImageId, Guid userId)
		{
			Name = productName;
			Description = productDescription;
			Price = productPrice;
			IsAvailable = productIsAvailable;
			Date = orderDate;
			ModelId = modelId;
			PreviewImageId = previewImageId;
			UserId = userId;
			RaiseEvent(new ProductCreatedEvent(AggregateId, productName, productDescription, productPrice, productIsAvailable, orderDate, modelId, previewImageId, userId));
		}

		public void DeleteOrder(Guid id)
		{
			RaiseEvent(new ProductDeletedEvent(AggregateId, id));
		}

		public void UpdateOrder(Guid productId, string productName, string productDescription, decimal productPrice, bool productIsAvailable, DateTime orderDate, Guid modelId, Guid previewImageId, Guid userId, int views)
		{
			AggregateId = productId;
			Name = productName;
			Description = productDescription;
			Price = productPrice;
			IsAvailable = productIsAvailable;
			Date = orderDate;
			ModelId = modelId;
			PreviewImageId = previewImageId;
			UserId = userId;
			Views = views;
			RaiseEvent(new ProductUpdatedEvent(AggregateId, productName, productDescription, productPrice, productIsAvailable, orderDate, modelId, previewImageId, userId, views));
		}

		// Добавьте другие методы бизнес-логики, если необходимо

		#endregion

		#region Event Handling

		// Обработка событий для восстановления состояния агрегата
		protected override void When(IDomainEvent @event)
		{
			switch (@event)
			{
				case ProductCreatedEvent e: OnProductCreatedEvent(e); break;
				case ProductDeletedEvent e: OnProductDeletedEvent(e); break;
				case ProductUpdatedEvent e: OnProductUpdatedEvent(e); break;
					// Добавьте обработку других событий, если необходимо
			}
		}

		private void OnProductCreatedEvent(ProductCreatedEvent @event)
		{
			AggregateId = @event.AggregateId;
			Name = @event.Name;
			Description = @event.Description;
			Price = @event.Price;
			IsAvailable = @event.IsAvailable;
			Date = @event.Date;
			ModelId = @event.ModelId;
			PreviewImageId = @event.PreviewImageId;
		}

		private void OnProductUpdatedEvent(ProductUpdatedEvent @event)
		{
			AggregateId = @event.AggregateId;
			Name = @event.Name;
			Description = @event.Description;
			Price = @event.Price;
			IsAvailable = @event.IsAvailable;
			Date = @event.Date;
			ModelId = @event.ModelId;
			PreviewImageId = @event.PreviewImageId;
		}

		private void OnProductDeletedEvent(ProductDeletedEvent @event)
		{
			AggregateId = @event.AggregateId;
		}

		#endregion
	}
}
