using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MerchantPortal.Infrastructure.Persistence.Migrations
{
    public partial class InitialEntityModel : Migration
    {
        #region Instance Members

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable("Companies",
                table => new
                {
                    Id = table.Column<long>("bigint", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>("nvarchar(max)", nullable: false)
                }, constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

            migrationBuilder.CreateTable("Merchants",
                table => new
                {
                    Id = table.Column<long>("bigint", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                    AcquirerId = table.Column<string>("nvarchar(max)", nullable: false),
                    MerchantIdentifier = table.Column<string>("nvarchar(max)", nullable: false),
                    MerchantCategoryCode = table.Column<short>("smallint", nullable: false), Name = table.Column<string>("nvarchar(max)", nullable: false),
                    StreetAddress = table.Column<string>("nvarchar(max)", nullable: false), City = table.Column<string>("nvarchar(max)", nullable: false),
                    ZipCode = table.Column<string>("nvarchar(max)", nullable: false), State = table.Column<string>("nvarchar(max)", nullable: false),
                    Country = table.Column<string>("nvarchar(max)", nullable: false), CompanyId = table.Column<long>("bigint", nullable: false)
                }, constraints: table =>
                {
                    table.PrimaryKey("PK_Merchants", x => x.Id);
                    table.ForeignKey("FK_Merchants_Companies_CompanyId", x => x.CompanyId, "Companies", "Id", onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable("Stores",
                table => new
                {
                    Id = table.Column<long>("bigint", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>("nvarchar(max)", nullable: false), Address = table.Column<string>("nvarchar(max)", nullable: false),
                    MerchantId = table.Column<long>("bigint", nullable: false)
                }, constraints: table =>
                {
                    table.PrimaryKey("PK_Stores", x => x.Id);
                    table.ForeignKey("FK_Stores_Merchants_MerchantId", x => x.MerchantId, "Merchants", "Id", onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable("Terminals",
                table => new
                {
                    Id = table.Column<long>("bigint", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                    StoreId = table.Column<long>("bigint", nullable: false)
                }, constraints: table =>
                {
                    table.PrimaryKey("PK_Terminals", x => x.Id);
                    table.ForeignKey("FK_Terminals_Stores_StoreId", x => x.StoreId, "Stores", "Id", onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex("IX_Merchants_CompanyId", "Merchants", "CompanyId");

            migrationBuilder.CreateIndex("IX_Stores_MerchantId", "Stores", "MerchantId");

            migrationBuilder.CreateIndex("IX_Terminals_StoreId", "Terminals", "StoreId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("Terminals");

            migrationBuilder.DropTable("Stores");

            migrationBuilder.DropTable("Merchants");

            migrationBuilder.DropTable("Companies");
        }

        #endregion
    }
}