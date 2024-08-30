using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astria.Rabbitmq.Events
{
	public class ModelCreatedEvent
	{
		public Guid ModelId { get; set; }
		public Guid ProductId { get; set; }
		public byte[] ModelData { get; set; }
		public string ModelType { get; set; }
		public byte[] BinFileData { get; set; }
		public Guid TextureId { get; set; }
	}
}
