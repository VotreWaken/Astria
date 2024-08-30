using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astria.Rabbitmq.Events
{
	public class ProductPreviewImageProcessingEvent
	{
		public Guid ProductPreviewImageId { get; set; }
		public Guid ProductId { get; set; }
		public string ImagePath { get; set; }
		public byte[] ImageData { get; set; }
	}
}
