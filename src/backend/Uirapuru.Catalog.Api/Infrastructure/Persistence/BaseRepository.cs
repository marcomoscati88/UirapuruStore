using Microsoft.EntityFrameworkCore;
using Uirapuru.Catalog.Api.Application.Common.Repositories;

namespace Uirapuru.Catalog.Api.Infrastructure.Persistence;

public abstract class BaseRepository<TEntity, TId>
	: IBaseRepository<TEntity, TId>
	where TEntity : class
{
	protected readonly CatalogDbContext DbContext;
	protected readonly DbSet<TEntity> Entities;

	protected BaseRepository(CatalogDbContext dbContext)
	{
		DbContext = dbContext;
		Entities = dbContext.Set<TEntity>();
	}

	public async Task<TEntity?> GetByIdAsync(
		TId id,
		CancellationToken cancellationToken = default)
	{
		return await Entities.FindAsync(
			[id],
			cancellationToken);
	}

	public async Task AddAsync(
		TEntity entity,
		CancellationToken cancellationToken = default)
	{
		await Entities.AddAsync(entity, cancellationToken);
	}

	public void Delete(TEntity entity)
	{
		Entities.Remove(entity);
	}
}
