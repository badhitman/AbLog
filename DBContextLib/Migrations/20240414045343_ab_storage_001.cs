using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dbcontext.Migrations.Parameters
{
    /// <inheritdoc />
    public partial class ab_storage_001 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDisable",
                table: "PortModelDB",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_CommandModelDB_Sorting",
                table: "CommandModelDB",
                column: "Sorting");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CommandModelDB_Sorting",
                table: "CommandModelDB");

            migrationBuilder.DropColumn(
                name: "IsDisable",
                table: "PortModelDB");
        }
    }
}
