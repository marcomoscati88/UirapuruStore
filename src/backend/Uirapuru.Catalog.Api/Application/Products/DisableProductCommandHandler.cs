using MediatR;
using Uirapuru.Catalog.Api.Application.Categories.Repositories;
using Uirapuru.Catalog.Api.Application.Products.Repositories;
using Uirapuru.Catalog.Api.Domains.Categories;
using Uirapuru.Catalog.Api.Domains.Products;

namespace Uirapuru.Catalog.Api.Application.Products
{
	public class DisableProductCommandHandler : IRequestHandler<DisableProductCommand.Input, DisableProductCommand.Result>
	{
		private readonly IProductRepository productRepository;
		private readonly ICategoryRepository categoryRepository;

		public DisableProductCommandHandler(IProductRepository productRepository, ICategoryRepository categoryRepository)
		{
			this.productRepository = productRepository;
			this.categoryRepository = categoryRepository;
		}

		public async Task<DisableProductCommand.Result> Handle(DisableProductCommand.Input command, CancellationToken cancellationToken)
		{
			DisableProductCommand.Result result = new();

			await ValidateCommand(command, result, cancellationToken);

			if (!result.IsSuccess)
			{
				return result;
			}

			Product productToUpdate = await productRepository.GetByIdAsync(new ProductID(command.Id), cancellationToken);

			productToUpdate.Disable();

			return result;
		}

		private async Task ValidateCommand(DisableProductCommand.Input command, DisableProductCommand.Result result, CancellationToken cancellationToken)
		{
			Product productToUpdate = await productRepository.GetByIdAsync(new ProductID(command.Id), cancellationToken);

			if (productToUpdate == null)
			{
				result.SetError("Il prodotto da disabilitare non esiste.");
				return;
			}
		}
	}
}