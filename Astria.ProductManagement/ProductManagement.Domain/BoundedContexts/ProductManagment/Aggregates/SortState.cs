using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Domain.BoundedContexts.ProductManagement.Aggregates
{
	public enum SortState
	{
		PriceAsc,
		PriceDesc,
		PublishedDateAsc,
		PublishedDateDesc,
		MostLikedAsc,
		MostLikedDesc,
		MostViewedAsc,
		MostViewedDesc
	}
}
