using Uirapuru.Catalog.Api.Application.Common.Messaging;

namespace Uirapuru.Catalog.Api.Application.Categories;

public static class GetCategoryListQuery
{
	public sealed class Input : IQuery<Result>
	{
	}

	public sealed class Result
	{
		public bool IsSuccess { get; private set; }

		public string ErrorMessage { get; private set; }

		public IReadOnlyList<CategoryListItem> Object { get; private set; }

		public Result(bool isSuccess, string errorMessage, IReadOnlyList<CategoryListItem> @object)
		{
			IsSuccess = isSuccess;
			ErrorMessage = errorMessage;
			Object = @object;
		}
	}

	public sealed class CategoryListItem
	{
		public byte Id { get; private set; }

		public string Name { get; private set; }

		public bool IsEnabled { get; private set; }

		public CategoryListItem(byte id, string name, bool isEnabled)
		{
			Id = id;
			Name = name;
			IsEnabled = isEnabled;
		}
	}
}
