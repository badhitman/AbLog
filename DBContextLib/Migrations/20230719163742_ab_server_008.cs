using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dbcontext.Migrations.Server
{
    /// <inheritdoc />
    public partial class ab_server_008 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AllowChangeMqttConfig",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "ChatId",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<int>(
                name: "MessageId",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "UsersForms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OwnerUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersForms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsersForms_Users_OwnerUserId",
                        column: x => x.OwnerUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsersFormsProperties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OwnerFormId = table.Column<int>(type: "INTEGER", nullable: false),
                    PropValue = table.Column<string>(type: "TEXT", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersFormsProperties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsersFormsProperties_UsersForms_OwnerFormId",
                        column: x => x.OwnerFormId,
                        principalTable: "UsersForms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UsersForms_OwnerUserId",
                table: "UsersForms",
                column: "OwnerUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UsersFormsProperties_OwnerFormId",
                table: "UsersFormsProperties",
                column: "OwnerFormId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsersFormsProperties");

            migrationBuilder.DropTable(
                name: "UsersForms");

            migrationBuilder.DropColumn(
                name: "AllowChangeMqttConfig",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ChatId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "MessageId",
                table: "Users");
        }
    }
}
