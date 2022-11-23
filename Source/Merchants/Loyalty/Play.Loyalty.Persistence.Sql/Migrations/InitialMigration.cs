#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace Play.Loyalty.Persistence.Sql.Migrations;

public partial class InitialMigration : Migration
{
    #region Instance Members

    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable("Alerts", table => new
        {
            Id = table.Column<string>("nvarchar(450)", nullable: false),
            IsActive = table.Column<bool>("bit", nullable: false),
            LowInventoryThreshold = table.Column<int>("int", nullable: false)
        }, constraints: table =>
        {
            table.PrimaryKey("PK_Alerts", x => x.Id);
        });

        migrationBuilder.CreateTable("Locations", table => new
        {
            Id = table.Column<string>("nvarchar(450)", nullable: false),
            AllLocations = table.Column<bool>("bit", nullable: false)
        }, constraints: table =>
        {
            table.PrimaryKey("PK_Locations", x => x.Id);
        });

        migrationBuilder.CreateTable("Prices", table => new
        {
            Id = table.Column<string>("nvarchar(450)", nullable: false),
            Amount = table.Column<int>("int", nullable: false),
            NumericCurrencyCode = table.Column<ushort>("int", nullable: false)
        }, constraints: table =>
        {
            table.PrimaryKey("PK_Prices", x => x.Id);
        });

        migrationBuilder.CreateTable("StockActions", table => new {Value = table.Column<string>("nvarchar(450)", nullable: false)}, constraints: table =>
        {
            table.PrimaryKey("PK_StockActions", x => x.Value);
        });

        migrationBuilder.CreateTable("Stores", table => new
        {
            Id = table.Column<string>("nvarchar(450)", nullable: false),
            LocationsId = table.Column<string>("nvarchar(450)", nullable: true)
        }, constraints: table =>
        {
            table.PrimaryKey("PK_Stores", x => x.Id);
            table.ForeignKey("FK_Stores_Locations_LocationsId", x => x.LocationsId, "Locations", "Id");
        });

        migrationBuilder.CreateTable("Items", table => new
        {
            Id = table.Column<string>("nvarchar(450)", nullable: false),
            AlertsId = table.Column<string>("nvarchar(450)", nullable: true),
            CategoriesId = table.Column<string>("nvarchar(max)", nullable: true),
            Description = table.Column<string>("nvarchar(max)", nullable: false),
            LocationsId = table.Column<string>("nvarchar(450)", nullable: true),
            MerchantId = table.Column<string>("nvarchar(max)", nullable: false),
            Name = table.Column<string>("nvarchar(max)", nullable: false),
            PriceId = table.Column<string>("nvarchar(450)", nullable: true),
            Quantity = table.Column<int>("int", nullable: false),
            Sku = table.Column<string>("nvarchar(max)", nullable: true),
            VariationsId = table.Column<string>("nvarchar(max)", nullable: true)
        }, constraints: table =>
        {
            table.PrimaryKey("PK_Items", x => x.Id);
            table.ForeignKey("FK_Items_Alerts_AlertsId", x => x.AlertsId, "Alerts", "Id");
            table.ForeignKey("FK_Items_Locations_LocationsId", x => x.LocationsId, "Locations", "Id");
            table.ForeignKey("FK_Items_Prices_PriceId", x => x.PriceId, "Prices", "Id");
        });

        migrationBuilder.CreateTable("Categories", table => new
        {
            Id = table.Column<string>("nvarchar(450)", nullable: false),
            ItemId = table.Column<string>("nvarchar(450)", nullable: true),
            MerchantId = table.Column<string>("nvarchar(max)", nullable: false),
            Name = table.Column<string>("nvarchar(max)", nullable: false)
        }, constraints: table =>
        {
            table.PrimaryKey("PK_Categories", x => x.Id);
            table.ForeignKey("FK_Categories_Items_ItemId", x => x.ItemId, "Items", "Id");
        });

        migrationBuilder.CreateTable("Variations", table => new
        {
            Id = table.Column<string>("nvarchar(450)", nullable: false),
            ItemId = table.Column<string>("nvarchar(450)", nullable: true),
            Name = table.Column<string>("nvarchar(max)", nullable: false),
            PriceId = table.Column<string>("nvarchar(450)", nullable: true),
            Quantity = table.Column<int>("int", nullable: false),
            Sku = table.Column<string>("nvarchar(max)", nullable: true)
        }, constraints: table =>
        {
            table.PrimaryKey("PK_Variations", x => x.Id);
            table.ForeignKey("FK_Variations_Items_ItemId", x => x.ItemId, "Items", "Id");
            table.ForeignKey("FK_Variations_Prices_PriceId", x => x.PriceId, "Prices", "Id");
        });

        migrationBuilder.InsertData("StockActions", "Value", new object[] {"Restock", "Return", "Shrinkage", "Sold"});

        migrationBuilder.CreateIndex("IX_Categories_ItemId", "Categories", "ItemId");

        migrationBuilder.CreateIndex("IX_Items_AlertsId", "Items", "AlertsId");

        migrationBuilder.CreateIndex("IX_Items_LocationsId", "Items", "LocationsId");

        migrationBuilder.CreateIndex("IX_Items_PriceId", "Items", "PriceId");

        migrationBuilder.CreateIndex("IX_Stores_LocationsId", "Stores", "LocationsId");

        migrationBuilder.CreateIndex("IX_Variations_ItemId", "Variations", "ItemId");

        migrationBuilder.CreateIndex("IX_Variations_PriceId", "Variations", "PriceId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable("Categories");

        migrationBuilder.DropTable("StockActions");

        migrationBuilder.DropTable("Stores");

        migrationBuilder.DropTable("Variations");

        migrationBuilder.DropTable("Items");

        migrationBuilder.DropTable("Alerts");

        migrationBuilder.DropTable("Locations");

        migrationBuilder.DropTable("Prices");
    }

    #endregion
}