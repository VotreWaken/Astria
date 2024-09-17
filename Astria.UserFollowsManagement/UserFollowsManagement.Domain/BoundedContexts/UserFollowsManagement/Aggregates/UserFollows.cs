using Astria.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserFollowsManagement.Domain.BoundedContexts.UserFollowsManagement.Events;

namespace UserFollowsManagement.Domain.BoundedContexts.UserFollowsManagement.Aggregates
{
	public class UserFollows : AggregateRoot
	{
		public Guid FollowerId { get; set; }
		public Guid FollowedId { get; set; }

		// Конструктор по умолчанию
		public UserFollows() { }

		// Конструктор для создания нового заказа
		public UserFollows(Guid orderId, Guid followerId, Guid followedId)
		{
			if (orderId == Guid.Empty)
				throw new ArgumentNullException(nameof(orderId));

			AggregateId = orderId;
			FollowerId = followerId;
			FollowedId = followedId;

			RaiseEvent(new UserFollowsCreatedEvent(AggregateId, followerId, followedId));
		}

		// Конструктор для создания нового заказа
		public UserFollows(Guid orderNumber, Guid orderDate)
		{
			AggregateId = new Guid();
			FollowerId = orderNumber;
			FollowedId = orderDate;

			RaiseEvent(new UserFollowsCreatedEvent(AggregateId, orderNumber, orderDate));
		}

		#region Aggregate Methods

		// Методы бизнес-логики, например, создание заказа
		public void CreateOrder(Guid orderNumber, Guid orderDate)
		{
			FollowerId = orderNumber;
			FollowedId = orderDate;

			RaiseEvent(new UserFollowsCreatedEvent(AggregateId, orderNumber, orderDate));
		}

		// Добавьте другие методы бизнес-логики, если необходимо

		#endregion

		#region Event Handling

		// Обработка событий для восстановления состояния агрегата
		protected override void When(IDomainEvent @event)
		{
			switch (@event)
			{
				case UserFollowsCreatedEvent e: OnOrderCreatedEvent(e); break;
				case UserUnFollowsCreatedEvent e: OnProductDeletedEvent(e); break;
			}
		}

		private void OnOrderCreatedEvent(UserFollowsCreatedEvent @event)
		{
			AggregateId = @event.AggregateId;
			FollowerId = @event.FollowerId;
			FollowedId = @event.FollowedId;
		}

		public void DeleteOrder(Guid followerId, Guid followedId)
		{
			RaiseEvent(new UserUnFollowsCreatedEvent(AggregateId, followerId, followedId));
		}

		private void OnProductDeletedEvent(UserUnFollowsCreatedEvent @event)
		{
			AggregateId = @event.AggregateId;
		}

		#endregion
	}
}
