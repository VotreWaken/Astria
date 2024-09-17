using ImagesManagment.Application.Results;
using MediatR;

namespace ImagesManagment.Application.BoundedContexts.PreviewProductPictureManagment.Commands
{
	public class ProductPreviewPictureCreateCommand : IRequest<CommandResult>
	{
		public Guid ProductPreviewImageId { get; set; }
		public Guid ProductId { get; set; }
		public string Url { get; set; }
	}
}
