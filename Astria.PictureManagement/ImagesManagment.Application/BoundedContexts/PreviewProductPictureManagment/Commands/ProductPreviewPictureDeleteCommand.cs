using ImagesManagment.Application.Results;
using MediatR;

namespace ImagesManagment.Application.BoundedContexts.PreviewProductPictureManagment.Commands
{
	public class ProductPreviewPictureDeleteCommand : IRequest<CommandResult>
	{
		public Guid ProductPreviewImageId { get; set; }

		public ProductPreviewPictureDeleteCommand(Guid id)
		{
			ProductPreviewImageId = id;
		}
	}
}
