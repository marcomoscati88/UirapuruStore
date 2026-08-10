using Microsoft.EntityFrameworkCore;
using Uirapuru.Catalog.Api.Application.Products.Repositories;
using Uirapuru.Catalog.Api.Domains.Products;

namespace Uirapuru.Catalog.Api.Infrastructure.Persistence.Products.Repositories
{
	public sealed class ProductRepository : BaseRepository<Product, ProductID>, IProductRepository
	{
		public ProductRepository(CatalogDbContext dbContext) : base(dbContext)
		{
		}

		public async Task<IReadOnlyList<Product>> GetListAsync(CancellationToken cancellationToken = default)
		{
			return await Entities
				.AsNoTracking()
				.OrderBy(product => product.Id)
				.ToListAsync(cancellationToken);
		}
	}
}
