using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReceiptVoucher.EF.Migrations
{
    /// <inheritdoc />
    public partial class AddDataRelatedToTheTypeOfPaymentInReceipt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ReceivedBy",
                table: "Receipts",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AccountNumber",
                table: "Receipts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Bank",
                table: "Receipts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateOnly>(
                name: "CheckDate",
                table: "Receipts",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<int>(
                name: "CheckNumber",
                table: "Receipts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PaymentType",
                table: "Receipts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountNumber",
                table: "Receipts");

            migrationBuilder.DropColumn(
                name: "Bank",
                table: "Receipts");

            migrationBuilder.DropColumn(
                name: "CheckDate",
                table: "Receipts");

            migrationBuilder.DropColumn(
                name: "CheckNumber",
                table: "Receipts");

            migrationBuilder.DropColumn(
                name: "PaymentType",
                table: "Receipts");

            migrationBuilder.AlterColumn<string>(
                name: "ReceivedBy",
                table: "Receipts",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);
        }
    }
}
