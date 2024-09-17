using Astria.SharedKernel;
using ProductCommentsManagement.Domain.BoundedContexts.ProductCommentsManagement.Events;

namespace ProductCommentsManagement.Domain.BoundedContexts.ProductCommentsManagement.Aggregates
{
	public class ProductComments : AggregateRoot
	{
		public Guid Id { get; private set; }
		public Guid ProductId { get; private set; }
		public Guid UserId { get; private set; }
		public string CommentText { get; private set; }
		public Guid ParentCommentId { get; private set; }
		public DateTime CreatedAt { get; private set; }
		public DateTime? UpdatedAt { get; private set; }
		public bool IsDeleted { get; private set; }

		// Конструктор по умолчанию
		public ProductComments() { }

		// Конструктор для создания нового заказа
		public ProductComments(Guid id, Guid productId, Guid userId, string commentText, Guid parentCommentId)
		{
			if (id == Guid.Empty)
				throw new ArgumentNullException(nameof(id));
			if (productId == Guid.Empty)
				throw new ArgumentNullException(nameof(productId));
			if (userId == Guid.Empty)
				throw new ArgumentNullException(nameof(userId));
			if (string.IsNullOrWhiteSpace(commentText))
				throw new ArgumentNullException(nameof(commentText));

			Id = id;
			ProductId = productId;
			UserId = userId;
			CommentText = commentText;
			ParentCommentId = parentCommentId;
			CreatedAt = DateTime.UtcNow;
			IsDeleted = false;

			RaiseEvent(new ProductCommentsCreatedEvent(Id, ProductId, UserId, CommentText, ParentCommentId, CreatedAt));
		}

		#region Aggregate Methods

		// Методы бизнес-логики, например, создание заказа
		public void EditComment(Guid id, string newCommentText)
		{
			if (string.IsNullOrWhiteSpace(newCommentText))
				throw new ArgumentNullException(nameof(newCommentText));

			Id = id;
			CommentText = newCommentText;
			UpdatedAt = DateTime.UtcNow;

			RaiseEvent(new ProductCommentsEditedEvent(Id, CommentText, UpdatedAt.Value));
		}

		public void DeleteComment(Guid id)
		{
			if (IsDeleted)
				throw new InvalidOperationException("Comment is already deleted.");

			IsDeleted = true;
			UpdatedAt = DateTime.UtcNow;

			RaiseEvent(new ProductCommentsDeletedEvent(id, UpdatedAt.Value));

			Console.WriteLine("Raise Deleted Event");
		}

		// Добавьте другие методы бизнес-логики, если необходимо

		#endregion

		#region Event Handling

		// Обработка событий для восстановления состояния агрегата
		protected override void When(IDomainEvent @event)
		{
			switch (@event)
			{
				case ProductCommentsCreatedEvent e:
					Apply(e);
					break;
				case ProductCommentsEditedEvent e:
					Apply(e);
					break;
				case ProductCommentsDeletedEvent e:
					Apply(e);
					Console.WriteLine("When Deleted Event Switch");
					break;
			}
		}

		private void Apply(ProductCommentsCreatedEvent @event)
		{
			Id = @event.Id;
			ProductId = @event.ProductId;
			UserId = @event.UserId;
			CommentText = @event.CommentText;
			ParentCommentId = @event.ParentCommentId;
			CreatedAt = @event.CreatedAt;
			IsDeleted = false;
		}

		private void Apply(ProductCommentsEditedEvent @event)
		{
			Id = @event.Id;
			CommentText = @event.NewCommentText;
			UpdatedAt = @event.UpdatedAt;
		}

		private void Apply(ProductCommentsDeletedEvent @event)
		{
			IsDeleted = true;
			UpdatedAt = @event.DeletedAt;
			Console.WriteLine("ProductCommentDeletedEvent");
		}

		#endregion
	}
}
