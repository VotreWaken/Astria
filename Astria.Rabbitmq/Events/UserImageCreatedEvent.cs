using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astria.Rabbitmq.Events
{
	public class UserImageCreatedEvent
	{
		public Guid UserImageId { get; set; }
		public byte[] ImageData { get; set; }
	}
}
