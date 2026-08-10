using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Uirapuru.Catalog.Api.Infrastructure.Persistence.Migrations;

[DbContext(typeof(CatalogDbContext))]
[Migration("20260809153000_EnforceCategoryAndProductConstraints")]
public sealed class EnforceCategoryAndProductConstraints : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.AlterColumn<string>(
			name: "Name",
			schema: "catalog",
			table: "Products",
			type: "character varying(255)",
			maxLength: 255,
			nullable: false,
			oldClrType: typeof(string),
			oldType: "text");

		migrationBuilder.AlterColumn<string>(
			name: "Description",
			schema: "catalog",
			table: "Products",
			type: "character varying(500)",
			maxLength: 500,
			nullable: true,
			oldClrType: typeof(string),
			oldType: "text");

		migrationBuilder.AddCheckConstraint(
			name: "CK_Products_Name_NotWhiteSpace",
			schema: "catalog",
			table: "Products",
			sql: """btrim("Name") <> ''""");

		migrationBuilder.AddCheckConstraint(
			name: "CK_Categories_Name_NotWhiteSpace",
			schema: "catalog",
			table: "Categories",
			sql: """btrim("Name") <> ''""");

		migrationBuilder.CreateIndex(
			name: "IX_Categories_Name",
			schema: "catalog",
			table: "Categories",
			column: "Name",
			unique: true);
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropCheckConstraint(
			name: "CK_Products_Name_NotWhiteSpace",
			schema: "catalog",
			table: "Products");

		migrationBuilder.DropCheckConstraint(
			name: "CK_Categories_Name_NotWhiteSpace",
			schema: "catalog",
			table: "Categories");

		migrationBuilder.DropIndex(
			name: "IX_Categories_Name",
			schema: "catalog",
			table: "Categories");

		migrationBuilder.AlterColumn<string>(
			name: "Name",
			schema: "catalog",
			table: "Products",
			type: "text",
			nullable: false,
			oldClrType: typeof(string),
			oldType: "character varying(255)",
			oldMaxLength: 255);

		migrationBuilder.AlterColumn<string>(
			name: "Description",
			schema: "catalog",
			table: "Products",
			type: "text",
			nullable: false,
			defaultValue: "",
			oldClrType: typeof(string),
			oldType: "character varying(500)",
			oldMaxLength: 500,
			oldNullable: true);
	}
}
