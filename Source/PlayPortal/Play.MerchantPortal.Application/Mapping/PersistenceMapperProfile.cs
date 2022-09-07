using AutoMapper;

using Play.MerchantPortal.Contracts.DTO;
using Play.MerchantPortal.Domain.Entities;

namespace Play.MerchantPortal.Application.Mapping;

public class MerchantConfigurationMapperProfile : Profile
{
    #region Constructor

    public MerchantConfigurationMapperProfile()
    {
        CreateMap<CompanyEntity, CompanyDto>();
        CreateMap<MerchantEntity, MerchantDto>();
        CreateMap<StoreEntity, StoreDto>();
        CreateMap<TerminalEntity, TerminalDto>();

        CreateMap<CompanyDto, CompanyEntity>();
        CreateMap<MerchantDto, MerchantEntity>();
        CreateMap<StoreDto, StoreEntity>();
        CreateMap<TerminalDto, TerminalEntity>();
    }

    #endregion
}