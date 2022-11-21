using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Play.Underwriting.Persistence.Sql.Migrations;

public partial class InitialMigration : Migration
{
    #region Instance Members

    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable("Individuals", table => new
        {
            Number = table.Column<decimal>("decimal(20,0)", nullable: false),
            Name = table.Column<string>("nvarchar(350)", maxLength: 350, nullable: false),
            EntityType = table.Column<string>("nvarchar(12)", maxLength: 12, nullable: false),
            Program = table.Column<string>("nvarchar(200)", maxLength: 200, nullable: false),
            Title = table.Column<string>("nvarchar(200)", maxLength: 200, nullable: false),
            VesselCallSign = table.Column<string>("nvarchar(8)", maxLength: 8, nullable: false),
            VesselType = table.Column<string>("nvarchar(25)", maxLength: 25, nullable: false),
            Tonnage = table.Column<string>("nvarchar(14)", maxLength: 14, nullable: false),
            GrossRegisteredTonnage = table.Column<string>("nvarchar(8)", maxLength: 8, nullable: false),
            VesselFlag = table.Column<string>("nvarchar(40)", maxLength: 40, nullable: false),
            VesselOwner = table.Column<string>("nvarchar(150)", maxLength: 150, nullable: false),
            Remarks = table.Column<string>("nvarchar(1000)", maxLength: 1000, nullable: false)
        }, constraints: table =>
        {
            table.PrimaryKey("PK_Individuals", x => x.Number);
        });

        migrationBuilder.CreateTable("Addresses", table => new
        {
            Number = table.Column<decimal>("decimal(20,0)", nullable: false),
            IndividualNumber = table.Column<decimal>("decimal(20,0)", nullable: false),
            StreetAddress = table.Column<string>("nvarchar(750)", maxLength: 750, nullable: false),
            City = table.Column<string>("nvarchar(max)", nullable: false),
            State = table.Column<string>("nvarchar(max)", nullable: false),
            ZipCode = table.Column<string>("nvarchar(max)", nullable: false),
            Country = table.Column<string>("nvarchar(250)", maxLength: 250, nullable: false),
            Remarks = table.Column<string>("nvarchar(200)", maxLength: 200, nullable: false)
        }, constraints: table =>
        {
            table.PrimaryKey("PK_Addresses", x => x.Number);
            table.ForeignKey("FK_Addresses_Individuals_IndividualNumber", x => x.IndividualNumber, "Individuals", "Number",
                onDelete: ReferentialAction.Cascade);
        });

        migrationBuilder.CreateTable("Aliases", table => new
        {
            Number = table.Column<decimal>("decimal(20,0)", nullable: false),
            IndividualNumber = table.Column<decimal>("decimal(20,0)", nullable: false),
            Name = table.Column<string>("nvarchar(max)", nullable: false),
            Type = table.Column<string>("nvarchar(max)", nullable: false),
            Remarks = table.Column<string>("nvarchar(200)", maxLength: 200, nullable: false)
        }, constraints: table =>
        {
            table.PrimaryKey("PK_Aliases", x => x.Number);
            table.ForeignKey("FK_Aliases_Individuals_IndividualNumber", x => x.IndividualNumber, "Individuals", "Number", onDelete: ReferentialAction.Cascade);
        });

        migrationBuilder.CreateIndex("IX_Addresses_IndividualNumber", "Addresses", "IndividualNumber");

        migrationBuilder.CreateIndex("IX_Aliases_IndividualNumber", "Aliases", "IndividualNumber");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable("Addresses");

        migrationBuilder.DropTable("Aliases");

        migrationBuilder.DropTable("Individuals");
    }

    #endregion
}