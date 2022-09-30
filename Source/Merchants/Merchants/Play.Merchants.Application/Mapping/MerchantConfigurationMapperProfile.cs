using AutoMapper;

using Play.Merchants.Contracts.DTO;
using Play.Merchants.Domain.Entities;

namespace Play.Merchants.Application.Mapping;

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