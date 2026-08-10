using Uirapuru.Catalog.Api.Application.Common.Repositories;
using Uirapuru.Catalog.Api.Domains.Categories;

namespace Uirapuru.Catalog.Api.Application.Categories.Repositories;

public interface ICategoryRepository : IBaseRepository<Category, CategoryID>
{
	Task<IReadOnlyList<Category>> GetListAsync(CancellationToken cancellationToken = default);
	Task<bool> ExistsWithNameAsync(string name, CategoryID idCategory, CancellationToken cancellationToken = default);
}
