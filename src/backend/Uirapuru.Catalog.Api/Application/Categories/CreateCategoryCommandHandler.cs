using MediatR;
using System.ComponentModel;
using Uirapuru.Catalog.Api.Application.Categories.Repositories;
using Uirapuru.Catalog.Api.Domains.Categories;

namespace Uirapuru.Catalog.Api.Application.Categories;

public sealed class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand>
{
	private readonly ICategoryRepository _categoryRepository;

	public CreateCategoryCommandHandler(ICategoryRepository categoryRepository)
	{
		_categoryRepository = categoryRepository;
	}

	public async Task Handle(CreateCategoryCommand command, CancellationToken cancellationToken)
	{
		await ValidateCommand(command);

		Category category = Category.Create(
				name: command.Name,
				createdNow: DateTimeOffset.UtcNow);

		await _categoryRepository.AddAsync(category, cancellationToken);
	}

	private async Task ValidateCommand(CreateCategoryCommand command)
	{
		if (string.IsNullOrWhiteSpace(command.Name) || command.Name.Length > 255)
		{
			throw new ArgumentOutOfRangeException(nameof(command.Name), "La categoria deve avere un nome e, il nome, non può eccedere i 255 caratteri.");
		}
	}
}
