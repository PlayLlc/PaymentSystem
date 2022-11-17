using Play.Domain.Common.ValueObjects;
using Play.Identity.Api.Client;
using Play.Identity.Contracts.Dtos;
using Play.Inventory.Domain.Entities;
using Play.Inventory.Domain.Services;
using Play.Restful.Clients;

namespace Play.Inventory.Application.Services;

public class MerchantRetriever : IRetrieveMerchants
{
    #region Instance Values

    private readonly IMerchantApi _MerchantApi;

    #endregion

    #region Constructor

    public MerchantRetriever(IMerchantApi merchantApi)
    {
        _MerchantApi = merchantApi;
    }

    #endregion

    #region Instance Members

    public async Task<Merchant> GetByIdAsync(string id)
    {
        throw new NotImplementedException();
    }

    public Merchant GetById(string id)
    {
        throw new NotImplementedException();
    }

    #endregion
}