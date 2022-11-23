using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Play.Domain.Common.ValueObjects;
using Play.Loyalty.Domain.Aggregates;
using Play.Loyalty.Domain.Repositories;
using Play.Persistence.Sql;

namespace Play.Loyalty.Persistence.Sql.Repositories;

public class LoyaltyProgramRepository : Repository<LoyaltyProgram, SimpleStringId>, ILoyaltyProgramRepository
{
    #region Constructor

    public LoyaltyProgramRepository(DbContext dbContext, ILogger<LoyaltyProgramRepository> logger) : base(dbContext, logger)
    { }

    #endregion

    #region Instance Members

    /// <exception cref="EntityFrameworkRepositoryException"></exception>
    public override async Task<LoyaltyProgram?> GetByIdAsync(SimpleStringId id)
    {
        try
        {
            return await _DbSet.Include("_RewardsProgram").Include("_Discounts").FirstOrDefaultAsync(a => a.Id == id).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            throw new EntityFrameworkRepositoryException(
                $"The {nameof(LoyaltyProgramRepository)} encountered an exception attempting to invoke {nameof(GetByIdAsync)} for the {nameof(id)}: [{id}];",
                ex);
        }
    }

    /// <exception cref="EntityFrameworkRepositoryException"></exception>
    public override LoyaltyProgram? GetById(SimpleStringId id)
    {
        try
        {
            return _DbSet.FirstOrDefault(a => a.Id == id);
        }
        catch (Exception ex)
        {
            throw new EntityFrameworkRepositoryException(
                $"The {nameof(LoyaltyProgramRepository)} encountered an exception attempting to invoke {nameof(GetByIdAsync)} for the {nameof(id)}: [{id}];",
                ex);
        }
    }

    /// <exception cref="EntityFrameworkRepositoryException"></exception>
    public async Task<LoyaltyProgram?> GetByMerchantIdAsync(SimpleStringId merchantId)
    {
        try
        {
            return await _DbSet.Include("_RewardsProgram")
                .Include("_Discounts")
                .FirstOrDefaultAsync(x => EF.Property<SimpleStringId>(x, "_MerchantId") == merchantId)
                .ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            throw new EntityFrameworkRepositoryException(
                $"The {nameof(LoyaltyProgramRepository)} encountered an exception attempting to invoke {nameof(GetByMerchantIdAsync)} for the {nameof(merchantId)}: [{merchantId}];",
                ex);
        }
    }

    #endregion
}