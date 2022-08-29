using AutoMapper;
using MerchantPortal.Application.DTO;

namespace MerchantPortal.WebApi.Models;

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
