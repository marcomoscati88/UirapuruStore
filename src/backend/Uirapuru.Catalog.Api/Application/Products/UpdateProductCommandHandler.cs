using MediatR;
using Uirapuru.Catalog.Api.Application.Categories.Repositories;
using Uirapuru.Catalog.Api.Application.Products.Repositories;
using Uirapuru.Catalog.Api.Domains.Categories;
using Uirapuru.Catalog.Api.Domains.Products;

namespace Uirapuru.Catalog.Api.Application.Products
{
	public sealed class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand.Input, UpdateProductCommand.Result>
	{
		private readonly IProductRepository productRepository;
		private readonly ICategoryRepository categoryRepository;

		public UpdateProductCommandHandler(IProductRepository productRepository, ICategoryRepository categoryRepository)
		{
			this.productRepository = productRepository;
			this.categoryRepository = categoryRepository;
		}

		public async Task<UpdateProductCommand.Result> Handle(UpdateProductCommand.Input command, CancellationToken cancellationToken)
		{
			UpdateProductCommand.Result result = new();

			await ValidateCommand(command, result, cancellationToken);

			if (!result.IsSuccess)
			{
				return result;
			}

			Product productToUpdate = await productRepository.GetByIdAsync(new ProductID(command.Id), cancellationToken);

			productToUpdate.Update(
				description: command.Description,
				price: command.Price,
				idCategory: (command.IdCategory.HasValue) ? new CategoryID(command.IdCategory.Value) : null,
				subCategory: command.SubCategory,
				lastModified: DateTime.UtcNow);

			return result;
		}

		private async Task ValidateCommand(UpdateProductCommand.Input command, UpdateProductCommand.Result result, CancellationToken cancellationToken)
		{
			if (!string.IsNullOrWhiteSpace(command.Description) && command.Description.Length > 500)
			{
				result.SetError("La descrizione non può eccedere i 500 caratteri.");
			}

			if (command.Price.HasValue && command.Price.Value < 0)
			{
				result.SetError("Il prezzo non può essere negativo.");
			}

			if (command.IdCategory.HasValue)
			{
				Category categoryExists = await categoryRepository.GetByIdAsync(new CategoryID(command.IdCategory.Value), cancellationToken);

				if (categoryExists == null)
				{
					result.SetError("La categoria selezionata non esiste.");
				}
			}
		}
	}
}
