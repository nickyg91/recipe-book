using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipeBook.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRecipeBookEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "image",
                table: "recipe",
                type: "bytea",
                maxLength: 5242880,
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "bytes",
                oldMaxLength: 5242880,
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "image",
                table: "recipe",
                type: "bytes",
                maxLength: 5242880,
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "bytea",
                oldMaxLength: 5242880,
                oldNullable: true);
        }
    }
}
