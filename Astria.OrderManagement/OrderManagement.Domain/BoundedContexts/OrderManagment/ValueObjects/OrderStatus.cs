using Astria.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OrderManagement.Domain.BoundedContexts.OrderManagment.ValueObjects
{
	public class OrderStatus : ValueObject
	{
		public static readonly OrderStatus Pending = new OrderStatus(nameof(Pending));
		public static readonly OrderStatus Confirmed = new OrderStatus(nameof(Confirmed));
		public static readonly OrderStatus Cancelled = new OrderStatus(nameof(Cancelled));

		public string Status { get; private set; }

		[JsonConstructor]
		private OrderStatus(string status)
		{
			Status = status;
		}

		public static OrderStatus FromString(string status)
		{
			return status switch
			{
				nameof(Pending) => Pending,
				nameof(Confirmed) => Confirmed,
				nameof(Cancelled) => Cancelled,
				_ => throw new ArgumentException("Invalid order status", nameof(status)),
			};
		}

		protected override IEnumerable<object> GetEqualityComponents()
		{
			yield return Status;
		}

		public override string ToString() => Status;
	}
}
