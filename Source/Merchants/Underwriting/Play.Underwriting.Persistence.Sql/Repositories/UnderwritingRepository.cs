using Microsoft.EntityFrameworkCore;
using Play.Underwriting.Domain.Aggregates;
using Play.Underwriting.Domain.Entities;
using Play.Underwriting.Domain.Enums;
using Play.Underwriting.Domain.Repositories;
using Play.Underwriting.Persistence.Persistence;

namespace Play.Underwriting.Persistence.Sql.Repositories;

internal class UnderwritingRepository : IUnderwritingRepository
{
    private readonly UnderwritingDbContext _DbContext;
    private readonly DbSet<Individual> _Individuals;

    public UnderwritingRepository(UnderwritingDbContext dbContext)
    {
        _DbContext = dbContext;
        _Individuals = dbContext.Set<Individual>();
    }

    public async Task<bool> IsIndustryFound(ushort merchantCategoryCode)
    {
        return await Task.FromResult(false);
    }

    public async Task<bool> IsMerchantFound(string name, Address merchantAddress)
    {
        return await _Individuals.AnyAsync(individual => individual.EntityType == EntityType.Empty &&
            (individual.Name.Equals(name) || individual.Aliases.Any(alias => alias.AliasName.Equals(name))) &&
            individual.Addresses.Any(address => address.IsEqual(merchantAddress.StreetAddress, merchantAddress.ZipCode, merchantAddress.City, merchantAddress.State)));
    }

    public async Task<bool> IsUserFound(string name, Address userAddress)
    {
        return await _Individuals.AnyAsync(individual => individual.EntityType == EntityType.Individual &&
            (individual.Name.Equals(name) || individual.Aliases.Any(alias => alias.AliasName.Equals(name))) &&
            individual.Addresses.Any(address => address.IsEqual(userAddress.StreetAddress, userAddress.ZipCode, userAddress.City, userAddress.State)));
    }
}
