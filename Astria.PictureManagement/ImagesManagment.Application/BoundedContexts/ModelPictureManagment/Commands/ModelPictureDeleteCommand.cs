using ImagesManagment.Application.Results;
using MediatR;

namespace ImagesManagment.Application.BoundedContexts.ModelPictureManagment.Commands
{
	public class ModelPictureDeleteCommand : IRequest<CommandResult>
	{
		public Guid Id { get; set; }

		public ModelPictureDeleteCommand(Guid id)
		{
			Id = id;
		}
	}
}
