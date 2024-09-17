using Astria.QueryRepository.Repository;
using OrderManagement.Domain.BoundedContexts.OrderManagment.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.BoundedContexts.QueryObjects
{
	public class OrderInfo : IQueryEntity
	{
		public Guid Id { get; set; }
		public Guid CustomerId { get; set; }
		public Guid ProductId { get; set; }
		public DateTime OrderDate { get; set; }
		public decimal OrderAmount { get; set; }
		public OrderStatus Status { get; set; }
	}
}
