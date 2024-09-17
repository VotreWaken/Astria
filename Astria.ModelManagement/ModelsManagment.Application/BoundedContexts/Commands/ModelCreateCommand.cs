using MediatR;
using ModelsManagment.Application.Results;

namespace ModelsManagment.Application.BoundedContexts.Commands
{
	public class ModelCreateCommand : IRequest<CommandResult>
	{
		public Guid ModelId { get; set; }
		public Guid ProductId { get; set; }
		public string ModelDataUrl { get; set; }
		public string ModelType { get; set; }
		public string BinFileDataUrl { get; set; }
		public Guid TextureId { get; set; }
	}
}
