using AutoMapper;
using Play.MerchantPortal.Contracts.DTO;
using MerchantPortal.WebApi.Models;

namespace MerchantPortal.WebApi.Mapping;

public class ProfileModelMapper : Profile
{
    public ProfileModelMapper()
    {
        CreateMap<UpdateMerchantRequest, MerchantDto>();
        CreateMap<InsertMerchantRequest, MerchantDto>();
        CreateMap<StoreDetailsRequest, StoreDto>();
        CreateMap<TerminalDetailsRequest, TerminalDto>();
    }
}
