using MediatR;

namespace Uirapuru.Catalog.Api.Application.Common.Messaging;

public interface ICommand
{
}

public interface ICommand<out TResult> : IRequest<TResult>, ICommand
	where TResult : ICommandResult
{
}

public interface ICommandResult
{
	bool IsSuccess { get; }

	string ErrorMessage { get; }
}
