namespace Uirapuru.Catalog.Api.Application.Common.Repositories;

public interface IBaseRepository<TEntity, in TId>
	where TEntity : class
{
	Task<TEntity> GetByIdAsync(
		TId id,
		CancellationToken cancellationToken = default);

	Task AddAsync(
		TEntity entity,
		CancellationToken cancellationToken = default);

	void Delete(TEntity entity);
}
