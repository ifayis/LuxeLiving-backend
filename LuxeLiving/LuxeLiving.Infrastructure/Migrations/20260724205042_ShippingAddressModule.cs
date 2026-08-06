using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FurnitureShop.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ShippingAddressModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WishlistItems_WishlistId",
                table: "WishlistItems");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Wishlists",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Wishlists",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "WishlistItems",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "WishlistItems",
                type: "rowversion",
                rowVersion: true,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Wishlists_UserId",
                table: "Wishlists",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WishlistItems_WishlistId_ProductId",
                table: "WishlistItems",
                columns: new[] { "WishlistId", "ProductId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Wishlists_UserId",
                table: "Wishlists");

            migrationBuilder.DropIndex(
                name: "IX_WishlistItems_WishlistId_ProductId",
                table: "WishlistItems");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Wishlists");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Wishlists");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "WishlistItems");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "WishlistItems");

            migrationBuilder.CreateIndex(
                name: "IX_WishlistItems_WishlistId",
                table: "WishlistItems",
                column: "WishlistId");
        }
    }
}
