using MediatR;
using Uirapuru.Catalog.Api.Application.Categories.Repositories;
using Uirapuru.Catalog.Api.Application.Products.Repositories;
using Uirapuru.Catalog.Api.Domains.Products;

namespace Uirapuru.Catalog.Api.Application.Products
{
	public class EnableProductCommandHandler : IRequestHandler<EnableProductCommand.Input, EnableProductCommand.Result>
	{
		private readonly IProductRepository productRepository;
		private readonly ICategoryRepository categoryRepository;

		public EnableProductCommandHandler(IProductRepository productRepository, ICategoryRepository categoryRepository)
		{
			this.productRepository = productRepository;
			this.categoryRepository = categoryRepository;
		}

		public async Task<EnableProductCommand.Result> Handle(EnableProductCommand.Input command, CancellationToken cancellationToken)
		{
			EnableProductCommand.Result result = new();

			await ValidateCommand(command, result, cancellationToken);

			if (!result.IsSuccess)
			{
				return result;
			}

			Product productToUpdate = await productRepository.GetByIdAsync(new ProductID(command.Id), cancellationToken);

			productToUpdate.Enable();

			return result;
		}

		private async Task ValidateCommand(EnableProductCommand.Input command, EnableProductCommand.Result result, CancellationToken cancellationToken)
		{
			Product productToUpdate = await productRepository.GetByIdAsync(new ProductID(command.Id), cancellationToken);

			if (productToUpdate == null)
			{
				result.SetError("Il prodotto da abilitare non esiste.");
				return;
			}
		}
	}
}