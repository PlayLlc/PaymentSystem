using AutoMapper;

using Play.MerchantPortal.Api.Models;
using Play.MerchantPortal.Contracts.DTO;

namespace Play.MerchantPortal.Api.Mapping;

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