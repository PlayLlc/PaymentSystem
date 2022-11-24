using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.HighPerformance.Enumerables;

using Play.Domain.Common.ValueObjects;
using Play.Loyalty.Domain.Aggregates;
using Play.Loyalty.Domain.Repositories;
using Play.Persistence.Sql;

namespace Play.Loyalty.Persistence.Sql.Repositories;

public class LoyaltyMemberRepository : Repository<LoyaltyMember, SimpleStringId>, ILoyaltyMemberRepository
{
    #region Constructor

    public LoyaltyMemberRepository(DbContext dbContext, ILogger<LoyaltyMemberRepository> logger) : base(dbContext, logger)
    { }

    #endregion

    #region Instance Members

    /// <exception cref="EntityFrameworkRepositoryException"></exception>
    public override async Task<LoyaltyMember?> GetByIdAsync(SimpleStringId id)
    {
        try
        {
            return await _DbSet.Include("_Categories").Include("_Rewards").FirstOrDefaultAsync(a => a.Id == id).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            throw new EntityFrameworkRepositoryException(
                $"The {nameof(LoyaltyMemberRepository)} encountered an exception attempting to invoke {nameof(GetByIdAsync)} for the {nameof(id)}: [{id}];",
                ex);
        }
    }

    /// <exception cref="EntityFrameworkRepositoryException"></exception>
    public override LoyaltyMember? GetById(SimpleStringId id)
    {
        try
        {
            return _DbSet.Include("_Categories").Include("_Rewards").FirstOrDefault(a => a.Id == id);
        }
        catch (Exception ex)
        {
            throw new EntityFrameworkRepositoryException(
                $"The {nameof(LoyaltyMemberRepository)} encountered an exception attempting to invoke {nameof(GetById)} for the {nameof(id)}: [{id}];", ex);
        }
    }

    /// <exception cref="EntityFrameworkRepositoryException"></exception>
    public async Task RemoveAll(SimpleStringId merchantId)
    {
        try
        {
            List<LoyaltyMember> loyaltyMembers = await _DbSet.Include("_Categories")
                .Include("_Rewards")
                .Where(x => EF.Property<SimpleStringId>(x, "_MerchantId") == merchantId)
                .ToListAsync()
                .ConfigureAwait(false);

            foreach (LoyaltyMember member in loyaltyMembers)
                _DbSet.RemoveRange(loyaltyMembers);
        }
        catch (Exception ex)
        {
            throw new EntityFrameworkRepositoryException(
                $"The {nameof(LoyaltyMemberRepository)} encountered an exception attempting to invoke {nameof(RemoveAll)} for the {nameof(merchantId)}: [{merchantId}];",
                ex);
        }
    }

    #endregion
}