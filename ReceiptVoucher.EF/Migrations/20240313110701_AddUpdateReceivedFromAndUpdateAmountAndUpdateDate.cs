using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReceiptVoucher.EF.Migrations
{
    /// <inheritdoc />
    public partial class AddUpdateReceivedFromAndUpdateAmountAndUpdateDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "UpdateAmount",
                table: "Receipts",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateOnly>(
                name: "UpdateDate",
                table: "Receipts",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdateReceivedFrom",
                table: "Receipts",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdateAmount",
                table: "Receipts");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                table: "Receipts");

            migrationBuilder.DropColumn(
                name: "UpdateReceivedFrom",
                table: "Receipts");
        }
    }
}
