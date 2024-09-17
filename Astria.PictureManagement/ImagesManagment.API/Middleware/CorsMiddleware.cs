namespace ImagesManagment.API.Middleware
{
	// Access-Control-Allow-Origin не применяется к статическим файлам, которые
	// обслуживаются через UseStaticFiles
	public class CorsMiddleware
	{
		private readonly RequestDelegate _next;

		public CorsMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task Invoke(HttpContext context)
		{
			context.Response.OnStarting(() =>
			{
				context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
				context.Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
				context.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Authorization");
				return Task.CompletedTask;
			});

			await _next(context);
		}
	}
}
