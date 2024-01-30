using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReceiptVoucher.EF.Migrations
{
    /// <inheritdoc />
    public partial class SubProjectDateUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "Sub_Projects",
                newName: "Date");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Sub_Projects",
                newName: "CreatedDate");
        }
    }
}
