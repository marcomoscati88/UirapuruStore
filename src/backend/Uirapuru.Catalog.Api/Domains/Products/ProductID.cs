namespace Uirapuru.Catalog.Api.Domains.Products;

public sealed record class ProductID
{
	public int Value { get; }

	public ProductID(int value) : this(value, true)
	{
	}

	private ProductID(int value, bool validate)
	{
		if (validate && value <= 0)
		{
			throw new ArgumentOutOfRangeException(
				nameof(value),
				"Product ID must be greater than zero.");
		}

		Value = value;
	}

	internal static ProductID FromPersistence(int value)
	{
		return new ProductID(value, false);
	}
}
