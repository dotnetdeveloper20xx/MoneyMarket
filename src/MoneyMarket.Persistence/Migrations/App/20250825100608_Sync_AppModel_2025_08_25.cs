using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoneyMarket.Persistence.Migrations.App
{
    /// <inheritdoc />
    public partial class Sync_AppModel_2025_08_25 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Fundings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LenderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fundings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Fundings_Lenders_LenderId",
                        column: x => x.LenderId,
                        principalTable: "Lenders",
                        principalColumn: "LenderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Fundings_Loans_LoanId",
                        column: x => x.LoanId,
                        principalTable: "Loans",
                        principalColumn: "LoanId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Fundings_LenderId",
                table: "Fundings",
                column: "LenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Fundings_LoanId",
                table: "Fundings",
                column: "LoanId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Fundings");
        }
    }
}
