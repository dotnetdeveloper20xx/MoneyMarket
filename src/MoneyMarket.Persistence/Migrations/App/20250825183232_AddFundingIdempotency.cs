using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoneyMarket.Persistence.Migrations.App
{
    /// <inheritdoc />
    public partial class AddFundingIdempotency : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RepaymentInstallment_Loans_LoanId",
                table: "RepaymentInstallment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RepaymentInstallment",
                table: "RepaymentInstallment");

            migrationBuilder.RenameTable(
                name: "RepaymentInstallment",
                newName: "RepaymentInstallments");

            migrationBuilder.RenameIndex(
                name: "IX_RepaymentInstallment_LoanId",
                table: "RepaymentInstallments",
                newName: "IX_RepaymentInstallments_LoanId");

            migrationBuilder.AddColumn<string>(
                name: "IdempotencyKey",
                table: "Fundings",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RepaymentInstallments",
                table: "RepaymentInstallments",
                column: "InstallmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Fundings_IdempotencyKey",
                table: "Fundings",
                column: "IdempotencyKey",
                unique: true,
                filter: "[IdempotencyKey] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_RepaymentInstallments_Loans_LoanId",
                table: "RepaymentInstallments",
                column: "LoanId",
                principalTable: "Loans",
                principalColumn: "LoanId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RepaymentInstallments_Loans_LoanId",
                table: "RepaymentInstallments");

            migrationBuilder.DropIndex(
                name: "IX_Fundings_IdempotencyKey",
                table: "Fundings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RepaymentInstallments",
                table: "RepaymentInstallments");

            migrationBuilder.DropColumn(
                name: "IdempotencyKey",
                table: "Fundings");

            migrationBuilder.RenameTable(
                name: "RepaymentInstallments",
                newName: "RepaymentInstallment");

            migrationBuilder.RenameIndex(
                name: "IX_RepaymentInstallments_LoanId",
                table: "RepaymentInstallment",
                newName: "IX_RepaymentInstallment_LoanId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RepaymentInstallment",
                table: "RepaymentInstallment",
                column: "InstallmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_RepaymentInstallment_Loans_LoanId",
                table: "RepaymentInstallment",
                column: "LoanId",
                principalTable: "Loans",
                principalColumn: "LoanId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
