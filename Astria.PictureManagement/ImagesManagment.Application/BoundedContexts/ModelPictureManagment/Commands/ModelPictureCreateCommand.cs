using ImagesManagment.Application.Results;
using MediatR;

namespace ImagesManagment.Application.BoundedContexts.ModelPictureManagment.Commands
{
	public class ModelPictureCreateOrUpdateCommand : IRequest<CommandResult>
	{
		public Guid ModelId { get; set; }
		public string TextureType { get; set; }
		public string ImageUrl { get; set; }
	}
}
