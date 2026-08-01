using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Uirapuru.Catalog.Api.Infrastructure.Persistence.Migrations;

[DbContext(typeof(CatalogDbContext))]
[Migration("20260801120000_AddCategories")]
public sealed class AddCategories : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.AlterColumn<byte>(
			name: "IdCategory",
			schema: "catalog",
			table: "Products",
			type: "smallint",
			nullable: true,
			oldClrType: typeof(int),
			oldType: "integer");

		migrationBuilder.CreateTable(
			name: "Categories",
			schema: "catalog",
			columns: table => new
			{
				Id = table.Column<byte>(
						type: "smallint",
						nullable: false)
					.Annotation(
						"Npgsql:ValueGenerationStrategy",
						NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
				Name = table.Column<string>(
					type: "character varying(255)",
					maxLength: 255,
					nullable: false),
				CreatedAtUtc = table.Column<DateTimeOffset>(
					type: "timestamp with time zone",
					nullable: false)
			},
			constraints: table =>
			{
				table.PrimaryKey("PK_Categories", x => x.Id);
			});

		migrationBuilder.CreateIndex(
			name: "IX_Products_IdCategory",
			schema: "catalog",
			table: "Products",
			column: "IdCategory");

		// The new Categories table is initially empty, so legacy identifiers
		// cannot be preserved as valid foreign keys.
		migrationBuilder.Sql(
			"""UPDATE catalog."Products" SET "IdCategory" = NULL;""");

		migrationBuilder.AddForeignKey(
			name: "FK_Products_Categories_IdCategory",
			schema: "catalog",
			table: "Products",
			column: "IdCategory",
			principalSchema: "catalog",
			principalTable: "Categories",
			principalColumn: "Id",
			onDelete: ReferentialAction.SetNull);
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropForeignKey(
			name: "FK_Products_Categories_IdCategory",
			schema: "catalog",
			table: "Products");

		migrationBuilder.DropTable(
			name: "Categories",
			schema: "catalog");

		migrationBuilder.DropIndex(
			name: "IX_Products_IdCategory",
			schema: "catalog",
			table: "Products");

		migrationBuilder.Sql(
			"""UPDATE catalog."Products" SET "IdCategory" = 0 WHERE "IdCategory" IS NULL;""");

		migrationBuilder.AlterColumn<int>(
			name: "IdCategory",
			schema: "catalog",
			table: "Products",
			type: "integer",
			nullable: false,
			oldClrType: typeof(byte),
			oldType: "smallint",
			oldNullable: true);
	}
}
