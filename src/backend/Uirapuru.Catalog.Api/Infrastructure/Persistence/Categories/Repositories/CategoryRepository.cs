using Uirapuru.Catalog.Api.Application.Categories.Repositories;
using Uirapuru.Catalog.Api.Domains.Categories;

namespace Uirapuru.Catalog.Api.Infrastructure.Persistence.Categories.Repositories;

public sealed class CategoryRepository
	: BaseRepository<Category, CategoryID>,
	  ICategoryRepository
{
	public CategoryRepository(CatalogDbContext dbContext)
		: base(dbContext)
	{
	}
}
