using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.HighPerformance.Enumerables;

using Play.Domain.Common.ValueObjects;
using Play.Loyalty.Domain.Aggregates;
using Play.Loyalty.Domain.Repositories;
using Play.Loyalty.Domain.ValueObjects;
using Play.Persistence.Sql;

namespace Play.Loyalty.Persistence.Sql.Repositories;

public class MemberRepository : Repository<Member, SimpleStringId>, IMemberRepository
{
    #region Constructor

    public MemberRepository(DbContext dbContext, ILogger<MemberRepository> logger) : base(dbContext, logger)
    { }

    #endregion

    #region Instance Members

    /// <exception cref="EntityFrameworkRepositoryException"></exception>
    public override async Task<Member?> GetByIdAsync(SimpleStringId id)
    {
        try
        {
            return await _DbSet.Include("_Categories").Include("_Rewards").FirstOrDefaultAsync(a => a.Id == id).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            throw new EntityFrameworkRepositoryException(
                $"The {nameof(MemberRepository)} encountered an exception attempting to invoke {nameof(GetByIdAsync)} for the {nameof(id)}: [{id}];", ex);
        }
    }

    /// <exception cref="EntityFrameworkRepositoryException"></exception>
    public override Member? GetById(SimpleStringId id)
    {
        try
        {
            return _DbSet.Include("_Categories").Include("_Rewards").FirstOrDefault(a => a.Id == id);
        }
        catch (Exception ex)
        {
            throw new EntityFrameworkRepositoryException(
                $"The {nameof(MemberRepository)} encountered an exception attempting to invoke {nameof(GetById)} for the {nameof(id)}: [{id}];", ex);
        }
    }

    /// <exception cref="EntityFrameworkRepositoryException"></exception>
    public async Task RemoveAll(SimpleStringId merchantId)
    {
        try
        {
            List<Member> loyaltyMembers = await _DbSet.Include("_Categories")
                .Include("_Rewards")
                .Where(x => EF.Property<SimpleStringId>(x, "_MerchantId") == merchantId)
                .ToListAsync()
                .ConfigureAwait(false);

            foreach (Member member in loyaltyMembers)
                _DbSet.RemoveRange(loyaltyMembers);

            await _DbContext.SaveChangesAsync().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            throw new EntityFrameworkRepositoryException(
                $"The {nameof(MemberRepository)} encountered an exception attempting to invoke {nameof(RemoveAll)} for the {nameof(merchantId)}: [{merchantId}];",
                ex);
        }
    }

    /// <exception cref="EntityFrameworkRepositoryException"></exception>
    public bool IsRewardsNumberUnique(SimpleStringId merchantId, string rewardsNumber)
    {
        try
        {
            return _DbSet.Any(a => (EF.Property<SimpleStringId>(a, "_MerchantId") == merchantId)
                                   && (EF.Property<RewardsNumber>(a, "_RewardsNumber").Value == rewardsNumber));
        }
        catch (Exception ex)
        {
            throw new EntityFrameworkRepositoryException(
                $"The {nameof(MemberRepository)} encountered an exception attempting to invoke {nameof(RemoveAll)} for the {nameof(merchantId)}: [{merchantId}];",
                ex);
        }
    }

    #endregion
}