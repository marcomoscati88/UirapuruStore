using Uirapuru.Catalog.Api.Application.Common.Messaging;

namespace Uirapuru.Catalog.Api.Application.Products
{
	public static class UpdateProductCommand
	{
		public sealed class Input : ICommand<Result>
		{
			public int Id { get; set; }

			public string Name { get; set; }

			public string Description { get; set; }

			public decimal? Price { get; set; }

			public byte? IdCategory { get; set; }

			public string SubCategory { get; set; }
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
}
