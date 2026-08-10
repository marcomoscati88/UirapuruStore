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
		public bool IsSuccess { get; private set; }

		public string ErrorMessage { get; private set; }

		public object Object { get; private set; }

		public Result()
		{
			IsSuccess = true;
			ErrorMessage = string.Empty;
			Object = null;
		}

		public void SetError(string errorMessage)
		{
			IsSuccess = false;
			ErrorMessage = errorMessage;
		}
	}
}
