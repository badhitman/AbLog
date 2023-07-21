using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dbcontext.Migrations.Server
{
    /// <inheritdoc />
    public partial class ab_server_010 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "UsersFormsProperties",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "UsersFormsProperties");
        }
    }
}
