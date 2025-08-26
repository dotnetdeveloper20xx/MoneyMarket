using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoneyMarket.Persistence.Migrations.App
{
    /// <inheritdoc />
    public partial class AddBorrowerfullregistrationAndBorroweprofile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BorrowerProfile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    NationalIdNumber = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    CurrentAddress_House = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    CurrentAddress_Street = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    CurrentAddress_City = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    CurrentAddress_Country = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    CurrentAddress_PostCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Employment_EmployerName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Employment_JobTitle = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Employment_LengthOfEmployment = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Employment_GrossAnnualIncome = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Employment_AdditionalIncomeSources = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BorrowerProfile", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BorrowerDebts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LenderName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DebtType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BorrowerProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BorrowerDebts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BorrowerDebts_BorrowerProfile_BorrowerProfileId",
                        column: x => x.BorrowerProfileId,
                        principalTable: "BorrowerProfile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BorrowerDebts_BorrowerProfileId",
                table: "BorrowerDebts",
                column: "BorrowerProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_BorrowerProfile_UserId",
                table: "BorrowerProfile",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BorrowerDebts");

            migrationBuilder.DropTable(
                name: "BorrowerProfile");
        }
    }
}
