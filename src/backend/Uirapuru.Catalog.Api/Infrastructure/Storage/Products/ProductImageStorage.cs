using Uirapuru.Catalog.Api.Application.Products.Storage;

namespace Uirapuru.Catalog.Api.Infrastructure.Storage.Products
{
	public sealed class ProductImageStorage : IProductImageStorage
	{
		private readonly string storagePath;
		private readonly string requestPath;

		public ProductImageStorage(IConfiguration configuration, IWebHostEnvironment environment)
		{
			string configuredStoragePath = configuration["ProductImages:StoragePath"];
			storagePath = Path.IsPathRooted(configuredStoragePath) ? configuredStoragePath : Path.GetFullPath(Path.Combine(environment.ContentRootPath, configuredStoragePath));
			requestPath = configuration["ProductImages:RequestPath"].TrimEnd('/');
		}

		public async Task<string> SaveAsync(int idProduct, Stream imageStream, string extension, CancellationToken cancellationToken)
		{
			string productStoragePath = Path.Combine(storagePath, idProduct.ToString());

			if (!Directory.Exists(productStoragePath))
			{
				Directory.CreateDirectory(productStoragePath);
			}

			string fileName = $"{Guid.NewGuid():N}{extension}";
			string filePath = Path.Combine(productStoragePath, fileName);

			await using (FileStream fileStream = new FileStream(filePath, FileMode.CreateNew, FileAccess.Write, FileShare.None, 81920, true))
			{
				await imageStream.CopyToAsync(fileStream, cancellationToken);
			}

			foreach (string previousFilePath in Directory.EnumerateFiles(productStoragePath))
			{
				if (!previousFilePath.Equals(filePath, StringComparison.OrdinalIgnoreCase))
				{
					File.Delete(previousFilePath);
				}
			}

			return $"{requestPath}/{idProduct}/{fileName}";
		}
	}
}
