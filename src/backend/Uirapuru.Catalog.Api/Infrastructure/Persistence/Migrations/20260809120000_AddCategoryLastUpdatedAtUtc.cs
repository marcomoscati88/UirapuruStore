using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Uirapuru.Catalog.Api.Infrastructure.Persistence.Migrations;

[DbContext(typeof(CatalogDbContext))]
[Migration("20260809120000_AddCategoryLastUpdatedAtUtc")]
public sealed class AddCategoryLastUpdatedAtUtc : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.AddColumn<DateTimeOffset>(
			name: "LastUpdatedAtUtc",
			schema: "catalog",
			table: "Categories",
			type: "timestamp with time zone",
			nullable: true);
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropColumn(
			name: "LastUpdatedAtUtc",
			schema: "catalog",
			table: "Categories");
	}
}
