namespace Uirapuru.Catalog.Api.Application.Common.Persistence;

public interface IUnitOfWork
{
	Task<TResponse> ExecuteInTransactionAsync<TResponse>(
		Func<CancellationToken, Task<TResponse>> operation,
		CancellationToken cancellationToken = default);

	Task<int> SaveChangesAsync(
		CancellationToken cancellationToken = default);
}
