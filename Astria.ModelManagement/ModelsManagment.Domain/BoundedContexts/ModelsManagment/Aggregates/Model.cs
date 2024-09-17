using Astria.SharedKernel;
using ModelsManagment.Domain.BoundedContexts.ModelsManagment.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsManagment.Domain.BoundedContexts.ModelsManagment.Aggregates
{
	public class Model : AggregateRoot
	{
		public Guid ModelId { get; private set; }
		public Guid ProductId { get; private set; }
		public string ModelDataUrl { get; private set; }
		public Guid TextureId { get; private set; }
		public string ModelType { get; private set; }

		// Конструктор по умолчанию
		public Model() { }

		// Конструктор для создания новой модели
		public Model(Guid orderId, Guid productId, string modelDataUrl, Guid textureId, string modelType)
		{
			if (orderId == Guid.Empty)
				throw new ArgumentNullException(nameof(orderId));

			ModelId = orderId;
			ProductId = productId;
			ModelDataUrl = modelDataUrl;
			TextureId = textureId;
			ModelType = modelType;
			RaiseEvent(new ModelCreatedEvent(orderId, productId, modelDataUrl, textureId, modelType));

		}

		#region Aggregate Methods

		// Методы бизнес-логики, например, создание заказа
		public void CreateOrder(Guid productId, string modelDataUrl, Guid textureId, string modelType)
		{
			ProductId = productId;
			ModelDataUrl = modelDataUrl;
			TextureId = textureId;
			
			RaiseEvent(new ModelCreatedEvent(AggregateId, productId, modelDataUrl, textureId, modelType));
		}

		public void DeleteOrder(Guid id)
		{
			RaiseEvent(new ModelDeletedEvent(AggregateId, id));
		}

		// Добавьте другие методы бизнес-логики, если необходимо

		#endregion

		#region Event Handling

		// Обработка событий для восстановления состояния агрегата
		protected override void When(IDomainEvent @event)
		{
			switch (@event)
			{
				case ModelCreatedEvent e: OnProductCreatedEvent(e); break;
				case ModelDeletedEvent e: OnProductDeletedEvent(e); break;
					// Добавьте обработку других событий, если необходимо
			}
		}

		private void OnProductCreatedEvent(ModelCreatedEvent @event)
		{
			AggregateId = @event.AggregateId;
			ProductId = @event.ProductId;
			ModelDataUrl = @event.ModelDataUrl;
			TextureId = @event.TextureId;
		}

		private void OnProductDeletedEvent(ModelDeletedEvent @event)
		{
			AggregateId = @event.AggregateId;
		}

		#endregion
	}
}
