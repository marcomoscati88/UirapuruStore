using Uirapuru.Catalog.Api.Domains.Categories;

namespace Uirapuru.Catalog.Api.Domains.Products;

public sealed class Product
{
	public ProductID Id { get; set; } = null!;

	public string Name { get; set; } = null!;

	public string Description { get; set; } = null!;

	public decimal Price { get; set; }

	public CategoryID? IdCategory { get; private set; }

	public string SubCategory { get; set; } = null!;

	public DateTime CreatedAtUtc { get; set; }

	public DateTime? LastModifiedAtUtc { get; set; }

	private Product()
	{

	}

	public static Product Create(string name,
		string description,
		decimal price,
		CategoryID? idCategory,
		string subCategory,
		DateTime createdAtUtc,
		DateTime? lastModified
		) 
	{
		var product = new Product
		{
			Name = name,
			Description = description,
			Price = price,
			IdCategory = idCategory,
			CreatedAtUtc = createdAtUtc,
			SubCategory = subCategory,
			LastModifiedAtUtc = lastModified
		};

		return product;
	}
}
