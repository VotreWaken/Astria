using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astria.Rabbitmq.Events
{
	public class TextureProcessingEvent
	{
		public Guid TextureId { get; set; }
		public Guid ModelId { get; set; }
		public Guid ProductId { get; set; }
		public byte[] ModelData { get; set; }
		public byte[] BaseColorData { get; set; }
		public byte[] NormalMapData { get; set; }
		public byte[] DisplacementData { get; set; }
		public byte[] MetallicData { get; set; }
		public byte[] RoughnessData { get; set; }
		public byte[] EmissiveData { get; set; }
	}
}
