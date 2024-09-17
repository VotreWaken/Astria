using MediatR;
using OrderManagement.Application.Results;
using OrderManagement.Domain.BoundedContexts.OrderManagment.ValueObjects;


namespace OrderManagement.Application.BoundedContexts.Commands
{
	public class OrderCreateCommand : IRequest<CommandResult>
	{
		public Guid CustomerId { get; set; }
		public Guid ProductId { get; set; }
		public DateTime OrderDate { get; set; }
		public decimal OrderAmount { get; set; }
		public OrderStatus Status { get; set; }
	}
}
