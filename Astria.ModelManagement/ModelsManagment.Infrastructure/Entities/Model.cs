using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsManagment.Infrastructure.Entities
{
	public class Model
	{
		[Key]
		[Column("ModelId")]
		public Guid ModelId { get; set; }

		[Column("ProductId")]
		public Guid ProductId { get; set; }

		public string ModelDataUrl { get; set; }
		public string ModelType { get; set; }
		public string BinFileDataUrl { get; set; }

		[Column("TextureId")]
		public Guid TextureId { get; set; }
	}
}
