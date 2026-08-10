using Uirapuru.Catalog.Api.Domains.Products;

namespace Uirapuru.Catalog.Api.Domains.Categories;

public sealed class Category
{
	public CategoryID Id { get; private set; }

	public string Name { get; private set; }

	public DateTimeOffset CreatedAtUtc { get; private set; }
	
	public DateTimeOffset? LastUpdatedAtUtc { get; private set; }

	public bool IsEnabled { get; set; }


	public IReadOnlyCollection<Product> Products => _products.AsReadOnly();

	private readonly List<Product> _products = new List<Product>();

	private Category()
	{
	}

	public static Category Create(string name, DateTimeOffset createdNow)
	{
		Category newCategory = new Category() {
			Name = name,
			CreatedAtUtc = createdNow,
			IsEnabled = true
		};

		return newCategory;
	}

	public void Update(string name, DateTimeOffset updateNow) 
	{
		Name = name;
		LastUpdatedAtUtc = updateNow;
	}

	public void Enable()
	{
		this.IsEnabled = true;
	}

	public void Disable()
	{
		this.IsEnabled = false;
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

