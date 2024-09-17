using ImagesManagment.Application.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImagesManagment.Application.BoundedContexts.UserImageManagment.Commands
{
	public class ImageUpdateCommand : IRequest<CommandResult>
	{
		public Guid ImageId { get; set; }
		public string UserImageUrl { get; set; }
	}
}
