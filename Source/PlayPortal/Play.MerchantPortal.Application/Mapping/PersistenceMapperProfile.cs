using AutoMapper;
using Play.MerchantPortal.Contracts.DTO;
using MerchantPortal.Core.Entities;

namespace Play.MerchantPortal.Application.Mapping;

public class PersistenceMapperProfile : Profile
{
    public PersistenceMapperProfile()
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
}
