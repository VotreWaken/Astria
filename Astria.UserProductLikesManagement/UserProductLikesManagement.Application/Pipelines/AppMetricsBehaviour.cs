﻿using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserProductLikesManagement.Application.Pipelines
{
	public class MetricsBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
	{
		private readonly ILogger<MetricsBehaviour<TRequest, TResponse>> _logger;

		public MetricsBehaviour(ILogger<MetricsBehaviour<TRequest, TResponse>> logger)
		{
			_logger = logger;
		}
		public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
		{
			var stopwatch = Stopwatch.StartNew();

			var result = await next();

			stopwatch.Stop();

			_logger.LogInformation($"Finished in {stopwatch.ElapsedMilliseconds}ms");

			return result;
		}
	}
}
