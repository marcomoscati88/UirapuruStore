using Uirapuru.Catalog.Api.Application.Common.Messaging;

namespace Uirapuru.Catalog.Api.Application.Categories;

public sealed class CreateCategoryCommand : ICommand
{
	public string Name { get; set; } = null!;
}
