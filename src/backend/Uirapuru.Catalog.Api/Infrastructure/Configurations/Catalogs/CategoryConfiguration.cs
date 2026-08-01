using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Uirapuru.Catalog.Api.Domains.Categories;

namespace Uirapuru.Catalog.Api.Infrastructure.Configurations.Catalogs;

public sealed class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
	public void Configure(EntityTypeBuilder<Category> builder)
	{
		builder.HasKey(x => x.Id);

		builder.Property(x => x.Id)
			.HasConversion(
				id => id.Value,
				value => new CategoryID(value))
			.ValueGeneratedOnAdd();

		builder.Property(x => x.Name)
			.HasMaxLength(255)
			.IsRequired();

		builder.Property(x => x.CreatedAtUtc)
			.IsRequired();

		builder.Navigation(x => x.Products)
			.HasField("_products")
			.UsePropertyAccessMode(PropertyAccessMode.Field);
	}
}
