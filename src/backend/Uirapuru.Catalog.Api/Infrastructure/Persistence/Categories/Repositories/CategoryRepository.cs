using Microsoft.EntityFrameworkCore;
using Uirapuru.Catalog.Api.Application.Categories.Repositories;
using Uirapuru.Catalog.Api.Domains.Categories;

namespace Uirapuru.Catalog.Api.Infrastructure.Persistence.Categories.Repositories;

public sealed class CategoryRepository : BaseRepository<Category, CategoryID>, ICategoryRepository
{
	public CategoryRepository(CatalogDbContext dbContext) : base(dbContext)
	{
	}

	public async Task<IReadOnlyList<Category>> GetListAsync(CancellationToken cancellationToken = default)
	{
		return await Entities
			.AsNoTracking()
			.OrderBy(category => category.Id)
			.ToListAsync(cancellationToken);
	}

	public async Task<bool> ExistsWithNameAsync(string name, CategoryID idCategory, CancellationToken cancellationToken = default)
	{
		return await Entities
			.AsNoTracking()
			.AnyAsync(category => category.Name == name && category.Id != idCategory, cancellationToken);
	}
}
