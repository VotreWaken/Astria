﻿using Newtonsoft.Json;

namespace ImagesManagment.API.Middleware
{
	public class ErrorDetailsModel
	{
		public int StatusCode { get; set; }
		public string Message { get; set; }

		public override string ToString()
		{
			return JsonConvert.SerializeObject(this);
		}
	}
}
