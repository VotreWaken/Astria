using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCommentsManagement.Application.Results
{
	public enum FailureTypes
	{
		None,
		BusinessRule,
		NotFound,
		Duplicate
	}
}
