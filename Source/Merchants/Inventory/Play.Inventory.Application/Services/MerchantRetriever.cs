﻿using System.Net;

using Play.Domain.Exceptions;
using Play.Identity.Api.Client;
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

    /// <exception cref="ApiException"></exception>
    public async Task<Merchant> GetByIdAsync(string id)
    {
        try
        {
            var dto = await _MerchantApi.MerchantGetAsync(id).ConfigureAwait(false) ?? throw new NotFoundException(typeof(Merchant));

            return new Merchant(dto.Id, dto.CompanyName, dto.IsActive);
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
            var dto = _MerchantApi.MerchantGet(id) ?? throw new NotFoundException(typeof(Merchant));

            return new Merchant(dto.Id, dto.CompanyName, dto.IsActive);
        }

        catch (Exception e)
        {
            throw new ApiException(500, e);
        }
    }

    #endregion
}