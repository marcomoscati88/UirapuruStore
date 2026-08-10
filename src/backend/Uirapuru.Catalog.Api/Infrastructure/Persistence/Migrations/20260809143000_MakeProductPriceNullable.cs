using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Uirapuru.Catalog.Api.Infrastructure.Persistence.Migrations;

[DbContext(typeof(CatalogDbContext))]
[Migration("20260809143000_MakeProductPriceNullable")]
public sealed class MakeProductPriceNullable : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.AlterColumn<decimal>(
			name: "Price",
			schema: "catalog",
			table: "Products",
			type: "numeric(18,2)",
			precision: 18,
			scale: 2,
			nullable: true,
			oldClrType: typeof(decimal),
			oldType: "numeric(18,2)",
			oldPrecision: 18,
			oldScale: 2);
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.Sql(
			"""UPDATE catalog."Products" SET "Price" = 0 WHERE "Price" IS NULL;""");

		migrationBuilder.AlterColumn<decimal>(
			name: "Price",
			schema: "catalog",
			table: "Products",
			type: "numeric(18,2)",
			precision: 18,
			scale: 2,
			nullable: false,
			oldClrType: typeof(decimal),
			oldType: "numeric(18,2)",
			oldPrecision: 18,
			oldScale: 2,
			oldNullable: true);
	}
}
