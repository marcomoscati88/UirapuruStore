using MediatR;
using Uirapuru.Catalog.Api.Application.Categories.Repositories;
using Uirapuru.Catalog.Api.Domains.Categories;

namespace Uirapuru.Catalog.Api.Application.Categories
{
	public class DisableCategoryCommandHandler : IRequestHandler<DisableCategoryCommand.Input, DisableCategoryCommand.Result>
	{
		private readonly ICategoryRepository categoryRepository;

		public DisableCategoryCommandHandler(ICategoryRepository categoryRepository)
		{
			this.categoryRepository = categoryRepository;
		}

		public async Task<DisableCategoryCommand.Result> Handle(DisableCategoryCommand.Input command, CancellationToken cancellationToken)
		{
			DisableCategoryCommand.Result result = new();

			await ValidateCommand(command, result, cancellationToken);

			if (!result.IsSuccess)
			{
				return result;
			}

			Category categoryToUpdate = await categoryRepository.GetByIdAsync(new CategoryID(command.Id), cancellationToken);

			categoryToUpdate.Disable();

			return result;
		}

		private async Task ValidateCommand(DisableCategoryCommand.Input command, DisableCategoryCommand.Result result, CancellationToken cancellationToken)
		{
			Category categoryToUpdate = await categoryRepository.GetByIdAsync(new CategoryID(command.Id), cancellationToken);

			if (categoryToUpdate == null)
			{
				result.SetError("La categoria da disabilitare non esiste.");
				return;
			}
		}
	}
}
