using MediatR;

namespace Uirapuru.Catalog.Api.Application.Common.Messaging;

public interface IQuery<out TResult> : IRequest<TResult>
{
}
