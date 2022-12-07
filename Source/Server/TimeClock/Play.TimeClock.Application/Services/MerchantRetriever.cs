using System.Net;

using Play.Domain.Exceptions;
using Play.Identity.Api.Client;
using Play.Identity.Contracts.Dtos;
using Play.Restful.Clients;
using Play.TimeClock.Domain.Entities;
using Play.TimeClock.Domain.Services;

namespace Play.TimeClock.Application.Services;

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

    /// <exception cref="ApiException"></exception>
    public async Task<Merchant> GetByIdAsync(string id)
    {
        try
        {
            MerchantDto dto = await _MerchantApi.GetMerchantAsync(id).ConfigureAwait(false) ?? throw new NotFoundException(typeof(Merchant));

            return new(dto.Id, dto.IsActive);
        }

        catch (Exception e)
        {
            throw new ApiException(HttpStatusCode.InternalServerError, e);
        }
    }

    /// <exception cref="ApiException"></exception>
    public Merchant GetById(string id)
    {
        try
        {
            MerchantDto dto = _MerchantApi.GetMerchant(id) ?? throw new NotFoundException(typeof(Merchant));

            return new(dto.Id, dto.IsActive);
        }

        catch (Exception e)
        {
            throw new ApiException(500, e);
        }
    }

    #endregion
}