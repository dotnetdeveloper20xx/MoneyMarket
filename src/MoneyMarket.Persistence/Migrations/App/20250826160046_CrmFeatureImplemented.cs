using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoneyMarket.Persistence.Migrations.App
{
    /// <inheritdoc />
    public partial class CrmFeatureImplemented : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DisabledAtUtc",
                table: "Lenders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DisabledReason",
                table: "Lenders",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Lenders",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsDisabled",
                table: "Lenders",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DisabledAtUtc",
                table: "Borrowers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DisabledReason",
                table: "Borrowers",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDisabled",
                table: "Borrowers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DisabledAtUtc",
                table: "BorrowerProfile",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DisabledReason",
                table: "BorrowerProfile",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDisabled",
                table: "BorrowerProfile",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "LenderProfile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDisabled = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DisabledReason = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    DisabledAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LenderProfile", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LenderProfile");

            migrationBuilder.DropColumn(
                name: "DisabledAtUtc",
                table: "Lenders");

            migrationBuilder.DropColumn(
                name: "DisabledReason",
                table: "Lenders");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Lenders");

            migrationBuilder.DropColumn(
                name: "IsDisabled",
                table: "Lenders");

            migrationBuilder.DropColumn(
                name: "DisabledAtUtc",
                table: "Borrowers");

            migrationBuilder.DropColumn(
                name: "DisabledReason",
                table: "Borrowers");

            migrationBuilder.DropColumn(
                name: "IsDisabled",
                table: "Borrowers");

            migrationBuilder.DropColumn(
                name: "DisabledAtUtc",
                table: "BorrowerProfile");

            migrationBuilder.DropColumn(
                name: "DisabledReason",
                table: "BorrowerProfile");

            migrationBuilder.DropColumn(
                name: "IsDisabled",
                table: "BorrowerProfile");
        }
    }
}
