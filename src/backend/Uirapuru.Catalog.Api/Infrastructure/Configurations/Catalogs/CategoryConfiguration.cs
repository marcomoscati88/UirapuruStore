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

		builder.HasIndex(x => x.Name)
			.IsUnique();

		builder.ToTable(tableBuilder => tableBuilder.HasCheckConstraint(
			"CK_Categories_Name_NotWhiteSpace",
			"""btrim("Name") <> ''"""));

		builder.Property(x => x.CreatedAtUtc)
			.IsRequired();

		builder.Property(x => x.LastUpdatedAtUtc)
			.IsRequired(false);

		builder.Property(x => x.IsEnabled)
			.HasDefaultValue(true)
			.IsRequired();

		builder.Navigation(x => x.Products)
			.HasField("_products")
			.UsePropertyAccessMode(PropertyAccessMode.Field);
	}
}
