using AutoMapper;

using Play.MerchantPortal.Contracts.DTO;
using Play.MerchantPortal.Domain.Entities;

namespace Play.MerchantPortal.Application.Mapping;

public class MerchantConfigurationMapperProfile : Profile
{
    #region Constructor

    public MerchantConfigurationMapperProfile()
    {
        CreateMap<Company, CompanyDto>();
        CreateMap<Merchant, MerchantDto>();
        CreateMap<Store, StoreDto>();
        CreateMap<Terminal, TerminalDto>();

        CreateMap<CompanyDto, Company>();
        CreateMap<MerchantDto, Merchant>();
        CreateMap<StoreDto, Store>();
        CreateMap<TerminalDto, Terminal>();
    }

    #endregion
}