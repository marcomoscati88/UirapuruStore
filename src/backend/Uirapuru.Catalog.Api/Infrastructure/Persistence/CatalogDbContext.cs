using Microsoft.EntityFrameworkCore;
using Uirapuru.Catalog.Api.Domains.Categories;
using Uirapuru.Catalog.Api.Domains.Products;

namespace Uirapuru.Catalog.Api.Infrastructure.Persistence;

public sealed class CatalogDbContext : DbContext
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
}
