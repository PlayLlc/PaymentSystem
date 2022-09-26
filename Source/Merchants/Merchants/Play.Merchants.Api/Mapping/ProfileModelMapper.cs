using AutoMapper;

using Play.Merchants.Api.Models;
using Play.Merchants.Contracts.DTO;

namespace Play.Merchants.Api.Mapping;

public class ProfileModelMapper : Profile
{
    #region Constructor

    public ProfileModelMapper()
    {
        CreateMap<UpdateMerchantRequest, MerchantDto>();
        CreateMap<InsertMerchantRequest, MerchantDto>();
        CreateMap<StoreDetailsRequest, StoreDto>();
        CreateMap<TerminalDetailsRequest, TerminalDto>();
    }

    #endregion
}