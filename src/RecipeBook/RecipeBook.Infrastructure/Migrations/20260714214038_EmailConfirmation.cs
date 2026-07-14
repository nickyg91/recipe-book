using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipeBook.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EmailConfirmation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "date_of_birth",
                table: "user");

            migrationBuilder.AddColumn<Guid>(
                name: "email_confirmation_token",
                table: "user",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_email_confirmed",
                table: "user",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "email_confirmation_token",
                table: "user");

            migrationBuilder.DropColumn(
                name: "is_email_confirmed",
                table: "user");

            migrationBuilder.AddColumn<DateOnly>(
                name: "date_of_birth",
                table: "user",
                type: "date",
                nullable: true);
        }
    }
}
