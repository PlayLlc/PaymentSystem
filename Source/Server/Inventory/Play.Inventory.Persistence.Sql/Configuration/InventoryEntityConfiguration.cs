﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Play.Domain.Common.ValueObjects;
using Play.Inventory.Domain.Aggregates;
using Play.Inventory.Domain.Entities;
using Play.Inventory.Domain.ValueObjects;
using Play.Persistence.Sql;

namespace Play.Inventory.Persistence.Sql.Configuration;

// You can configure a navigation in the model to be included every time the entity is loaded from the database using AutoInclude method
// https://learn.microsoft.com/en-us/ef/core/querying/related-data/eager
internal class InventoryEntityConfiguration : IEntityTypeConfiguration<Item>, IEntityTypeConfiguration<Category>,
    IEntityTypeConfiguration<Domain.Aggregates.Inventory>
{
    #region Instance Members

    public void Configure(EntityTypeBuilder<Item> builder)
    {
        builder.ToTable($"{nameof(Item)}s").HasKey(x => x.Id);

        // Simple Properties
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property<SimpleStringId>("_MerchantId").HasColumnName("MerchantId");
        builder.Property<Name>("_Name").HasColumnName($"{nameof(Name)}");
        builder.Property<string>("_Description").HasColumnName("Description");
        builder.Property<Sku>("_Sku").HasColumnName($"{nameof(Sku)}");
        builder.Property<int>("_Quantity").HasColumnName("Quantity");

        // Relationships
        //builder.HasOne<Price>("_Price");

        builder.HasOne<Item, Locations, SimpleStringId>("_Locations", "LocationsId");
        builder.HasOne<Item, Alerts, SimpleStringId>("_Alerts", "AlertsId");
        builder.HasMany<Item, Category, SimpleStringId>("_Categories", "CategoriesId");
    }

    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories").HasKey(x => x.Id);

        // Simple Properties
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property<SimpleStringId>("_MerchantId").HasColumnName("MerchantId");
        builder.Property<Name>("_Name").HasColumnName("Name");
    }

    public void Configure(EntityTypeBuilder<Domain.Aggregates.Inventory> builder)
    {
        builder.ToTable("Inventories").HasKey(x => x.Id);

        // Simple Properties
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property<SimpleStringId>("_MerchantId").HasColumnName("MerchantId");
        builder.Property<SimpleStringId>("_StoreId").HasColumnName("_StoreId");
        builder.HasMany<Domain.Aggregates.Inventory, StockItem, SimpleStringId>("_StockItems", "StockItemId");

        // StockItems
    }

    #endregion
}