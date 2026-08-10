using MediatR;
using Uirapuru.Catalog.Api.Application.Categories.Repositories;
using Uirapuru.Catalog.Api.Domains.Categories;

namespace Uirapuru.Catalog.Api.Application.Categories;

public sealed class GetCategoryListQueryHandler : IRequestHandler<GetCategoryListQuery.Input, GetCategoryListQuery.Result>
{
	private readonly ICategoryRepository categoryRepository;

	public GetCategoryListQueryHandler(ICategoryRepository categoryRepository)
	{
		this.categoryRepository = categoryRepository;
	}

	public async Task<GetCategoryListQuery.Result> Handle(GetCategoryListQuery.Input query, CancellationToken cancellationToken)
	{
		IReadOnlyList<Category> categories = await categoryRepository.GetListAsync(cancellationToken);

		var resultsList = categories.Select(category => new GetCategoryListQuery.CategoryListItem(category.Id.Value, category.Name, category.IsEnabled)).ToList();

		return new GetCategoryListQuery.Result(true, string.Empty, resultsList);
	}
}
