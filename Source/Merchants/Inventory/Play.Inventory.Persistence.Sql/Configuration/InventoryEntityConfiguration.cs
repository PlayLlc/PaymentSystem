using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Play.Domain.Common.Entities;
using Play.Domain.Common.ValueObjects;
using Play.Inventory.Domain.Aggregates;
using Play.Inventory.Domain.Entities;
using Play.Inventory.Domain.ValueObjects;
using Play.Persistence.Sql;

namespace Play.Inventory.Persistence.Sql.Configuration;

// You can configure a navigation in the model to be included every time the entity is loaded from the database using AutoInclude method
// https://learn.microsoft.com/en-us/ef/core/querying/related-data/eager
internal class InventoryEntityConfiguration : IEntityTypeConfiguration<Item>
{
    #region Instance Members

    public void Configure(EntityTypeBuilder<Item> builder)
    {
        builder.ToTable($"{nameof(Item)}s").HasKey(x => x.Id);

        // Simple Properties
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property<SimpleStringId>($"_MerchantId").HasColumnName("MerchantId");
        builder.Property<Name>($"_Name").HasColumnName($"{nameof(Name)}");
        builder.Property<string>("_Description").HasColumnName("Description");
        builder.Property<Sku>("_Sku").HasColumnName($"{nameof(Sku)}");
        builder.Property<int>("_Quantity").HasColumnName("Quantity");

        // Relationships
        //builder.HasOne<Price>("_Price");

        builder.HasOne<Item, Price, SimpleStringId>("_Price", "PriceId");
        builder.HasOne<Item, Locations, SimpleStringId>("_Locations", "LocationsId");
        builder.HasOne<Item, Alerts, SimpleStringId>("_Alerts", "AlertsId");
        builder.HasMany<Item, Category, SimpleStringId>("_Categories", "CategoriesId");
        builder.HasMany<Item, Variation, SimpleStringId>("_Variations", "VariationsId");
    }

    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable($"Categories").HasKey(x => x.Id);

        // Simple Properties
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property<SimpleStringId>($"_MerchantId").HasColumnName($"MerchantId");
        builder.Property<Name>($"_Name").HasColumnName($"Name");
    }

    #endregion
}