using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyApiApp.Migrations
{
    /// <inheritdoc />
    public partial class AdjustAccountNotRequireCustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CustomerId1",
                table: "account",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "account",
                keyColumn: "AccountId",
                keyValue: 1,
                column: "CustomerId1",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_account_CustomerId1",
                table: "account",
                column: "CustomerId1");

            migrationBuilder.AddForeignKey(
                name: "FK_account_customer_CustomerId1",
                table: "account",
                column: "CustomerId1",
                principalTable: "customer",
                principalColumn: "CustomerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_account_customer_CustomerId1",
                table: "account");

            migrationBuilder.DropIndex(
                name: "IX_account_CustomerId1",
                table: "account");

            migrationBuilder.DropColumn(
                name: "CustomerId1",
                table: "account");
        }
    }
}
