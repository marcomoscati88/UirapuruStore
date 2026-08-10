using MediatR;
using Uirapuru.Catalog.Api.Application.Products.Repositories;
using Uirapuru.Catalog.Api.Domains.Products;

namespace Uirapuru.Catalog.Api.Application.Products
{
	public sealed class GetProductQueryHandler : IRequestHandler<GetProductQuery.Input, GetProductQuery.Result>
	{
		private readonly IProductRepository productRepository;

		public GetProductQueryHandler(IProductRepository productRepository)
		{
			this.productRepository = productRepository;
		}

		public async Task<GetProductQuery.Result> Handle(GetProductQuery.Input query, CancellationToken cancellationToken)
		{
			Product product = await productRepository.GetByIdAsync(new ProductID(query.IdProduct), cancellationToken);

			if (product is null)
			{
				return new GetProductQuery.Result(false, "Prodotto non trovato.", null);
			}

			GetProductQuery.ProductResultItem item = new GetProductQuery.ProductResultItem(product.Id.Value, product.Name, product.Description, product.Price, product.IdCategory?.Value, product.SubCategory, product.IsEnabled, product.ImagePath);

			return new GetProductQuery.Result(true, string.Empty, item);
		}
	}
}
