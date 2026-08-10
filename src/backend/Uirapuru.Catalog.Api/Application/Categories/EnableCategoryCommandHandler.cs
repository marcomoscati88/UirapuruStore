using MediatR;
using Uirapuru.Catalog.Api.Application.Categories.Repositories;
using Uirapuru.Catalog.Api.Domains.Categories;

namespace Uirapuru.Catalog.Api.Application.Categories
{
	public class EnableCategoryCommandHandler : IRequestHandler<EnableCategoryCommand.Input, EnableCategoryCommand.Result>
	{
		private readonly ICategoryRepository categoryRepository;

		public EnableCategoryCommandHandler(ICategoryRepository categoryRepository)
		{
			this.categoryRepository = categoryRepository;
		}

		public async Task<EnableCategoryCommand.Result> Handle(EnableCategoryCommand.Input command, CancellationToken cancellationToken)
		{
			EnableCategoryCommand.Result result = new();

			await ValidateCommand(command, result, cancellationToken);

			if (!result.IsSuccess)
			{
				return result;
			}

			Category categoryToUpdate = await categoryRepository.GetByIdAsync(new CategoryID(command.Id), cancellationToken);

			categoryToUpdate.Enable();

			return result;
		}

		private async Task ValidateCommand(EnableCategoryCommand.Input command, EnableCategoryCommand.Result result, CancellationToken cancellationToken)
		{
			Category categoryToUpdate = await categoryRepository.GetByIdAsync(new CategoryID(command.Id), cancellationToken);

			if (categoryToUpdate == null)
			{
				result.SetError("La categoria da abilitare non esiste.");
				return;
			}
		}
	}
}
