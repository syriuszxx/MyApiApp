using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyApiApp.Migrations
{
    /// <inheritdoc />
    public partial class AdjustAccountNotRequireCustomer2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AccountId1",
                table: "transaction",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_transaction_AccountId1",
                table: "transaction",
                column: "AccountId1");

            migrationBuilder.AddForeignKey(
                name: "FK_transaction_account_AccountId1",
                table: "transaction",
                column: "AccountId1",
                principalTable: "account",
                principalColumn: "AccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_transaction_account_AccountId1",
                table: "transaction");

            migrationBuilder.DropIndex(
                name: "IX_transaction_AccountId1",
                table: "transaction");

            migrationBuilder.DropColumn(
                name: "AccountId1",
                table: "transaction");
        }
    }
}
