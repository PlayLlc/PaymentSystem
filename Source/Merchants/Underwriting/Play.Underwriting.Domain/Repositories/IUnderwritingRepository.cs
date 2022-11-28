using Play.Underwriting.Domain.Entities;

namespace Play.Underwriting.Domain.Repositories;

public interface IUnderwritingRepository
{
    #region Instance Members

    Task<bool> IsIndustryFound(ushort merchantCategoryCode);

    Task<bool> IsMerchantFound(string name, Address merchantAddress);

    Task<bool> IsUserFound(string name, Address userAddress);

    #endregion
}