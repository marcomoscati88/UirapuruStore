using MediatR;
using Uirapuru.Catalog.Api.Application.Products.Repositories;
using Uirapuru.Catalog.Api.Domains.Products;

namespace Uirapuru.Catalog.Api.Application.Products
{
	public sealed class GetProductListQueryHandler : IRequestHandler<GetProductListQuery.Input, GetProductListQuery.Result>
	{
		private readonly IProductRepository productRepository;

		public GetProductListQueryHandler(IProductRepository productRepository)
		{
			this.productRepository = productRepository;
		}

		public async Task<GetProductListQuery.Result> Handle(GetProductListQuery.Input query, CancellationToken cancellationToken)
		{
			IReadOnlyList<Product> products = await productRepository.GetListAsync(cancellationToken);

			var resultsList = products.Select(product => new GetProductListQuery.ProductListItem(product.Id.Value, product.Name, product.Description, product.Price, product.IdCategory?.Value, product.SubCategory, product.IsEnabled, product.ImagePath)).ToList();

			return new GetProductListQuery.Result(true, string.Empty, resultsList);
		}
	}
}
