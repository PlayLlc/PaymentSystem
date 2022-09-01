using AutoMapper;
using MerchantPortal.Application.DTO;
using MerchantPortal.WebApi.Models;

namespace MerchantPortal.WebApi.Mapping;

public class ProfileModelMapper : Profile
{
    public ProfileModelMapper()
    {
        CreateMap<MerchantUpdateRequest, MerchantDto>();
        CreateMap<MerchantInsertRequest, MerchantDto>();
        CreateMap<StoreDetails, StoreDto>();
        CreateMap<TerminalDetails, TerminalDto>();
    }
}
