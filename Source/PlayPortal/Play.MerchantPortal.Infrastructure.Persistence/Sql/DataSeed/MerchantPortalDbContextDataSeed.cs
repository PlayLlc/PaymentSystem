﻿namespace MerchantPortal.Infrastructure.Persistence.Sql.DataSeed;

internal class MerchantPortalDbContextDataSeed
{
    private readonly MerchantPortalDbContext _dbContext;

    public MerchantPortalDbContextDataSeed(MerchantPortalDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task SeedInitialData()
    {
        throw new NotImplementedException();
    }
}
