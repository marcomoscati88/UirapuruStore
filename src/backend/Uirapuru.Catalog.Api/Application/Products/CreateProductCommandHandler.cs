using MediatR;
using Uirapuru.Catalog.Api.Application.Categories.Repositories;
using Uirapuru.Catalog.Api.Application.Products.Repositories;
using Uirapuru.Catalog.Api.Domains.Categories;
using Uirapuru.Catalog.Api.Domains.Products;

namespace Uirapuru.Catalog.Api.Application.Products
{
	public sealed class CreateProductCommandHandler : IRequestHandler<CreateProductCommand.Input, CreateProductCommand.Result>
	{
		private readonly IProductRepository productRepository;
		private readonly ICategoryRepository categoryRepository;

		public CreateProductCommandHandler(IProductRepository productRepository, ICategoryRepository categoryRepository)
		{
			this.productRepository = productRepository;
			this.categoryRepository = categoryRepository;
		}

		public async Task<CreateProductCommand.Result> Handle(CreateProductCommand.Input command, CancellationToken cancellationToken)
		{
			CreateProductCommand.Result result = new();

			await ValidateCommand(command, result, cancellationToken);

			if (!result.IsSuccess)
			{
				return result;
			}

			Product product = Product.Create(
				name: command.Name,
				description: command.Description,
				price: command.Price,
				idCategory: (command.IdCategory != null) ? new CategoryID(command.IdCategory.Value) : null,
				subCategory: command.SubCategory,
				createdAtUtc: DateTime.UtcNow,
				lastModified: null);

			await productRepository.AddAsync(product, cancellationToken);

			return result;
		}

		private async Task ValidateCommand(CreateProductCommand.Input command, CreateProductCommand.Result result, CancellationToken cancellationToken)
		{
			if (string.IsNullOrWhiteSpace(command.Name) || command.Name.Length > 255)
			{
				result.SetError("Il prodotto deve avere un nome e il nome non può eccedere i 255 caratteri.");
			}

			if (!string.IsNullOrWhiteSpace(command.Description) && command.Description.Length > 500)
			{
				result.SetError("La descrizione non può eccedere i 500 caratteri.");
			}

			if (command.Price.HasValue && command.Price.Value < 0)
			{
				result.SetError("Il prezzo non può essere negativo.");
			}

			if (command.IdCategory != null)
			{
				Category categoryExists =await categoryRepository.GetByIdAsync(new CategoryID(command.IdCategory.Value), cancellationToken);

				if (categoryExists == null) 
				{
					result.SetError("La categoria selezionata non esiste.");
				}
			}
		}
	}
}
