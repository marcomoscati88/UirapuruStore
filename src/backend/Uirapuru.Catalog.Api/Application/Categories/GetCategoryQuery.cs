using Uirapuru.Catalog.Api.Application.Common.Messaging;

namespace Uirapuru.Catalog.Api.Application.Categories;

public static class GetCategoryQuery
{
	public sealed class Input : IQuery<Result>
	{
		public byte IdCategory { get; set; }
	}

	public sealed class Result
	{
		public bool IsSuccess { get; private set; }

		public string ErrorMessage { get; private set; }

		public CategoryResultItem Object { get; private set; }

		public Result(bool isSuccess, string errorMessage, CategoryResultItem @object)
		{
			IsSuccess = isSuccess;
			ErrorMessage = errorMessage;
			Object = @object;
		}
	}

	public sealed class CategoryResultItem
	{
		public byte Id { get; private set; }

		public string Name { get; private set; }

		public bool IsEnabled { get; private set; }

		public CategoryResultItem(byte id, string name, bool isEnabled)
		{
			Id = id;
			Name = name;
			IsEnabled = isEnabled;
		}
	}
}
