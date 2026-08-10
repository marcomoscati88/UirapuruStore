namespace Uirapuru.Catalog.Api.Application.Products.Storage
{
	public interface IProductImageStorage
	{
		Task<string> SaveAsync(int idProduct, Stream imageStream, string extension, CancellationToken cancellationToken);
	}
}
