using MediatR;
using ModelsManagment.Application.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsManagment.Application.BoundedContexts.Commands
{
	public class ModelDeleteCommand : IRequest<CommandResult>
	{
		public Guid Id { get; set; }

		public ModelDeleteCommand(Guid id)
		{
			Id = id;
		}
	}
}
