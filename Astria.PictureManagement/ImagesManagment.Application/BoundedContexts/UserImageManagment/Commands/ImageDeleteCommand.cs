using ImagesManagment.Application.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImagesManagment.Application.BoundedContexts.UserImageManagment.Commands
{
    public class ImageDeleteCommand : IRequest<CommandResult>
    {
        public Guid Id { get; set; }

        public ImageDeleteCommand(Guid id)
        {
            Id = id;
        }
    }
}
