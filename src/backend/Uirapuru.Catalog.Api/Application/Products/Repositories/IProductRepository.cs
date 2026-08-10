using Uirapuru.Catalog.Api.Application.Common.Repositories;
using Uirapuru.Catalog.Api.Domains.Products;

namespace Uirapuru.Catalog.Api.Application.Products.Repositories
{
	public interface IProductRepository : IBaseRepository<Product, ProductID>
	{
		Task<IReadOnlyList<Product>> GetListAsync(CancellationToken cancellationToken = default);
	}
}
