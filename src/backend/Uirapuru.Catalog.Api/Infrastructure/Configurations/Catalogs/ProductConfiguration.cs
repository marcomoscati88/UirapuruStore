using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Uirapuru.Catalog.Api.Domains.Categories;
using Uirapuru.Catalog.Api.Domains.Products;

namespace Uirapuru.Catalog.Api.Infrastructure.Configurations.Catalogs
{
	public sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
	{
		public void Configure(EntityTypeBuilder<Product> builder)
		{
			builder.HasKey(x => x.Id);

			builder.Property(x => x.Id)
				.HasConversion(
					id => id.Value,
					value => new ProductID(value))
				.ValueGeneratedOnAdd();

			builder.Property(x => x.IdCategory)
				.HasConversion(
					id => id!.Value,
					value => new CategoryID(value))
				.IsRequired(false);

			builder.HasOne<Category>()
				.WithMany(x => x.Products)
				.HasForeignKey(x => x.IdCategory)
				.OnDelete(DeleteBehavior.SetNull);

			builder.Property(x => x.Price)
				.HasPrecision(18, 2);
		}
	}
}
