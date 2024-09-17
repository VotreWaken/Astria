using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Application.BoundedContexts.Commands
{
	public class ProductReportCommand
	{
		public Guid Id { get; set; }

		public ProductReportCommand(Guid id)
		{
			Id = id;
		}
	}
}
