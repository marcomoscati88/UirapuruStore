using MediatR;
using Uirapuru.Catalog.Api.Application.Categories.Repositories;
using Uirapuru.Catalog.Api.Domains.Categories;

namespace Uirapuru.Catalog.Api.Application.Categories;

public sealed class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand.Input, CreateCategoryCommand.Result>
{
	private readonly ICategoryRepository _categoryRepository;

	public CreateCategoryCommandHandler(ICategoryRepository categoryRepository)
	{
		_categoryRepository = categoryRepository;
	}

	public async Task<CreateCategoryCommand.Result> Handle(CreateCategoryCommand.Input command, CancellationToken cancellationToken)
	{
		CreateCategoryCommand.Result result = new()
		{
			IsSuccess = true
		};

		await ValidateCommand(command, result);

		if (!result.IsSuccess)
		{
			return result;
		}

		Category category = Category.Create(
			name: command.Name,
			createdNow: DateTimeOffset.UtcNow);

		await _categoryRepository.AddAsync(category, cancellationToken);

		return result;
	}

	private static Task ValidateCommand( CreateCategoryCommand.Input command, CreateCategoryCommand.Result result)
	{
		if (string.IsNullOrWhiteSpace(command.Name) || command.Name.Length > 255)
		{
			result.IsSuccess = false;
			result.ErrorMessage = "La categoria deve avere un nome e il nome non può eccedere i 255 caratteri.";
		}

		return Task.CompletedTask;
	}
}
