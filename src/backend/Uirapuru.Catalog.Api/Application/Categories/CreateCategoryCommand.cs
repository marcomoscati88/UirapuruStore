using Uirapuru.Catalog.Api.Application.Common.Messaging;

namespace Uirapuru.Catalog.Api.Application.Categories;

public static class CreateCategoryCommand
{
	public sealed class Input : ICommand<Result>
	{
		public string Name { get; set; } = null!;
	}

	public sealed class Result : ICommandResult
	{
		public bool IsSuccess { get; set; }

		public string ErrorMessage { get; set; } = string.Empty;
	}
}
