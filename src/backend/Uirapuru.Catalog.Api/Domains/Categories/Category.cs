using Uirapuru.Catalog.Api.Domains.Products;

namespace Uirapuru.Catalog.Api.Domains.Categories;

public sealed class Category
{
	public CategoryID Id { get; private set; } = null!;

	public string Name { get; private set; } = null!;

	public DateTimeOffset CreatedAtUtc { get; private set; }

	public IReadOnlyCollection<Product> Products => _products.AsReadOnly();

	private readonly List<Product> _products = new List<Product>();

	private Category()
	{
	}

	public static Category Create(string name, DateTimeOffset createdNow)
	{
		Category newCategory = new Category() {
			Name = name,
			CreatedAtUtc = createdNow
		};

		return newCategory;
		//if (string.IsNullOrWhiteSpace(name))
		//{
		//	throw new ArgumentException(
		//		"Nome categoria obbligatorio.",
		//		nameof(name));
		//}

		//if (name.Length > 255)
		//{
		//	throw new ArgumentOutOfRangeException(
		//		nameof(name),
		//		"Il nome della categoria non può eccedere i 255 caratteri.");
		//}

		//return new Category
		//{
		//	Name = name.Trim(),
		//	CreatedAtUtc = DateTimeOffset.UtcNow
		//};
	}

	public void AddProduct(Product product)
	{
		ArgumentNullException.ThrowIfNull(product);

		if (!_products.Contains(product))
		{
			_products.Add(product);
		}
	}

	public bool RemoveProduct(Product product)
	{
		ArgumentNullException.ThrowIfNull(product);

		return _products.Remove(product);
	}
}

