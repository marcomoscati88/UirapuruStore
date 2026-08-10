using Microsoft.AspNetCore.Http;
using Uirapuru.Catalog.Api.Application.Common.Messaging;

namespace Uirapuru.Catalog.Api.Application.Products
{
	public sealed class UpdateProductImageCommand
	{
		public sealed class Input : ICommand<Result>
		{
			public int Id { get; set; }

			public IFormFile Image { get; set; }
		}

		public sealed class Result : ICommandResult
		{
			public bool IsSuccess { get; private set; }

			public string ErrorMessage { get; private set; }

			public string Object { get; private set; }

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

			public void SetObject(string imagePath)
			{
				Object = imagePath;
			}
		}
	}
}
