using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReceiptVoucher.EF.Migrations
{
    /// <inheritdoc />
    public partial class Link_ReceivedBy_WithUserID_As_ForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ReceivedBy",
                table: "Receipts",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Receipts_ReceivedBy",
                table: "Receipts",
                column: "ReceivedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Receipts_AspNetUsers_ReceivedBy",
                table: "Receipts",
                column: "ReceivedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Receipts_AspNetUsers_ReceivedBy",
                table: "Receipts");

            migrationBuilder.DropIndex(
                name: "IX_Receipts_ReceivedBy",
                table: "Receipts");

            migrationBuilder.AlterColumn<string>(
                name: "ReceivedBy",
                table: "Receipts",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
