using Microsoft.AspNetCore.SignalR;

namespace Astria.NotificationService.NotificationHub
{
	public interface IConnectionMapping
	{
		void AddConnection(Guid userId, string connectionId);
		void RemoveConnection(Guid userId);
		string GetConnectionId(Guid userId);
	}

	public class ConnectionMapping : IConnectionMapping
	{
		private readonly Dictionary<Guid, string> _connections = new Dictionary<Guid, string>();

		public void AddConnection(Guid userId, string connectionId)
		{
			lock (_connections)
			{
				_connections[userId] = connectionId;
			}
		}

		public void RemoveConnection(Guid userId)
		{
			lock (_connections)
			{
				_connections.Remove(userId);
			}
		}

		public string GetConnectionId(Guid userId)
		{
			lock (_connections)
			{
				return _connections.TryGetValue(userId, out var connectionId) ? connectionId : null;
			}
		}
	}

	public class NotificationHub : Hub
	{
		private readonly IConnectionMapping _connectionMapping;

		public NotificationHub(IConnectionMapping connectionMapping)
		{
			_connectionMapping = connectionMapping;
		}

		public override async Task OnConnectedAsync()
		{
			var userId = Context.GetHttpContext().Request.Query["access_token"].ToString();
			if (!string.IsNullOrEmpty(userId))
			{
				Console.WriteLine($"User connected: {userId}, ConnectionId: {Context.ConnectionId}");
				_connectionMapping.AddConnection(Guid.Parse(userId), Context.ConnectionId);
			}
			else
			{
				Console.WriteLine($"Connection with null or invalid UserIdentifier. User connected: {userId} ConnectionId: {Context.ConnectionId}");
			}
			
			await base.OnConnectedAsync();
		}	

		public override async Task OnDisconnectedAsync(Exception exception)
		{
			var userId = Context.GetHttpContext().Request.Query["access_token"].ToString();
			if (string.IsNullOrEmpty(userId))
			{
				Console.WriteLine($"Disconnection with null or invalid UserIdentifier. ConnectionId: {Context.ConnectionId}");
			}
			else
			{
				_connectionMapping.RemoveConnection(Guid.Parse(userId));
				Console.WriteLine($"User disconnected: {userId}, ConnectionId: {Context.ConnectionId}");
			}

			await base.OnDisconnectedAsync(exception);
		}
	}
}
