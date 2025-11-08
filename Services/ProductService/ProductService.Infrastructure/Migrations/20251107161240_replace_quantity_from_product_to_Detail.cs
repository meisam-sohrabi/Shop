using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class replace_quantity_from_product_to_Detail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                schema: "Product",
                table: "Products");

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                schema: "Product",
                table: "Details",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                schema: "Product",
                table: "Details");

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                schema: "Product",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
