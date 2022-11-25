using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Play.Loyalty.Persistence.Sql.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DiscountPrograms",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DiscountId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscountPrograms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MoneyValueObject",
                columns: table => new
                {
                    Amount = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    NumericCurrencyCode = table.Column<ushort>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "RewardProgram",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    PointsPerDollar = table.Column<long>(type: "bigint", nullable: false),
                    PointsRequired = table.Column<long>(type: "bigint", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    NumericCurrencyCode = table.Column<decimal>(type: "decimal(20,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RewardProgram", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rewards",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    NumericCurrencyCode = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    Points = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rewards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Discounts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DiscountProgramId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ItemId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    NumericCurrencyCode = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    VariationId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Discounts_DiscountPrograms_DiscountProgramId",
                        column: x => x.DiscountProgramId,
                        principalTable: "DiscountPrograms",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Programs",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DiscountProgramId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    MerchantId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RewardsProgramId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Programs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Programs_DiscountPrograms_DiscountProgramId",
                        column: x => x.DiscountProgramId,
                        principalTable: "DiscountPrograms",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Programs_RewardProgram_RewardsProgramId",
                        column: x => x.RewardsProgramId,
                        principalTable: "RewardProgram",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MerchantId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RewardsId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RewardsNumber = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Members_Rewards_RewardsId",
                        column: x => x.RewardsId,
                        principalTable: "Rewards",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Discounts_DiscountProgramId",
                table: "Discounts",
                column: "DiscountProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_Members_RewardsId",
                table: "Members",
                column: "RewardsId");

            migrationBuilder.CreateIndex(
                name: "IX_Programs_DiscountProgramId",
                table: "Programs",
                column: "DiscountProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_Programs_RewardsProgramId",
                table: "Programs",
                column: "RewardsProgramId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Discounts");

            migrationBuilder.DropTable(
                name: "Members");

            migrationBuilder.DropTable(
                name: "MoneyValueObject");

            migrationBuilder.DropTable(
                name: "Programs");

            migrationBuilder.DropTable(
                name: "Rewards");

            migrationBuilder.DropTable(
                name: "DiscountPrograms");

            migrationBuilder.DropTable(
                name: "RewardProgram");
        }
    }
}
