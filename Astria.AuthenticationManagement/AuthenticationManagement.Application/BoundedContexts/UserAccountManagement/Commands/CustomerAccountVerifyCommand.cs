﻿using AuthenticationManagement.Application.Results;
using AuthenticationManagement.Domain.BoundedContexts.UserAccountManagement.Aggregates;
using AuthenticationManagement.Domain.Exceptions;
using Astria.EventSourcingRepository.Repository;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AuthenticationManagement.Application.BoundedContexts.UserAccountManagement.Commands
{
	public class CustomerAccountVerifyCommand : IRequest<CommandResult>
	{
		public Guid UserId { get; set; }
	}

	public class CustomerAccountVerifyCommandHandler : IRequestHandler<CustomerAccountVerifyCommand, CommandResult>
	{
		private readonly IEventSourcingRepository<Customer> _repository;
		public CustomerAccountVerifyCommandHandler(IEventSourcingRepository<Customer> repository)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		public async Task<CommandResult> Handle(CustomerAccountVerifyCommand command, CancellationToken cancellationToken)
		{
			try
			{
				var customer = await _repository.FindByIdAsync(command.UserId);
				if (customer is null)
					return CommandResult.NotFound(command.UserId);

				customer.VerifyAccount();

				await _repository.SaveAsync(customer);
				return CommandResult.Success(customer.AggregateId);
			}
			catch (DomainException ex)
			{
				return CommandResult.BusinessFail(ex.Message);
			}
		}
	}
}
