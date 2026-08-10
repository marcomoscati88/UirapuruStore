using MediatR;
using Uirapuru.Catalog.Api.Application.Products.Repositories;
using Uirapuru.Catalog.Api.Application.Products.Storage;
using Uirapuru.Catalog.Api.Domains.Products;

namespace Uirapuru.Catalog.Api.Application.Products
{
	public sealed class UpdateProductImageCommandHandler : IRequestHandler<UpdateProductImageCommand.Input, UpdateProductImageCommand.Result>
	{
		private readonly IProductRepository productRepository;
		private readonly IProductImageStorage productImageStorage;
		private readonly long maxFileSizeInBytes;

		public UpdateProductImageCommandHandler(IProductRepository productRepository, IProductImageStorage productImageStorage, IConfiguration configuration)
		{
			this.productRepository = productRepository;
			this.productImageStorage = productImageStorage;
			maxFileSizeInBytes = configuration.GetValue<long>("ProductImages:MaxFileSizeInBytes");
		}

		public async Task<UpdateProductImageCommand.Result> Handle(UpdateProductImageCommand.Input command, CancellationToken cancellationToken)
		{
			UpdateProductImageCommand.Result result = new();

			string extension = ValidateCommand(command, result);

			if (!result.IsSuccess)
			{
				return result;
			}

			Product productToUpdate = await productRepository.GetByIdAsync(new ProductID(command.Id), cancellationToken);

			if (productToUpdate == null)
			{
				result.SetError("Il prodotto da aggiornare non esiste.");
				return result;
			}

			await using Stream imageStream = command.Image.OpenReadStream();
			string imagePath = await productImageStorage.SaveAsync(command.Id, imageStream, extension, cancellationToken);

			productToUpdate.UpdateImage(imagePath);
			result.SetObject(imagePath);

			return result;
		}

		private string ValidateCommand(UpdateProductImageCommand.Input command, UpdateProductImageCommand.Result result)
		{
			if (command.Id <= 0)
			{
				result.SetError("Il prodotto selezionato non è valido.");
				return string.Empty;
			}

			if (command.Image == null || command.Image.Length == 0)
			{
				result.SetError("Selezionare un'immagine da caricare.");
				return string.Empty;
			}

			if (command.Image.Length > maxFileSizeInBytes)
			{
				result.SetError("L'immagine non può superare i 5 MB.");
				return string.Empty;
			}

			return command.Image.ContentType.ToLowerInvariant() switch
			{
				"image/jpeg" => ".jpg",
				"image/png" => ".png",
				"image/webp" => ".webp",
				_ => SetInvalidFormat(result)
			};
		}

		private string SetInvalidFormat(UpdateProductImageCommand.Result result)
		{
			result.SetError("Il formato dell'immagine deve essere JPEG, PNG o WebP.");
			return string.Empty;
		}
	}
}
