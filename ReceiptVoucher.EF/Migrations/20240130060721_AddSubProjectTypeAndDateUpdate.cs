using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReceiptVoucher.EF.Migrations
{
    /// <inheritdoc />
    public partial class AddSubProjectTypeAndDateUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateOnly>(
                name: "CreatedDate",
                table: "Sub_Projects",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AddColumn<int>(
                name: "SubProjectType",
                table: "Sub_Projects",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubProjectType",
                table: "Sub_Projects");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Sub_Projects",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");
        }
    }
}
