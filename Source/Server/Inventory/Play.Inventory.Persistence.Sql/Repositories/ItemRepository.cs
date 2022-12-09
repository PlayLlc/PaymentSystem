﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Play.Domain.Common.ValueObjects;
using Play.Inventory.Domain.Aggregates;
using Play.Inventory.Domain.Entities;
using Play.Inventory.Domain.Repositories;
using Play.Persistence.Sql;

namespace Play.Inventory.Persistence.Sql.Repositories;

public class ItemRepository : Repository<Item, SimpleStringId>, IItemRepository
{
    #region Constructor

    public ItemRepository(DbContext dbContext, ILogger<ItemRepository> logger) : base(dbContext, logger)
    { }

    #endregion

    #region Instance Members

    /// <exception cref="EntityFrameworkRepositoryException"></exception>
    public override async Task<Item?> GetByIdAsync(SimpleStringId id)
    {
        try
        {
            Item? result = await _DbSet.Include("_Categories")
                .Include("_Locations")
                .Include("_Variations")
                .Include("_Alerts")
                .FirstOrDefaultAsync(a => a.Id == id)
                .ConfigureAwait(false);

            return result;
        }
        catch (Exception ex)
        {
            throw new EntityFrameworkRepositoryException(
                $"The {nameof(ItemRepository)} encountered an exception attempting to invoke {nameof(GetByIdAsync)} for the {nameof(id)}: [{id}];", ex);
        }
    }

    /// <exception cref="EntityFrameworkRepositoryException"></exception>
    public override Item? GetById(SimpleStringId id)
    {
        try
        {
            Item? result = _DbSet.AsQueryable()
                .Include("_Variations")
                .Include("_Categories")
                .Include("_Locations")
                .Include("_Alerts")
                .FirstOrDefault(a => a.Id == id);

            return result;
        }
        catch (Exception ex)
        {
            throw new EntityFrameworkRepositoryException(
                $"The {nameof(ItemRepository)} encountered an exception attempting to invoke {nameof(GetByIdAsync)} for the {nameof(id)}: [{id}];", ex);
        }
    }

    /// <exception cref="EntityFrameworkRepositoryException"></exception>
    public async Task<IEnumerable<Item>> GetItemsAsync(SimpleStringId merchantId)
    {
        try
        {
            return await _DbSet.AsQueryable()
                .Include("_Variations")
                .Include("_Categories")
                .Include("_Locations")
                .Include("_Alerts")
                .Where(a => EF.Property<SimpleStringId>(a, "_MerchantId") == merchantId)
                .ToListAsync()
                .ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            throw new EntityFrameworkRepositoryException(
                $"The {nameof(ItemRepository)} encountered an exception attempting to invoke {nameof(GetByIdAsync)} for the {nameof(merchantId)}: [{merchantId.Value}];",
                ex);
        }
    }

    /// <exception cref="EntityFrameworkRepositoryException"></exception>
    public async Task<IEnumerable<Item>> GetItemsAsync(SimpleStringId merchantId, SimpleStringId storeId)
    {
        try
        {
            return await _DbSet.Include("_Variations")
                .Include("_Categories")
                .Include("_Locations")
                .Include("_Alerts")
                .Where(a => (EF.Property<SimpleStringId>(a, "_MerchantId") == merchantId)
                            && EF.Property<HashSet<Store>>(a, "_Locations._Stores").Any(a => a.Id == storeId))
                .ToListAsync()
                .ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            throw new EntityFrameworkRepositoryException(
                $"The {nameof(ItemRepository)} encountered an exception attempting to invoke {nameof(GetByIdAsync)} for the {nameof(merchantId)}: [{merchantId.Value}];",
                ex);
        }
    }

    /// <exception cref="EntityFrameworkRepositoryException"></exception>
    public async Task<IEnumerable<Item>> GetItemsAsync(SimpleStringId merchantId, int pageSize, int position)
    {
        try
        {
            List<Item> result = await _DbSet.Include("_Variations")
                .Include("_Categories")
                .Include("_Locations")
                .Include("_Alerts")
                .Where(a => EF.Property<SimpleStringId>(a, "_MerchantId") == merchantId)
                .Skip(position)
                .Take(pageSize)
                .ToListAsync()
                .ConfigureAwait(false);

            return result;
        }
        catch (Exception ex)
        {
            throw new EntityFrameworkRepositoryException(
                $"The {nameof(ItemRepository)} encountered an exception attempting to invoke {nameof(GetByIdAsync)} for the {nameof(merchantId)}: [{merchantId.Value}];",
                ex);
        }
    }

    /// <exception cref="EntityFrameworkRepositoryException"></exception>
    public async Task<IEnumerable<Item>> GetItemsWithAllLocationsSet(SimpleStringId merchantId)
    {
        try
        {
            List<Item> result = await _DbSet.Include("_Variations")
                .Include("_Categories")
                .Include("_Locations")
                .Include("_Alerts")
                .Where(a => (EF.Property<SimpleStringId>(a, "_MerchantId") == merchantId) && EF.Property<bool>(a, "_Locations._AllLocations"))
                .ToListAsync()
                .ConfigureAwait(false);

            return result;
        }
        catch (Exception ex)
        {
            throw new EntityFrameworkRepositoryException(
                $"The {nameof(ItemRepository)} encountered an exception attempting to invoke {nameof(GetByIdAsync)} for the {nameof(merchantId)}: [{merchantId.Value}];",
                ex);
        }
    }

    #endregion
}