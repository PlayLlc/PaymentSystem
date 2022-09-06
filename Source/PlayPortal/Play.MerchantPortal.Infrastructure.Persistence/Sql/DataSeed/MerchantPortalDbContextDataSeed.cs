namespace MerchantPortal.Infrastructure.Persistence.Sql.DataSeed;

internal class MerchantPortalDbContextDataSeed
{
    #region Instance Values

    private readonly MerchantPortalDbContext _DbContext;

    #endregion

    #region Constructor

    public MerchantPortalDbContextDataSeed(MerchantPortalDbContext dbContext)
    {
        _DbContext = dbContext;
    }

    #endregion

    #region Instance Members

    public Task SeedInitialData()
    {
        throw new NotImplementedException();
    }

    #endregion
}