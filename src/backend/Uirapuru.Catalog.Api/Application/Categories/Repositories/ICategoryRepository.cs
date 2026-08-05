using Uirapuru.Catalog.Api.Application.Common.Repositories;
using Uirapuru.Catalog.Api.Domains.Categories;

namespace Uirapuru.Catalog.Api.Application.Categories.Repositories;

public interface ICategoryRepository
	: IBaseRepository<Category, CategoryID>
{
}
