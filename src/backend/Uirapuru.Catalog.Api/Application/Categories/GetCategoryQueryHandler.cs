using MediatR;
using Uirapuru.Catalog.Api.Application.Categories.Repositories;
using Uirapuru.Catalog.Api.Domains.Categories;

namespace Uirapuru.Catalog.Api.Application.Categories;

public sealed class GetCategoryQueryHandler : IRequestHandler<GetCategoryQuery.Input, GetCategoryQuery.Result>
{
	private readonly ICategoryRepository categoryRepository;

	public GetCategoryQueryHandler(ICategoryRepository categoryRepository)
	{
		this.categoryRepository = categoryRepository;
	}

	public async Task<GetCategoryQuery.Result> Handle(GetCategoryQuery.Input query, CancellationToken cancellationToken)
	{
		Category category = await categoryRepository.GetByIdAsync(
			new CategoryID(query.IdCategory),
			cancellationToken);

		if (category is null)
		{
			return new GetCategoryQuery.Result(false, "Categoria non trovata.", null);
		}

		GetCategoryQuery.CategoryResultItem item = new GetCategoryQuery.CategoryResultItem(category.Id.Value, category.Name, category.IsEnabled);

		return new GetCategoryQuery.Result(true, string.Empty, item);
	}
}
