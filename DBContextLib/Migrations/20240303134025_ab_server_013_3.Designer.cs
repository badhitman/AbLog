﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ab.context;

#nullable disable

namespace dbcontext.Migrations.Server
{
    [DbContext(typeof(ServerContext))]
    [Migration("20240303134025_ab_server_013_3")]
    partial class ab_server_013_3
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.2");

            modelBuilder.Entity("SharedLib.CommandConditionModelDB", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ConditionValueType")
                        .HasColumnType("INTEGER");

                    b.Property<int>("HardwareId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("OwnerId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PortId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("СomparisonMode")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("HardwareId");

                    b.HasIndex("PortId");

                    b.HasIndex("OwnerId", "HardwareId", "PortId")
                        .IsUnique();

                    b.ToTable("ConditionsCommands");
                });

            modelBuilder.Entity("SharedLib.CommandModelDB", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CommandType")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Execution")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ExecutionParametr")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Hidden")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("PauseSecondsBeforeExecution")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ScriptId")
                        .HasColumnType("INTEGER");

                    b.Property<double>("Sorting")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.HasIndex("ScriptId");

                    b.ToTable("Commands");
                });

            modelBuilder.Entity("SharedLib.ContentionsModelDB", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("MasterScriptId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SlaveScriptId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "MasterScriptId" }, "IX_ContentionsMaster");

                    b.HasIndex(new[] { "SlaveScriptId" }, "IX_ContentionsSlave");

                    b.HasIndex(new[] { "MasterScriptId", "SlaveScriptId" }, "IX_ScriptJoinLink")
                        .IsUnique();

                    b.ToTable("Contentions");
                });

            modelBuilder.Entity("SharedLib.HardwareModelDB", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("AlarmSubscriber")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("CommandsAllowed")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Address")
                        .IsUnique();

                    b.HasIndex("AlarmSubscriber", "CommandsAllowed");

                    b.ToTable("Hardwares");
                });

            modelBuilder.Entity("SharedLib.PortModelDB", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("HardwareId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsDisable")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<uint>("PortNum")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("HardwareId");

                    b.ToTable("Ports");
                });

            modelBuilder.Entity("SharedLib.ReportModelDB", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("OwnerTaskId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Success")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("OwnerTaskId");

                    b.ToTable("Reports");
                });

            modelBuilder.Entity("SharedLib.ScriptModelDB", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Scripts");
                });

            modelBuilder.Entity("SharedLib.SystemCommandModelDB", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Arguments")
                        .HasColumnType("TEXT");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsDisabled")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("SystemCommands");
                });

            modelBuilder.Entity("SharedLib.TaskModelDB", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("FinishedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("ReportId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ScriptId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TaskInitiatorId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TaskInitiatorType")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ScriptId");

                    b.ToTable("Tasks");
                });

            modelBuilder.Entity("SharedLib.TrigerConditionModelDB", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ConditionValueType")
                        .HasColumnType("INTEGER");

                    b.Property<int>("HardwareId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("OwnerId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PortId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("СomparisonMode")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("HardwareId");

                    b.HasIndex("PortId");

                    b.HasIndex("OwnerId", "HardwareId", "PortId")
                        .IsUnique();

                    b.ToTable("TrigersConditions");
                });

            modelBuilder.Entity("SharedLib.TrigerModelDB", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsDisable")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("ScriptId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ScriptId");

                    b.ToTable("Trigers");
                });

            modelBuilder.Entity("SharedLib.UserFormModelDb", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("FormMapCode")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("OwnerUserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("OwnerUserId")
                        .IsUnique();

                    b.ToTable("UsersForms");
                });

            modelBuilder.Entity("SharedLib.UserFormPropertyModelDb", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("OwnerFormId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("PropValue")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("OwnerFormId");

                    b.ToTable("UsersFormsProperties");
                });

            modelBuilder.Entity("SharedLib.UserModelDB", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("AlarmSubscriber")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("AllowChangeMqttConfig")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("AllowSystemCommands")
                        .HasColumnType("INTEGER");

                    b.Property<long>("ChatId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("CommandsAllowed")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsDisabled")
                        .HasColumnType("INTEGER");

                    b.Property<string>("LastName")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("LastUpdate")
                        .HasColumnType("TEXT");

                    b.Property<int>("MessageId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<long>("TelegramId")
                        .HasColumnType("INTEGER");

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("BLOB");

                    b.HasKey("Id");

                    b.HasIndex("TelegramId")
                        .IsUnique();

                    b.HasIndex("AlarmSubscriber", "CommandsAllowed");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("SharedLib.CommandConditionModelDB", b =>
                {
                    b.HasOne("SharedLib.HardwareModelDB", "Hardware")
                        .WithMany()
                        .HasForeignKey("HardwareId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SharedLib.CommandModelDB", "Command")
                        .WithMany("Conditions")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SharedLib.PortModelDB", "Port")
                        .WithMany()
                        .HasForeignKey("PortId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Command");

                    b.Navigation("Hardware");

                    b.Navigation("Port");
                });

            modelBuilder.Entity("SharedLib.CommandModelDB", b =>
                {
                    b.HasOne("SharedLib.ScriptModelDB", "Script")
                        .WithMany("Commands")
                        .HasForeignKey("ScriptId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Script");
                });

            modelBuilder.Entity("SharedLib.ContentionsModelDB", b =>
                {
                    b.HasOne("SharedLib.ScriptModelDB", "MasterScript")
                        .WithMany("Contentions")
                        .HasForeignKey("MasterScriptId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SharedLib.ScriptModelDB", "SlaveScript")
                        .WithMany()
                        .HasForeignKey("SlaveScriptId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MasterScript");

                    b.Navigation("SlaveScript");
                });

            modelBuilder.Entity("SharedLib.PortModelDB", b =>
                {
                    b.HasOne("SharedLib.HardwareModelDB", "Hardware")
                        .WithMany("Ports")
                        .HasForeignKey("HardwareId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Hardware");
                });

            modelBuilder.Entity("SharedLib.ReportModelDB", b =>
                {
                    b.HasOne("SharedLib.TaskModelDB", "OwnerTask")
                        .WithMany("Reports")
                        .HasForeignKey("OwnerTaskId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("OwnerTask");
                });

            modelBuilder.Entity("SharedLib.TaskModelDB", b =>
                {
                    b.HasOne("SharedLib.ScriptModelDB", "Script")
                        .WithMany()
                        .HasForeignKey("ScriptId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Script");
                });

            modelBuilder.Entity("SharedLib.TrigerConditionModelDB", b =>
                {
                    b.HasOne("SharedLib.HardwareModelDB", "Hardware")
                        .WithMany()
                        .HasForeignKey("HardwareId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SharedLib.TrigerModelDB", "Triger")
                        .WithMany("Conditions")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SharedLib.PortModelDB", "Port")
                        .WithMany()
                        .HasForeignKey("PortId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Hardware");

                    b.Navigation("Port");

                    b.Navigation("Triger");
                });

            modelBuilder.Entity("SharedLib.TrigerModelDB", b =>
                {
                    b.HasOne("SharedLib.ScriptModelDB", "Script")
                        .WithMany("Triggers")
                        .HasForeignKey("ScriptId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Script");
                });

            modelBuilder.Entity("SharedLib.UserFormModelDb", b =>
                {
                    b.HasOne("SharedLib.UserModelDB", "OwnerUser")
                        .WithOne("UserForm")
                        .HasForeignKey("SharedLib.UserFormModelDb", "OwnerUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("OwnerUser");
                });

            modelBuilder.Entity("SharedLib.UserFormPropertyModelDb", b =>
                {
                    b.HasOne("SharedLib.UserFormModelDb", "OwnerForm")
                        .WithMany("Properties")
                        .HasForeignKey("OwnerFormId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("OwnerForm");
                });

            modelBuilder.Entity("SharedLib.CommandModelDB", b =>
                {
                    b.Navigation("Conditions");
                });

            modelBuilder.Entity("SharedLib.HardwareModelDB", b =>
                {
                    b.Navigation("Ports");
                });

            modelBuilder.Entity("SharedLib.ScriptModelDB", b =>
                {
                    b.Navigation("Commands");

                    b.Navigation("Contentions");

                    b.Navigation("Triggers");
                });

            modelBuilder.Entity("SharedLib.TaskModelDB", b =>
                {
                    b.Navigation("Reports");
                });

            modelBuilder.Entity("SharedLib.TrigerModelDB", b =>
                {
                    b.Navigation("Conditions");
                });

            modelBuilder.Entity("SharedLib.UserFormModelDb", b =>
                {
                    b.Navigation("Properties");
                });

            modelBuilder.Entity("SharedLib.UserModelDB", b =>
                {
                    b.Navigation("UserForm");
                });
#pragma warning restore 612, 618
        }
    }
}
