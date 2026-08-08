namespace Uirapuru.Catalog.Api.Application.Common.Persistence;

public interface IUnitOfWork
{
	Task<TResponse> ExecuteInTransactionAsync<TResponse>(
		Func<CancellationToken, Task<TResponse>> operation,
		Func<TResponse, bool> shouldCommit,
		CancellationToken cancellationToken = default);

	Task<int> SaveChangesAsync(
		CancellationToken cancellationToken = default);
}
