namespace Uirapuru.Catalog.Api.Domains.Products;

public sealed record class ProductID
{
	public int Value { get; }

	public ProductID(int value)
	{
		if (value <= 0)
		{
			throw new ArgumentOutOfRangeException(
				nameof(value),
				"Product ID must be greater than zero.");
		}

		Value = value;
	}
}
