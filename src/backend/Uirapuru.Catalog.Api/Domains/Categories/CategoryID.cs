namespace Uirapuru.Catalog.Api.Domains.Categories;

public sealed record class CategoryID
{
	public byte Value { get; }

	public CategoryID(byte value)
	{
		if (value == 0)
		{
			throw new ArgumentOutOfRangeException(
				nameof(value),
				"Category ID must be greater than zero.");
		}

		Value = value;
	}
}
