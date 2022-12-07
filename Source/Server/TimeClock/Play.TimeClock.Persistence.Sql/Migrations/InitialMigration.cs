using System;

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Play.TimeClock.Persistence.Sql.Migrations;

public partial class InitialMigration : Migration
{
    #region Instance Members

    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable("MoneyValueObject", table => new
            {
                Amount = table.Column<decimal>("decimal(20,0)", nullable: false),
                NumericCurrencyCode = table.Column<ushort>("int", nullable: false)
            }, constraints: table =>
            { });

        migrationBuilder.CreateTable("TimeClocks", table => new
        {
            Id = table.Column<string>("nvarchar(450)", nullable: false),
            ClockedInAt = table.Column<DateTime>("datetime2", nullable: true),
            EmployeeId = table.Column<string>("nvarchar(max)", nullable: false),
            TimeClockStatus = table.Column<string>("nvarchar(max)", nullable: false)
        }, constraints: table =>
        {
            table.PrimaryKey("PK_TimeClocks", x => x.Id);
        });

        migrationBuilder.CreateTable("TimeClockStatus", table => new {Value = table.Column<string>("nvarchar(450)", nullable: false)}, constraints: table =>
        {
            table.PrimaryKey("PK_TimeClockStatus", x => x.Value);
        });

        migrationBuilder.CreateTable("Employees", table => new
        {
            Id = table.Column<string>("nvarchar(450)", nullable: false),
            MerchantId = table.Column<string>("nvarchar(max)", nullable: false),
            TimeEntryId = table.Column<string>("nvarchar(max)", nullable: true),
            TimePuncherId = table.Column<string>("nvarchar(450)", nullable: true),
            UserId = table.Column<string>("nvarchar(max)", nullable: false)
        }, constraints: table =>
        {
            table.PrimaryKey("PK_Employees", x => x.Id);
            table.ForeignKey("FK_Employees_TimeClocks_TimePuncherId", x => x.TimePuncherId, "TimeClocks", "Id");
        });

        migrationBuilder.CreateTable("TimeEntries", table => new
        {
            Id = table.Column<string>("nvarchar(450)", nullable: false),
            EmployeeId1 = table.Column<string>("nvarchar(450)", nullable: true),
            EmployeeId = table.Column<string>("nvarchar(max)", nullable: false),
            EndTime = table.Column<DateTime>("datetime2", nullable: false),
            StartTime = table.Column<DateTime>("datetime2", nullable: false)
        }, constraints: table =>
        {
            table.PrimaryKey("PK_TimeEntries", x => x.Id);
            table.ForeignKey("FK_TimeEntries_Employees_EmployeeId1", x => x.EmployeeId1, "Employees", "Id");
        });

        migrationBuilder.InsertData("TimeClockStatus", "Value", "ClockedIn");

        migrationBuilder.InsertData("TimeClockStatus", "Value", "ClockedOut");

        migrationBuilder.CreateIndex("IX_Employees_TimePuncherId", "Employees", "TimePuncherId");

        migrationBuilder.CreateIndex("IX_TimeEntries_EmployeeId1", "TimeEntries", "EmployeeId1");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable("MoneyValueObject");

        migrationBuilder.DropTable("TimeClockStatus");

        migrationBuilder.DropTable("TimeEntries");

        migrationBuilder.DropTable("Employees");

        migrationBuilder.DropTable("TimeClocks");
    }

    #endregion
}