#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace Play.Loyalty.Persistence.Sql.Migrations;

public partial class InitialMigration : Migration
{
    #region Instance Members

    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable("DiscountPrograms", table => new
        {
            Id = table.Column<string>("nvarchar(450)", nullable: false),
            DiscountId = table.Column<string>("nvarchar(max)", nullable: true),
            IsActive = table.Column<bool>("bit", nullable: false)
        }, constraints: table =>
        {
            table.PrimaryKey("PK_DiscountPrograms", x => x.Id);
        });

        migrationBuilder.CreateTable("MoneyValueObject", table => new
            {
                Amount = table.Column<decimal>("decimal(20,0)", nullable: false),
                NumericCurrencyCode = table.Column<ushort>("int", nullable: false)
            }, constraints: table =>
            { });

        migrationBuilder.CreateTable("RewardProgram", table => new
        {
            Id = table.Column<string>("nvarchar(450)", nullable: false),
            IsActive = table.Column<bool>("bit", nullable: false),
            PointsPerDollar = table.Column<long>("bigint", nullable: false),
            PointsRequired = table.Column<long>("bigint", nullable: false),
            Amount = table.Column<decimal>("decimal(20,0)", nullable: false),
            NumericCurrencyCode = table.Column<decimal>("decimal(20,0)", nullable: false)
        }, constraints: table =>
        {
            table.PrimaryKey("PK_RewardProgram", x => x.Id);
        });

        migrationBuilder.CreateTable("Rewards", table => new
        {
            Id = table.Column<string>("nvarchar(450)", nullable: false),
            Amount = table.Column<decimal>("decimal(20,0)", nullable: false),
            NumericCurrencyCode = table.Column<decimal>("decimal(20,0)", nullable: false),
            Points = table.Column<long>("bigint", nullable: false)
        }, constraints: table =>
        {
            table.PrimaryKey("PK_Rewards", x => x.Id);
        });

        migrationBuilder.CreateTable("Discounts", table => new
        {
            Id = table.Column<string>("nvarchar(450)", nullable: false),
            DiscountProgramId = table.Column<string>("nvarchar(450)", nullable: true),
            ItemId = table.Column<string>("nvarchar(max)", nullable: false),
            Amount = table.Column<decimal>("decimal(20,0)", nullable: false),
            NumericCurrencyCode = table.Column<decimal>("decimal(20,0)", nullable: false),
            VariationId = table.Column<string>("nvarchar(max)", nullable: false)
        }, constraints: table =>
        {
            table.PrimaryKey("PK_Discounts", x => x.Id);
            table.ForeignKey("FK_Discounts_DiscountPrograms_DiscountProgramId", x => x.DiscountProgramId, "DiscountPrograms", "Id");
        });

        migrationBuilder.CreateTable("Programs", table => new
        {
            Id = table.Column<string>("nvarchar(450)", nullable: false),
            DiscountProgramId = table.Column<string>("nvarchar(450)", nullable: true),
            MerchantId = table.Column<string>("nvarchar(max)", nullable: false),
            RewardsProgramId = table.Column<string>("nvarchar(450)", nullable: true)
        }, constraints: table =>
        {
            table.PrimaryKey("PK_Programs", x => x.Id);
            table.ForeignKey("FK_Programs_DiscountPrograms_DiscountProgramId", x => x.DiscountProgramId, "DiscountPrograms", "Id");
            table.ForeignKey("FK_Programs_RewardProgram_RewardsProgramId", x => x.RewardsProgramId, "RewardProgram", "Id");
        });

        migrationBuilder.CreateTable("Members", table => new
        {
            Id = table.Column<string>("nvarchar(450)", nullable: false),
            Email = table.Column<string>("nvarchar(max)", nullable: true),
            MerchantId = table.Column<string>("nvarchar(max)", nullable: false),
            Name = table.Column<string>("nvarchar(max)", nullable: false),
            Phone = table.Column<string>("nvarchar(max)", nullable: false),
            RewardsId = table.Column<string>("nvarchar(450)", nullable: true),
            RewardsNumber = table.Column<string>("nvarchar(max)", nullable: false)
        }, constraints: table =>
        {
            table.PrimaryKey("PK_Members", x => x.Id);
            table.ForeignKey("FK_Members_Rewards_RewardsId", x => x.RewardsId, "Rewards", "Id");
        });

        migrationBuilder.CreateIndex("IX_Discounts_DiscountProgramId", "Discounts", "DiscountProgramId");

        migrationBuilder.CreateIndex("IX_Members_RewardsId", "Members", "RewardsId");

        migrationBuilder.CreateIndex("IX_Programs_DiscountProgramId", "Programs", "DiscountProgramId");

        migrationBuilder.CreateIndex("IX_Programs_RewardsProgramId", "Programs", "RewardsProgramId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable("Discounts");

        migrationBuilder.DropTable("Members");

        migrationBuilder.DropTable("MoneyValueObject");

        migrationBuilder.DropTable("Programs");

        migrationBuilder.DropTable("Rewards");

        migrationBuilder.DropTable("DiscountPrograms");

        migrationBuilder.DropTable("RewardProgram");
    }

    #endregion
}