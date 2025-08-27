using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoneyMarket.Persistence.Migrations.App
{
    /// <inheritdoc />
    public partial class AddLenderApplicationRowVersionAndBusinessSplit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "LenderApplications",
                type: "rowversion",
                rowVersion: true,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "LenderApplications");
        }
    }
}
