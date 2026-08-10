using Uirapuru.Catalog.Api.Domains.Categories;

namespace Uirapuru.Catalog.Api.Domains.Products;

public sealed class Product
{
	public ProductID Id { get; set; } = null!;

	public string Name { get; set; } = null!;

	public string Description { get; set; }

	public decimal? Price { get; set; }

	public CategoryID IdCategory { get; private set; }

	public string SubCategory { get; set; }

	public DateTime CreatedAtUtc { get; set; }

	public DateTime? LastModifiedAtUtc { get; set; }
	public bool IsEnabled { get; set; }

	public string ImagePath { get; private set; }

	private Product()
	{

	}

	public static Product Create(string name,
		string description,
		decimal? price,
		CategoryID idCategory,
		string subCategory,
		DateTime createdAtUtc,
		DateTime? lastModified
		) 
	{
		var product = new Product
		{
			Name = name,
			Description = description,
			Price = TruncatePrice(price),
			IdCategory = idCategory,
			CreatedAtUtc = createdAtUtc,
			SubCategory = subCategory,
			LastModifiedAtUtc = lastModified,
			IsEnabled = true
		};

		return product;
	}

	public void Update(string description,
		decimal? price,
		CategoryID idCategory,
		string subCategory,
		DateTime lastModified)
	{
		Description = description;
		Price = TruncatePrice(price);
		IdCategory = idCategory;
		SubCategory = subCategory;
		LastModifiedAtUtc = lastModified;
	}

	public void Enable() 
	{
		this.IsEnabled = true;
	}

	public void Disable()
	{
		this.IsEnabled = false;
	}

	public void UpdateImage(string imagePath)
	{
		ImagePath = imagePath;
	}

	private static decimal? TruncatePrice(decimal? price)
	{
		if (price == null)
		{
			return null;
		}

		return decimal.Truncate(price.Value * 100) / 100;
	}
}
