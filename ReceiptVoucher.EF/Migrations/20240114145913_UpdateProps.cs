using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReceiptVoucher.EF.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "Sub_Projects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Age",
                table: "Receipts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "Receipts",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GrantDestinations",
                table: "Receipts",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Mobile",
                table: "Receipts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Area",
                table: "Branches",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Branches",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Mobile",
                table: "Branches",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResponsiblePerson",
                table: "Branches",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Note",
                table: "Sub_Projects");

            migrationBuilder.DropColumn(
                name: "Age",
                table: "Receipts");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Receipts");

            migrationBuilder.DropColumn(
                name: "GrantDestinations",
                table: "Receipts");

            migrationBuilder.DropColumn(
                name: "Mobile",
                table: "Receipts");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Area",
                table: "Branches");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Branches");

            migrationBuilder.DropColumn(
                name: "Mobile",
                table: "Branches");

            migrationBuilder.DropColumn(
                name: "ResponsiblePerson",
                table: "Branches");
        }
    }
}
