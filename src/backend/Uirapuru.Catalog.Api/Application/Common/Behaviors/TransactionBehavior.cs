using MediatR;
using Uirapuru.Catalog.Api.Application.Common.Messaging;
using Uirapuru.Catalog.Api.Application.Common.Persistence;

namespace Uirapuru.Catalog.Api.Application.Common.Behaviors;

public sealed class TransactionBehavior<TRequest, TResponse>
	: IPipelineBehavior<TRequest, TResponse>
	where TRequest : ICommand<TResponse>
	where TResponse : ICommandResult
{
	private readonly IUnitOfWork _unitOfWork;

	public TransactionBehavior(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
	}

	public Task<TResponse> Handle(
		TRequest request,
		RequestHandlerDelegate<TResponse> next,
		CancellationToken cancellationToken)
	{
		return _unitOfWork.ExecuteInTransactionAsync(
			async transactionCancellationToken =>
			{
				TResponse response = await next();

				if (response.IsSuccess)
				{
					await _unitOfWork.SaveChangesAsync(
						transactionCancellationToken);
				}

				return response;
			},
			response => response.IsSuccess,
			cancellationToken);
	}
}
