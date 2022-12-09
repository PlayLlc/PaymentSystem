using System.Net;

using Play.Domain.Exceptions;
using Play.Identity.Api.Client;
using Play.Identity.Contracts.Dtos;
using Play.Payroll.Contracts.Dtos;
using Play.Payroll.Domain.Aggregates;
using Play.Payroll.Domain.Entities;
using Play.Payroll.Domain.Services;
using Play.Restful.Clients;

using MerchantDto = Play.Identity.Contracts.Dtos.MerchantDto;

namespace Play.Payroll.Application.Services;

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

            return new Merchant(dto.Id, dto.IsActive);
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

            return new Merchant(dto.Id, dto.IsActive);
        }

        catch (Exception e)
        {
            throw new ApiException(500, e);
        }
    }

    #endregion
}