using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoneyMarket.Persistence.Migrations.App
{
    /// <inheritdoc />
    public partial class RegisterLenderAndLenderProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LenderApplications",
                columns: table => new
                {
                    LenderApplicationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BusinessRegistration_BusinessName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BusinessRegistration_RegistrationNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BusinessRegistration_ProofOfIncorporationDocuments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BusinessRegistration_LendingLicenses = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BusinessRegistration_ComplianceStatement = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FinancialCapacity_FundingSourceType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FinancialCapacity_FundingSourceDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FinancialCapacity_CapitalReserveDocuments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RiskManagement_UnderwritingPolicy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RiskManagement_RiskAssessmentTools = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RiskManagement_PaymentCollectionProcess = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RiskManagement_CommunicationPlan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RiskManagement_DefaultHandlingStrategy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RiskManagement_PricingStrategy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LenderApplications", x => x.LenderApplicationId);
                });

            migrationBuilder.CreateTable(
                name: "LenderProducts",
                columns: table => new
                {
                    LenderProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LenderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TermType = table.Column<int>(type: "int", nullable: false),
                    MinAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    MaxAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TermMonths = table.Column<int>(type: "int", nullable: false),
                    Instalments = table.Column<int>(type: "int", nullable: false),
                    InterestRate = table.Column<decimal>(type: "decimal(6,4)", precision: 6, scale: 4, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LenderProducts", x => x.LenderProductId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LenderProducts_LenderId_IsActive",
                table: "LenderProducts",
                columns: new[] { "LenderId", "IsActive" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LenderApplications");

            migrationBuilder.DropTable(
                name: "LenderProducts");
        }
    }
}
