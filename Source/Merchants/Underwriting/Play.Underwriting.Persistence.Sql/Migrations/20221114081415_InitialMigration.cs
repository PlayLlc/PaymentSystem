using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Play.Underwriting.Persistence.Sql.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Individuals",
                columns: table => new
                {
                    Number = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(350)", maxLength: 350, nullable: false),
                    EntityType = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    Program = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    VesselCallSign = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    VesselType = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Tonnage = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: false),
                    GrossRegisteredTonnage = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    VesselFlag = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    VesselOwner = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Individuals", x => x.Number);
                });

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Number = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    IndividualNumber = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    StreetAddress = table.Column<string>(type: "nvarchar(750)", maxLength: 750, nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ZipCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Number);
                    table.ForeignKey(
                        name: "FK_Addresses_Individuals_IndividualNumber",
                        column: x => x.IndividualNumber,
                        principalTable: "Individuals",
                        principalColumn: "Number",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Aliases",
                columns: table => new
                {
                    Number = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    IndividualNumber = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aliases", x => x.Number);
                    table.ForeignKey(
                        name: "FK_Aliases_Individuals_IndividualNumber",
                        column: x => x.IndividualNumber,
                        principalTable: "Individuals",
                        principalColumn: "Number",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_IndividualNumber",
                table: "Addresses",
                column: "IndividualNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Aliases_IndividualNumber",
                table: "Aliases",
                column: "IndividualNumber");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "Aliases");

            migrationBuilder.DropTable(
                name: "Individuals");
        }
    }
}
