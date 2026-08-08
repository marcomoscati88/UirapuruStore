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

