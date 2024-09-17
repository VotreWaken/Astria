using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astria.SharedKernel;
using UserProductLikesManagement.Domain.BoundedContexts.UserProductLikesManagement.Events;
namespace UserProductLikesManagement.Domain.BoundedContexts.UserProductLikesManagement.Aggregates
{
	public class UserProductLike : AggregateRoot
	{
		public Guid UserId { get; set; }
		public Guid ProductId { get; set; }

		// Конструктор по умолчанию
		public UserProductLike() { }

		// Конструктор для создания нового заказа
		public UserProductLike(Guid orderId, Guid orderNumber, Guid orderDate)
		{
			if (orderId == Guid.Empty)
				throw new ArgumentNullException(nameof(orderId));

			AggregateId = orderId;
			UserId = orderNumber;
			ProductId = orderDate;

			RaiseEvent(new UserLikeProductCreatedEvent(AggregateId, orderNumber, orderDate));
		}

		// Конструктор для создания нового заказа
		public UserProductLike(Guid orderNumber, Guid orderDate)
		{
			AggregateId = new Guid();
			UserId = orderNumber;
			ProductId = orderDate;

			RaiseEvent(new UserLikeProductCreatedEvent(AggregateId, orderNumber, orderDate));
		}

		#region Aggregate Methods

		// Методы бизнес-логики, например, создание заказа
		public void CreateOrder(Guid orderNumber, Guid orderDate)
		{
			UserId = orderNumber;
			ProductId = orderDate;

			RaiseEvent(new UserLikeProductCreatedEvent(AggregateId, orderNumber, orderDate));
		}

		// Добавьте другие методы бизнес-логики, если необходимо

		#endregion

		#region Event Handling

		// Обработка событий для восстановления состояния агрегата
		protected override void When(IDomainEvent @event)
		{
			switch (@event)
			{
				case UserLikeProductCreatedEvent e: OnOrderCreatedEvent(e); break;
					// Добавьте обработку других событий, если необходимо
			}
		}

		private void OnOrderCreatedEvent(UserLikeProductCreatedEvent @event)
		{
			AggregateId = @event.AggregateId;
			UserId = @event.UserId;
			ProductId = @event.ProductId;
		}

		#endregion
	}
}
