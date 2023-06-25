using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dbcontext.Migrations
{
    /// <inheritdoc />
    public partial class ab_storage_init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HardwareModelDB",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    AlarmSubscriber = table.Column<bool>(type: "INTEGER", nullable: false),
                    CommandsAllowed = table.Column<bool>(type: "INTEGER", nullable: false),
                    Address = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HardwareModelDB", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ParametersStorage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StoredValue = table.Column<string>(type: "TEXT", nullable: false),
                    TypeName = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParametersStorage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ScriptModelDB",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IsEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScriptModelDB", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PortModelDB",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    HardwareId = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    PortNum = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PortModelDB", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PortModelDB_HardwareModelDB_HardwareId",
                        column: x => x.HardwareId,
                        principalTable: "HardwareModelDB",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CommandModelDB",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CommandType = table.Column<int>(type: "INTEGER", nullable: false),
                    ScriptId = table.Column<int>(type: "INTEGER", nullable: false),
                    PauseSecondsBeforeExecution = table.Column<int>(type: "INTEGER", nullable: false),
                    Hidden = table.Column<bool>(type: "INTEGER", nullable: false),
                    Execution = table.Column<int>(type: "INTEGER", nullable: false),
                    ExecutionParametr = table.Column<string>(type: "TEXT", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Sorting = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommandModelDB", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommandModelDB_ScriptModelDB_ScriptId",
                        column: x => x.ScriptId,
                        principalTable: "ScriptModelDB",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContentionsModelDB",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MasterScriptId = table.Column<int>(type: "INTEGER", nullable: false),
                    SlaveScriptId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentionsModelDB", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContentionsModelDB_ScriptModelDB_MasterScriptId",
                        column: x => x.MasterScriptId,
                        principalTable: "ScriptModelDB",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContentionsModelDB_ScriptModelDB_SlaveScriptId",
                        column: x => x.SlaveScriptId,
                        principalTable: "ScriptModelDB",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrigerModelDB",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ScriptId = table.Column<int>(type: "INTEGER", nullable: false),
                    IsDisable = table.Column<bool>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrigerModelDB", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrigerModelDB_ScriptModelDB_ScriptId",
                        column: x => x.ScriptId,
                        principalTable: "ScriptModelDB",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CommandConditionModelDB",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    HardwareId = table.Column<int>(type: "INTEGER", nullable: false),
                    PortId = table.Column<int>(type: "INTEGER", nullable: false),
                    ConditionValueType = table.Column<int>(type: "INTEGER", nullable: false),
                    СomparisonMode = table.Column<int>(type: "INTEGER", nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: false),
                    OwnerId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommandConditionModelDB", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommandConditionModelDB_CommandModelDB_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "CommandModelDB",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommandConditionModelDB_HardwareModelDB_HardwareId",
                        column: x => x.HardwareId,
                        principalTable: "HardwareModelDB",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommandConditionModelDB_PortModelDB_PortId",
                        column: x => x.PortId,
                        principalTable: "PortModelDB",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrigerConditionModelDB",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    HardwareId = table.Column<int>(type: "INTEGER", nullable: false),
                    PortId = table.Column<int>(type: "INTEGER", nullable: false),
                    ConditionValueType = table.Column<int>(type: "INTEGER", nullable: false),
                    СomparisonMode = table.Column<int>(type: "INTEGER", nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: false),
                    OwnerId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrigerConditionModelDB", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrigerConditionModelDB_HardwareModelDB_HardwareId",
                        column: x => x.HardwareId,
                        principalTable: "HardwareModelDB",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrigerConditionModelDB_PortModelDB_PortId",
                        column: x => x.PortId,
                        principalTable: "PortModelDB",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrigerConditionModelDB_TrigerModelDB_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "TrigerModelDB",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommandConditionModelDB_HardwareId",
                table: "CommandConditionModelDB",
                column: "HardwareId");

            migrationBuilder.CreateIndex(
                name: "IX_CommandConditionModelDB_OwnerId_HardwareId_PortId",
                table: "CommandConditionModelDB",
                columns: new[] { "OwnerId", "HardwareId", "PortId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CommandConditionModelDB_PortId",
                table: "CommandConditionModelDB",
                column: "PortId");

            migrationBuilder.CreateIndex(
                name: "IX_CommandModelDB_ScriptId",
                table: "CommandModelDB",
                column: "ScriptId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentionsMaster",
                table: "ContentionsModelDB",
                column: "MasterScriptId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentionsSlave",
                table: "ContentionsModelDB",
                column: "SlaveScriptId");

            migrationBuilder.CreateIndex(
                name: "IX_ScriptJoinLink",
                table: "ContentionsModelDB",
                columns: new[] { "MasterScriptId", "SlaveScriptId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HardwareModelDB_Address",
                table: "HardwareModelDB",
                column: "Address",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HardwareModelDB_AlarmSubscriber_CommandsAllowed",
                table: "HardwareModelDB",
                columns: new[] { "AlarmSubscriber", "CommandsAllowed" });

            migrationBuilder.CreateIndex(
                name: "IX_PortModelDB_HardwareId",
                table: "PortModelDB",
                column: "HardwareId");

            migrationBuilder.CreateIndex(
                name: "IX_TrigerConditionModelDB_HardwareId",
                table: "TrigerConditionModelDB",
                column: "HardwareId");

            migrationBuilder.CreateIndex(
                name: "IX_TrigerConditionModelDB_OwnerId_HardwareId_PortId",
                table: "TrigerConditionModelDB",
                columns: new[] { "OwnerId", "HardwareId", "PortId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrigerConditionModelDB_PortId",
                table: "TrigerConditionModelDB",
                column: "PortId");

            migrationBuilder.CreateIndex(
                name: "IX_TrigerModelDB_ScriptId",
                table: "TrigerModelDB",
                column: "ScriptId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommandConditionModelDB");

            migrationBuilder.DropTable(
                name: "ContentionsModelDB");

            migrationBuilder.DropTable(
                name: "ParametersStorage");

            migrationBuilder.DropTable(
                name: "TrigerConditionModelDB");

            migrationBuilder.DropTable(
                name: "CommandModelDB");

            migrationBuilder.DropTable(
                name: "PortModelDB");

            migrationBuilder.DropTable(
                name: "TrigerModelDB");

            migrationBuilder.DropTable(
                name: "HardwareModelDB");

            migrationBuilder.DropTable(
                name: "ScriptModelDB");
        }
    }
}
