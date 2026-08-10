using Uirapuru.Catalog.Api.Application.Common.Messaging;

namespace Uirapuru.Catalog.Api.Application.Products
{
	public static class GetProductListQuery
	{
		public sealed class Input : IQuery<Result>
		{
		}

		public sealed class Result
		{
			public bool IsSuccess { get; private set; }

			public string ErrorMessage { get; private set; }

			public IReadOnlyList<ProductListItem> Object { get; private set; }

			public Result(bool isSuccess, string errorMessage, IReadOnlyList<ProductListItem> @object)
			{
				IsSuccess = isSuccess;
				ErrorMessage = errorMessage;
				Object = @object;
			}
		}

		public sealed class ProductListItem
		{
			public int Id { get; private set; }

			public string Name { get; private set; }

			public string Description { get; private set; }

			public decimal? Price { get; private set; }

			public byte? IdCategory { get; private set; }

			public string SubCategory { get; private set; }

			public bool IsEnabled { get; private set; }

			public string ImagePath { get; private set; }

			public ProductListItem(int id, string name, string description, decimal? price, byte? idCategory, string subCategory, bool isEnabled, string imagePath)
			{
				Id = id;
				Name = name;
				Description = description;
				Price = price;
				IdCategory = idCategory;
				SubCategory = subCategory;
				IsEnabled = isEnabled;
				ImagePath = imagePath;
			}
		}
	}
}
