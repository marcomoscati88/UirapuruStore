using MediatR;
using Uirapuru.Catalog.Api.Application.Categories.Repositories;
using Uirapuru.Catalog.Api.Domains.Categories;

namespace Uirapuru.Catalog.Api.Application.Categories
{
	public sealed class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand.Input, UpdateCategoryCommand.Result>
	{
		private readonly ICategoryRepository categoryRepository;

		public UpdateCategoryCommandHandler(ICategoryRepository categoryRepository)
		{
			this.categoryRepository = categoryRepository;
		}

		public async Task<UpdateCategoryCommand.Result> Handle(UpdateCategoryCommand.Input command, CancellationToken cancellationToken)
		{
			UpdateCategoryCommand.Result result = new();

			await ValidateCommand(command, result, cancellationToken);

			if (!result.IsSuccess)
			{
				return result;
			}

			Category categoryToUpdate = await categoryRepository.GetByIdAsync(new CategoryID(command.Id), cancellationToken);

			if (categoryToUpdate.Name.Equals(command.Name))
			{
				result.SetError("Il nome della categoria non è stato modificato.");
				return result;
			}

			categoryToUpdate.Update(
				name: command.Name,
				updateNow: DateTimeOffset.UtcNow);

			return result;
		}

		private async Task ValidateCommand(UpdateCategoryCommand.Input command, UpdateCategoryCommand.Result result, CancellationToken cancellationToken)
		{
			if (string.IsNullOrWhiteSpace(command.Name) || command.Name.Length > 255)
			{
				result.SetError("La categoria deve avere un nome e il nome non può eccedere i 255 caratteri.");
				return;
			}

			if (!string.IsNullOrWhiteSpace(command.Name)) 
			{
				bool exists = await categoryRepository.ExistsWithNameAsync(command.Name, new CategoryID(command.Id), cancellationToken);

				if (exists)
				{
					result.SetError("Esiste già una categoria con questo nome.");
					return;
				}
			}
		}
	}
}
