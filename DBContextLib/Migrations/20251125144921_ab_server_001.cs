using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dbcontext.Migrations
{
    /// <inheritdoc />
    public partial class ab_server_001 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Hardwires",
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
                    table.PrimaryKey("PK_Hardwires", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Scripts",
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
                    table.PrimaryKey("PK_Scripts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SystemCommands",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FileName = table.Column<string>(type: "TEXT", nullable: false),
                    Arguments = table.Column<string>(type: "TEXT", nullable: true),
                    IsDisabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemCommands", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    TelegramId = table.Column<long>(type: "INTEGER", nullable: false),
                    IsDisabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    AllowSystemCommands = table.Column<bool>(type: "INTEGER", nullable: false),
                    AllowChangeMqttConfig = table.Column<bool>(type: "INTEGER", nullable: false),
                    ChatId = table.Column<long>(type: "INTEGER", nullable: false),
                    MessageId = table.Column<int>(type: "INTEGER", nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", nullable: true),
                    LastUpdate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastName = table.Column<string>(type: "TEXT", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    AlarmSubscriber = table.Column<bool>(type: "INTEGER", nullable: false),
                    CommandsAllowed = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    HardwareId = table.Column<int>(type: "INTEGER", nullable: false),
                    IsDisable = table.Column<bool>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    PortNum = table.Column<uint>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ports_Hardwires_HardwareId",
                        column: x => x.HardwareId,
                        principalTable: "Hardwires",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Commands",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CommandType = table.Column<int>(type: "INTEGER", nullable: false),
                    ScriptId = table.Column<int>(type: "INTEGER", nullable: false),
                    PauseSecondsBeforeExecution = table.Column<int>(type: "INTEGER", nullable: false),
                    Hidden = table.Column<bool>(type: "INTEGER", nullable: false),
                    Execution = table.Column<int>(type: "INTEGER", nullable: false),
                    ExecutionParameter = table.Column<string>(type: "TEXT", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Sorting = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Commands", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Commands_Scripts_ScriptId",
                        column: x => x.ScriptId,
                        principalTable: "Scripts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Contentions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MasterScriptId = table.Column<int>(type: "INTEGER", nullable: false),
                    SlaveScriptId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contentions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contentions_Scripts_MasterScriptId",
                        column: x => x.MasterScriptId,
                        principalTable: "Scripts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Contentions_Scripts_SlaveScriptId",
                        column: x => x.SlaveScriptId,
                        principalTable: "Scripts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TaskInitiatorType = table.Column<int>(type: "INTEGER", nullable: false),
                    TaskInitiatorId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ScriptId = table.Column<int>(type: "INTEGER", nullable: false),
                    ReportId = table.Column<int>(type: "INTEGER", nullable: true),
                    FinishedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tasks_Scripts_ScriptId",
                        column: x => x.ScriptId,
                        principalTable: "Scripts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Triggers",
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
                    table.PrimaryKey("PK_Triggers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Triggers_Scripts_ScriptId",
                        column: x => x.ScriptId,
                        principalTable: "Scripts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsersForms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FormMapCode = table.Column<string>(type: "TEXT", nullable: false),
                    OwnerUserId = table.Column<int>(type: "INTEGER", nullable: false)
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
                name: "ConditionsCommands",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    HardwareId = table.Column<int>(type: "INTEGER", nullable: false),
                    PortId = table.Column<int>(type: "INTEGER", nullable: false),
                    ConditionValueType = table.Column<int>(type: "INTEGER", nullable: false),
                    ComparisonMode = table.Column<int>(type: "INTEGER", nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: false),
                    OwnerId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConditionsCommands", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConditionsCommands_Commands_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Commands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConditionsCommands_Hardwires_HardwareId",
                        column: x => x.HardwareId,
                        principalTable: "Hardwires",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConditionsCommands_Ports_PortId",
                        column: x => x.PortId,
                        principalTable: "Ports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    OwnerTaskId = table.Column<int>(type: "INTEGER", nullable: false),
                    Success = table.Column<bool>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reports_Tasks_OwnerTaskId",
                        column: x => x.OwnerTaskId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrigersConditions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    HardwareId = table.Column<int>(type: "INTEGER", nullable: false),
                    PortId = table.Column<int>(type: "INTEGER", nullable: false),
                    ConditionValueType = table.Column<int>(type: "INTEGER", nullable: false),
                    ComparisonMode = table.Column<int>(type: "INTEGER", nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: false),
                    OwnerId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrigersConditions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrigersConditions_Hardwires_HardwareId",
                        column: x => x.HardwareId,
                        principalTable: "Hardwires",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrigersConditions_Ports_PortId",
                        column: x => x.PortId,
                        principalTable: "Ports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrigersConditions_Triggers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Triggers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsersFormsProperties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Code = table.Column<string>(type: "TEXT", nullable: false),
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
                name: "IX_Commands_ScriptId",
                table: "Commands",
                column: "ScriptId");

            migrationBuilder.CreateIndex(
                name: "IX_Commands_Sorting",
                table: "Commands",
                column: "Sorting");

            migrationBuilder.CreateIndex(
                name: "IX_ConditionsCommands_HardwareId",
                table: "ConditionsCommands",
                column: "HardwareId");

            migrationBuilder.CreateIndex(
                name: "IX_ConditionsCommands_OwnerId_HardwareId_PortId",
                table: "ConditionsCommands",
                columns: new[] { "OwnerId", "HardwareId", "PortId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ConditionsCommands_PortId",
                table: "ConditionsCommands",
                column: "PortId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentionsMaster",
                table: "Contentions",
                column: "MasterScriptId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentionsSlave",
                table: "Contentions",
                column: "SlaveScriptId");

            migrationBuilder.CreateIndex(
                name: "IX_ScriptJoinLink",
                table: "Contentions",
                columns: new[] { "MasterScriptId", "SlaveScriptId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Hardwires_Address",
                table: "Hardwires",
                column: "Address",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Hardwires_AlarmSubscriber_CommandsAllowed",
                table: "Hardwires",
                columns: new[] { "AlarmSubscriber", "CommandsAllowed" });

            migrationBuilder.CreateIndex(
                name: "IX_Ports_HardwareId",
                table: "Ports",
                column: "HardwareId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_OwnerTaskId",
                table: "Reports",
                column: "OwnerTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_ScriptId",
                table: "Tasks",
                column: "ScriptId");

            migrationBuilder.CreateIndex(
                name: "IX_TrigersConditions_HardwareId",
                table: "TrigersConditions",
                column: "HardwareId");

            migrationBuilder.CreateIndex(
                name: "IX_TrigersConditions_OwnerId_HardwareId_PortId",
                table: "TrigersConditions",
                columns: new[] { "OwnerId", "HardwareId", "PortId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrigersConditions_PortId",
                table: "TrigersConditions",
                column: "PortId");

            migrationBuilder.CreateIndex(
                name: "IX_Triggers_ScriptId",
                table: "Triggers",
                column: "ScriptId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_AlarmSubscriber_CommandsAllowed",
                table: "Users",
                columns: new[] { "AlarmSubscriber", "CommandsAllowed" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_TelegramId",
                table: "Users",
                column: "TelegramId",
                unique: true);

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
                name: "ConditionsCommands");

            migrationBuilder.DropTable(
                name: "Contentions");

            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.DropTable(
                name: "SystemCommands");

            migrationBuilder.DropTable(
                name: "TrigersConditions");

            migrationBuilder.DropTable(
                name: "UsersFormsProperties");

            migrationBuilder.DropTable(
                name: "Commands");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "Ports");

            migrationBuilder.DropTable(
                name: "Triggers");

            migrationBuilder.DropTable(
                name: "UsersForms");

            migrationBuilder.DropTable(
                name: "Hardwires");

            migrationBuilder.DropTable(
                name: "Scripts");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
