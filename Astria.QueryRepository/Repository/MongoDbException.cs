using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astria.QueryRepository.Repository
{
	class MongoDbException : Exception
	{
		public MongoDbException() { }
		public MongoDbException(string message) : base(message) { }
		public MongoDbException(string message, Exception inner) : base(message, inner) { }
	}
}
