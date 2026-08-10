using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Uirapuru.Catalog.Api.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoryAndProductIsEnabled : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsEnabled",
                schema: "catalog",
                table: "Products",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsEnabled",
                schema: "catalog",
                table: "Categories",
                type: "boolean",
                nullable: false,
                defaultValue: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEnabled",
                schema: "catalog",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IsEnabled",
                schema: "catalog",
                table: "Categories");
        }
    }
}
