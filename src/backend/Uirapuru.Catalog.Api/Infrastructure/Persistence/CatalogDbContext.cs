using Microsoft.EntityFrameworkCore;
using Uirapuru.Catalog.Api.Application.Common.Persistence;
using Uirapuru.Catalog.Api.Domains.Categories;
using Uirapuru.Catalog.Api.Domains.Products;

namespace Uirapuru.Catalog.Api.Infrastructure.Persistence;

public sealed class CatalogDbContext : DbContext, IUnitOfWork
{
	public CatalogDbContext(DbContextOptions<CatalogDbContext> options)
		: base(options)
	{
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.HasDefaultSchema("catalog");

		modelBuilder.ApplyConfigurationsFromAssembly(
			typeof(CatalogDbContext).Assembly);

		base.OnModelCreating(modelBuilder);
	}

	public DbSet<Product> Products => Set<Product>();
	public DbSet<Category> Categories => Set<Category>();

	public async Task<TResponse> ExecuteInTransactionAsync<TResponse>(
		Func<CancellationToken, Task<TResponse>> operation,
		CancellationToken cancellationToken = default)
	{
		if (Database.CurrentTransaction is not null)
		{
			return await operation(cancellationToken);
		}

		await using var transaction =
			await Database.BeginTransactionAsync(cancellationToken);

		try
		{
			TResponse response = await operation(cancellationToken);

			await transaction.CommitAsync(cancellationToken);

			return response;
		}
		catch
		{
			await transaction.RollbackAsync(CancellationToken.None);
			throw;
		}
	}
}
