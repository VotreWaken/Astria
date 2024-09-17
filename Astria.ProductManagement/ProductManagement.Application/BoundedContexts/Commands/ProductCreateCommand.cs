using MediatR;
using ProductManagement.Application.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Application.BoundedContexts.Commands
{
	public class ProductCreateCommand : IRequest<CommandResult>
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public decimal Price { get; set; }
		public Guid ModelId { get; set; }
		public bool IsAvailable { get; set; }
		public DateTime Date { get; set; }
		public Guid PreviewImageId { get; set; }
		public Guid UserId { get ; set; }
	}
}
